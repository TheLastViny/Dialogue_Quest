using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace NarrativeSystem.Dialogue.Editor
{
    using Runtime;
    using Variables;


    /// <summary>
    /// DialogueSystemImporter is a <see cref="ScriptedImporter"/> that imports the <see cref="DialogueGraph"/>.
    /// </summary>
    /// <remarks>
    /// It build into a <see cref="DialogueSystemRuntimeGraph"/>
    /// </remarks>
    [ScriptedImporter(1, DialogueGraph.ASSETEXTENSION)]
    internal class DialogueSystemImporter : ScriptedImporter
    {
        private const string DATABASE_PATH = "Assets/Package/DialogueSystem/BlackboardVariablesDatabase.asset";

        /// <summary>
        /// Unity calls this method when the editor imports the asset.
        /// </summary>
        /// <param name="ctx">The asset import context.</param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            DialogueGraph graph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);

            if (graph == null)
            {
                ctx.LogImportError($"Invalid or corrupted DialogueGraph asset at path:\n{ctx.assetPath}.");
                return;
            }

            VariableDatabase database = AssetDatabase.LoadAssetAtPath<VariableDatabase>(DATABASE_PATH);

            if (database == null)
            {
                database = ScriptableObject.CreateInstance<VariableDatabase>();

                AssetDatabase.CreateAsset(database, DATABASE_PATH);
            }
   
            List<IVariable> variables = graph.GetVariables().ToList();

            foreach (IVariable variable in variables)
            {
                Type type = variable.dataType;

                if (type == typeof(string))
                {
                    variable.TryGetDefaultValue(out string value);
                    StringVariable stringVariable = new(value, variable.name);
                    if (!database.Variables.ContainsKey(variable.name))
                    {
                        database.Variables.Add(variable.name, stringVariable);
                    }
                }

                if (type == typeof(int))
                {
                    variable.TryGetDefaultValue(out int value);
                    IntVariable intVariable = new(value, variable.name);
                    if (!database.Variables.ContainsKey(variable.name))
                    {
                        database.Variables.Add(variable.name, intVariable);
                    }
                }


                if (type == typeof(float))
                {
                    variable.TryGetDefaultValue(out float value);
                    FloatVariable floatVariable = new(value, variable.name);
                    if (!database.Variables.ContainsKey(variable.name))
                    {
                        database.Variables.Add(variable.name, floatVariable);
                    }
                }

                if (type == typeof(bool))
                {
                    variable.TryGetDefaultValue(out bool value);
                    BoolVariable boolVariable = new(value, variable.name);
                    if (!database.Variables.ContainsKey(variable.name))
                    {
                        database.Variables.Add(variable.name, boolVariable);
                    }
                }
            }

            EditorUtility.SetDirty(database);

            List<INode> nodes = graph.GetNodes().ToList();

            Start startNode = nodes.OfType<Start>().FirstOrDefault();

            if(startNode == null)
            {
                ctx.LogImportError("Your DialogueGraph does not contain a StartNode.");
                return;
            }

            List<End> endNodes = nodes.OfType<End>().ToList();

            if(endNodes.Count == 0)
            {
                ctx.LogImportError("Your DialogueGraph does not contain an EndNode.");
                return;
            }

            DialogueSystemRuntimeGraph runtimeAsset = ScriptableObject.CreateInstance<DialogueSystemRuntimeGraph>();

            runtimeAsset.StartNodeID = startNode.GetID();

            foreach (INode node in nodes)
            {
                if (node is not Base)
                    continue;

                DSBaseNode runtimeNode = ConvertToRuntimeNode(node, runtimeAsset);
                runtimeAsset.Nodes.Add(runtimeNode.ID, runtimeNode);
            }

            ctx.AddObjectToAsset("RuntimeAsset", runtimeAsset);
            ctx.SetMainObject(runtimeAsset);
        }

        /// <summary>
        /// Convert a node in the graph to a runtime node.
        /// </summary>
        /// <param name="node">The node to convert/</param>
        /// <param name="runtimeGraph">The graph created by the import used in certains case.</param>
        /// <returns>A run time node.</returns>
        /// <exception cref="NotImplementedException">New type of node not implemented in the converter.</exception>
        private DSBaseNode ConvertToRuntimeNode(INode node, DialogueSystemRuntimeGraph runtimeGraph)
        {
            switch (node)
            {
                case Start startNode:
                    return ConvertToRuntimeStartNode(startNode);

                case End endNode:
                    return ConvertToRuntimeEndNode(endNode);

                case NoChoiceDialogue noChoiceDialogue:
                    Base nextNode = noChoiceDialogue.GetOutputPortByName(Base.OUT_PORT_DEFAULT_NAME).firstConnectedPort.GetNode() as Base;
                    if(nextNode is WaitForInput || nextNode is WaitForSecond)
                    {
                        return ConvertToRuntimeNoChoiceDialogueNode(noChoiceDialogue);
                    }
                    else
                    {
                        return ConvertToRuntimeNoChoiceDialogueNode(noChoiceDialogue, runtimeGraph);
                    }

                case ChoiceDialogue choiceDialogueNode:
                    return ConvertToRuntimeChoiceDialogueNode(choiceDialogueNode);

                case WaitForInput waitForInputNode:
                    return ConvertToRuntimeWaitForInputNode(waitForInputNode);

                case WaitForSecond waitForSecondNode:
                    return ConvertToRuntimeWaitForSecondNode(waitForSecondNode);

                default:
                    throw new NotImplementedException($"Conversion for {node.GetType().Name} not implemented");
            }
        }

        #region Convert Nodes

        #region Flow Node
        /// <summary>
        /// Convert a <see cref="Start"/> to a <see cref="DSStartNode"/>.
        /// </summary>
        /// <param name="startNode">The start node to convert.</param>
        /// <returns>A runtime start node.</returns>
        private DSStartNode ConvertToRuntimeStartNode(Start startNode)
        {
            Base nextNode = startNode.GetOutputPortByName(Base.OUT_PORT_DEFAULT_NAME).firstConnectedPort.GetNode() as Base;

            DSStartNode runtimeStartNode = new() 
            { 
                ID = startNode.GetID(),
                NextNodeID = nextNode.GetID()
            };

            return runtimeStartNode;
        }

        /// <summary>
        /// Convert a <see cref="End"/> to a <see cref="DSEndNode"/>.
        /// </summary>
        /// <param name="endNode">The end node to convert.</param>
        /// <returns>A runtime end node.</returns>
        private DSEndNode ConvertToRuntimeEndNode(End endNode)
        {
            DSEndNode runtimeEndNode = new()
            {
                ID = endNode.GetID()
            };

            return runtimeEndNode;
        }

        /// <summary>
        /// Convert a <see cref="WaitForInput"/> to a <see cref="DSWaitForInputNode"/>.
        /// </summary>
        /// <param name="waitForInputNode">The node to convert.</param>
        /// <returns>A runtime wait for input node.</returns>
        private DSWaitForInputNode ConvertToRuntimeWaitForInputNode(WaitForInput waitForInputNode)
        {
            Base nextNode = waitForInputNode.GetOutputPortByName(Base.OUT_PORT_DEFAULT_NAME).firstConnectedPort.GetNode() as Base;

            DSWaitForInputNode runtimeWaitForInputNode = new()
            {
                ID= waitForInputNode.GetID(),
                NextNodeID = nextNode.GetID()
            };

            return runtimeWaitForInputNode;
        }

        /// <summary>
        /// Convert a <see cref="WaitForInput"/> to a <see cref="DSWaitForInputNode"/>.
        /// This method is used when a node need to create a wait for input node within the import.
        /// </summary>
        /// <param name="waitForInputNode">The node to convert.</param>
        /// <param name="node">The node to find the next node.</param>
        /// <returns>A runtime wait for input node.</returns>
        private DSWaitForInputNode ConvertToRuntimeWaitForInputNode(WaitForInput waitForInputNode, Base node)
        {
            Base nextNode = node.GetOutputPortByName(Base.OUT_PORT_DEFAULT_NAME).firstConnectedPort.GetNode() as Base;

            DSWaitForInputNode runtimeWaitForInputNode = new()
            {
                ID = waitForInputNode.GetID(),
                NextNodeID = nextNode.GetID()
            };

            return runtimeWaitForInputNode;
        }

        private DSWaitForSecondNode ConvertToRuntimeWaitForSecondNode(WaitForSecond waitForSecondNode)
        {
            Base nextNode = waitForSecondNode.GetOutputPortByName(Base.OUT_PORT_DEFAULT_NAME).firstConnectedPort.GetNode() as Base;

            FloatRef secondRef = GetFloatRef(waitForSecondNode.GetInputPortByName(WaitForSecond.IN_PORT_NUMBER_SECONDS));

            DSWaitForSecondNode runtimeWaitForSecondNode = new()
            {
                ID = waitForSecondNode.GetID(),
                NextNodeID = nextNode.GetID(),
                Seconds = secondRef
            };

            return runtimeWaitForSecondNode;
        }

        #endregion

        #region Logic Node

   

        #endregion

        #region Dialogue Node
        /// <summary>
        /// Convert a <see cref="NoChoiceDialogue"/> to a <see cref="DSNoChoiceDialogueNode"/>.
        /// </summary>
        /// <param name="noChoiceDialogueNode">The node to convert.</param>
        /// <returns>A runtime no choice dialogue node.</returns>
        private DSNoChoiceDialogueNode ConvertToRuntimeNoChoiceDialogueNode(NoChoiceDialogue noChoiceDialogueNode)
        {
            Base nextNode = noChoiceDialogueNode.GetOutputPortByName(Base.OUT_PORT_DEFAULT_NAME).firstConnectedPort.GetNode() as Base;

            StringRef dialogueRef = GetStringRef(noChoiceDialogueNode.GetInputPortByName(BaseDialogue.IN_PORT_DIALOGUE_NAME));
            StringRef chracterRef = GetStringRef(noChoiceDialogueNode.GetInputPortByName(BaseDialogue.IN_PORT_CHARACTER_NAME));

            DSNoChoiceDialogueNode runtimeNoChoiceDialogueNode = new()
            {
                ID = noChoiceDialogueNode.GetID(),
                NextNodeID = nextNode.GetID(),
                Dialogue = dialogueRef,
                CharacterName = chracterRef
            };

            return runtimeNoChoiceDialogueNode;
        }

        /// <summary>
        /// Convert a <see cref="NoChoiceDialogue"/> to a <see cref="DSNoChoiceDialogueNode"/>.
        /// This method is use when their is no <see cref="WaitForInput"/> after the node.
        /// </summary>
        /// <param name="noChoiceDialogueNode">The node to convert.</param>
        /// <param name="runtimeGraph">The graph to add the wait for input node.</param>
        /// <returns>A runtime no choice dialogue node.</returns>
        private DSNoChoiceDialogueNode ConvertToRuntimeNoChoiceDialogueNode(NoChoiceDialogue noChoiceDialogueNode, DialogueSystemRuntimeGraph runtimeGraph)
        {
            WaitForInput waitForInputNode = new WaitForInput();
            DSWaitForInputNode dswaitForInputNode = ConvertToRuntimeWaitForInputNode(waitForInputNode, noChoiceDialogueNode);
            runtimeGraph.Nodes.Add(dswaitForInputNode.ID, dswaitForInputNode);

            StringRef dialogueRef = GetStringRef(noChoiceDialogueNode.GetInputPortByName(BaseDialogue.IN_PORT_DIALOGUE_NAME));
            StringRef chracterRef = GetStringRef(noChoiceDialogueNode.GetInputPortByName(BaseDialogue.IN_PORT_CHARACTER_NAME));

            DSNoChoiceDialogueNode runtimeNoChoiceDialogueNode = new()
            {
                ID = noChoiceDialogueNode.GetID(),
                NextNodeID = waitForInputNode.GetID(),
                Dialogue = dialogueRef,
                CharacterName = chracterRef
            };

            return runtimeNoChoiceDialogueNode;
        }

        /// <summary>
        /// Convert a <see cref="ChoiceDialogue"/> to a <see cref="DSChoiceDialogueNode"/>.
        /// </summary>
        /// <param name="choiceDialogueNode">The node to convert.</param>
        /// <returns>A runtime choice dialogue node.</returns>
        private DSChoiceDialogueNode ConvertToRuntimeChoiceDialogueNode(ChoiceDialogue choiceDialogueNode)
        {
            StringRef dialogueRef = GetStringRef(choiceDialogueNode.GetInputPortByName(BaseDialogue.IN_PORT_DIALOGUE_NAME));
            StringRef chracterRef = GetStringRef(choiceDialogueNode.GetInputPortByName(BaseDialogue.IN_PORT_CHARACTER_NAME));

            DSChoiceDialogueNode runtimeChoiceDialogueNode = new DSChoiceDialogueNode() 
            { 
                ID = choiceDialogueNode.GetID(),
                Dialogue = dialogueRef,
                CharacterName = chracterRef,
            };

            INodeOption optionNode = choiceDialogueNode.GetNodeOptionByName(ChoiceDialogue.PORT_OPTION_COUNT);
            optionNode.TryGetValue(out int portCount);
            
            for(int i = 0; i < portCount; i++)
            {
                StringRef textRef = GetStringRef(choiceDialogueNode.GetInputPortByName($"Choice Text {i + 1}"));
                Base nextNode = choiceDialogueNode.GetOutputPortByName($"Choice {i + 1}").firstConnectedPort.GetNode() as Base;

                TextChoice textChoice = new TextChoice() 
                { 
                    Text = textRef,
                    NextNodeID = nextNode.GetID()
                };

                runtimeChoiceDialogueNode.Choices.Add(textChoice);
            }

            return runtimeChoiceDialogueNode;
        }
        #endregion

        #endregion

        #region Utility

        /// <summary>
        /// Gets the value of an input port on a node.
        /// <br/><br/>
        /// The value is obtained from (in priority order):<br/>
        /// 1. Connections to the port (variable nodes, constant nodes, wire portals)<br/>
        /// 2. Embedded value on the port<br/>
        /// 3. Default value of the port<br/>
        /// </summary>
        static T GetInputPortValue<T>(IPort port, out bool isRef)
        {
            T value = default;

            // If port is connected to another node, get value from connection
            if (port.isConnected)
            {
                switch (port.firstConnectedPort.GetNode())
                {
                    case IVariableNode variableNode:
                        variableNode.variable.TryGetDefaultValue<T>(out value);
                        isRef = true;
                        return value;
                    case IConstantNode constantNode:
                        constantNode.TryGetValue<T>(out value);
                        isRef = false;
                        return value;
                    default:
                        break;
                }
            }
            else
            {
                // If port has embedded value, return it.
                // Otherwise, return the default value of the port
                port.TryGetValue(out value);
            }

            isRef = false;
            return value;
        }

        static string GetInputPortName(IPort port)
        {
            if(port.isConnected)
            {
                INode node = port.firstConnectedPort.GetNode();
                if(node is IVariableNode variableNode)
                {
                    return variableNode.variable.name;
                }
            }

            throw new InvalidOperationException("Input port is not connected to a variable node.");
        }

        static StringRef GetStringRef(IPort portName)
        {
            StringRef stringRef;

            string text = GetInputPortValue<string>(portName, out bool isRef);
            if (isRef)
            {
                stringRef = new VariableString(GetInputPortName(portName));
            }
            else
            {
                stringRef = new UnvariableString(text);
            }

            return stringRef;
        }


        static IntRef GetIntRef(IPort portName)
        {
            IntRef intRef;

            int number = GetInputPortValue<int>(portName, out bool isRef);
            if (isRef)
            {
                intRef = new VariableInt(GetInputPortName(portName));
            }
            else
            {
                intRef = new UnvariableInt(number);
            }

            return intRef;
        }

        static FloatRef GetFloatRef(IPort portName)
        {
            FloatRef floatRef;

            float number = GetInputPortValue<float>(portName, out bool isRef);
            if (isRef)
            {
                floatRef = new VariableFloat(GetInputPortName(portName));
            }
            else
            {
                floatRef = new UnvariableFloat(number);
            }

            return floatRef;
        }

        #endregion
    }
}
