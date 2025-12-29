using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Represents the graph for the dialogue system.
    /// </summary>
    [Serializable]
    [Graph(ASSETEXTENSION)]
    internal class DialogueGraph : Graph
    {
        public const string ASSETEXTENSION = "nsdg";

        [MenuItem("Assets/Create/Narrative System/Dialogue Graph", false)]
        [MenuItem("Window/Narrative System/Dialogue Graph", false)]
        private static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
        }

        public override void OnGraphChanged(GraphLogger graphLogger)
        {
            base.OnGraphChanged(graphLogger);

            CheckGraphErrors(graphLogger);
        }

        private void CheckGraphErrors(GraphLogger graphLogger)
        {
            ValidateStartNode(graphLogger);

            ValidateEndNode(graphLogger);

            ValidateWaitForInputNode(graphLogger);

            ValidateWaitForSecondNode(graphLogger);

            ValidateNoChoiceDialogueNode(graphLogger);

            ValidateChoiceDialogueNode(graphLogger);

            ValidateConditionalFloatNode(graphLogger);

            ValidateConditionalIntNode(graphLogger);

            ValidateConditionalStringNode(graphLogger);
        }

        #region Node Validation Region

        #region Flow Node
        private void ValidateStartNode(GraphLogger graphLogger)
        {
            List<Start> startNodes = GetNodes().OfType<Start>().ToList();

            switch(startNodes.Count)
            {
                case 0:
                    graphLogger.LogError("Your dialogue graph must containt 1 start node, you have 0.");
                    break;

                case 1:
                    
                    Start startNode = startNodes[0];
                    IPort outputPort = startNode.GetOutputPortByName(Base.OUT_PORT_DEFAULT_NAME);

                    if (!outputPort.isConnected)
                    {
                        graphLogger.LogError($"{startNode.GetType().Name} output port is not connected.", startNode);
                    }

                    List<IPort> outputConnectedPorts = new List<IPort>();
                    outputPort.GetConnectedPorts(outputConnectedPorts);

                    if(outputConnectedPorts.Count >= 2)
                    {
                        graphLogger.Log("Only the line with the number 1 will be used the rest will be ignore for the output port.", startNode);
                    }  
                    break;

                case > 1:
                    foreach (Start node in startNodes.Skip(1))
                    {
                        graphLogger.LogError("Your dialogue graph must contain 1 start node, you have too many.", node);
                    }
                    break;
            }
        }

        private void ValidateEndNode(GraphLogger graphLogger)
        {
            List<End> endNodes = GetNodes().OfType<End>().ToList();
            switch (endNodes.Count)
            {
                case 0:
                    graphLogger.LogError("Your dialogue graph must containt at least 1 end node, you have 0.");
                    break;

                case >= 1:
                    foreach (End node in endNodes)
                    {
                        IPort inputPort = node.GetInputPortByName(Base.IN_PORT_DEFAULT_NAME);

                        if (!inputPort.isConnected) 
                        { 
                            graphLogger.LogError($"{node.GetType().Name} input port is not connected.", node);
                        }
                    }
                    break; 
            } 
        }

        private void ValidateWaitForInputNode(GraphLogger graphLogger)
        {
            List<WaitForInput> inputNodes = GetNodes().OfType<WaitForInput>().ToList();

            foreach (WaitForInput node in inputNodes)
            {
                IPort outputPort = node.GetOutputPortByName(Base.OUT_PORT_DEFAULT_NAME);

                if (!outputPort.isConnected)
                {
                    graphLogger.LogError($"{node.GetType().Name} output port is not connected.", node);
                }

                IPort inputPort = node.GetInputPortByName(Base.IN_PORT_DEFAULT_NAME);

                if (!inputPort.isConnected)
                {
                    graphLogger.LogError($"{node.GetType().Name} input port is not connected.", node);
                }


                List<IPort> outputConnectedPorts = new List<IPort>();
                outputPort.GetConnectedPorts(outputConnectedPorts);

                if (outputConnectedPorts.Count >= 2)
                {
                    graphLogger.LogWarning("Only the line with the number 1 will be used the rest will be ignore for the output port.", node);
                }
            }
        }

        private void ValidateWaitForSecondNode(GraphLogger graphLogger)
        {
            List<WaitForSecond> secondNodes = GetNodes().OfType<WaitForSecond>().ToList();

            foreach (WaitForSecond node in secondNodes)
            {
                IPort outputPort = node.GetOutputPortByName(Base.OUT_PORT_DEFAULT_NAME);

                if (!outputPort.isConnected)
                {
                    graphLogger.LogError($"{node.GetType().Name} output port is not connected.", node);
                }

                IPort inputPort = node.GetInputPortByName(Base.IN_PORT_DEFAULT_NAME);

                if (!inputPort.isConnected)
                {
                    graphLogger.LogError($"{node.GetType().Name} input port is not connected.", node);
                }

                IPort secondPort = node.GetInputPortByName(WaitForSecond.IN_PORT_NUMBER_SECONDS);
                secondPort.TryGetValue(out float seconds);
                
                if (seconds == 0f)
                {
                    graphLogger.Log($"{node.GetType().Name} option port is equal to 0 seconds it will skip that node.", node);
                }

                List<IPort> outputConnectedPorts = new List<IPort>();
                outputPort.GetConnectedPorts(outputConnectedPorts);

                if (outputConnectedPorts.Count >= 2)
                {
                    graphLogger.LogWarning("Only the line with the number 1 will be used the rest will be ignore for the output port.", node);
                }
            }
        }

        #endregion

        #region Logic Node

        private void ValidateConditionalIntNode(GraphLogger graphLogger)
        {
            List<IntConditional> inputNodes = GetNodes().OfType<IntConditional>().ToList();

            foreach (IntConditional node in inputNodes)
            {
                IPort firstConditionPort = node.GetInputPortByName(BaseConditional.IN_PORT_FIRST_CONDITION);

                if (!firstConditionPort.isConnected)
                {
                    if (!firstConditionPort.TryGetValue(out int firstCondotion) || firstCondotion == 0)
                    {
                        graphLogger.Log($"{node.GetType().Name} first condition port is equal to 0 by default if you do not change it.", node);
                    }
                }

                IPort secondConditionPort = node.GetInputPortByName(BaseConditional.IN_PORT_SECOND_CONDITION);

                if (!secondConditionPort.isConnected)
                {
                    if (!secondConditionPort.TryGetValue(out int secondCondition) || secondCondition == 0)
                    {
                        graphLogger.Log($"{node.GetType().Name} second condition port is equal to 0 by default if you do not change it.", node);
                    }
                }
            }
        }

        private void ValidateConditionalFloatNode(GraphLogger graphLogger)
        {
            List<FloatConditional> inputNodes = GetNodes().OfType<FloatConditional>().ToList();

            foreach (FloatConditional node in inputNodes)
            {   
                IPort firstConditionPort = node.GetInputPortByName(BaseConditional.IN_PORT_FIRST_CONDITION);

                if (!firstConditionPort.isConnected)
                {
                    if (!firstConditionPort.TryGetValue(out float firstCondotion) || firstCondotion == 0)
                    {
                        graphLogger.Log($"{node.GetType().Name} first condition port is equal to 0 by default if you do not change it.", node);
                    }
                }

                IPort secondConditionPort = node.GetInputPortByName(BaseConditional.IN_PORT_SECOND_CONDITION);

                if (!secondConditionPort.isConnected)
                {
                    if (!secondConditionPort.TryGetValue(out float secondCondition) || secondCondition == 0)
                    {
                        graphLogger.Log($"{node.GetType().Name} second condition port is equal to 0 by default if you do not change it.", node);
                    }
                }
            }
        }

        private void ValidateConditionalStringNode(GraphLogger graphLogger)
        {
            List<StringConditional> inputNodes = GetNodes().OfType<StringConditional>().ToList();

            foreach (StringConditional node in inputNodes)
            {
                IPort firstConditionPort = node.GetInputPortByName(BaseConditional.IN_PORT_FIRST_CONDITION);

                if (!firstConditionPort.isConnected)
                {
                    if (!firstConditionPort.TryGetValue(out string firstCondotion) || string.IsNullOrEmpty(firstCondotion))
                    {
                        graphLogger.LogWarning($"{node.GetType().Name} first condition port is not connected or doesn't containt a value.", node);
                    }
                }

                IPort secondConditionPort = node.GetInputPortByName(BaseConditional.IN_PORT_SECOND_CONDITION);

                if (!secondConditionPort.isConnected)
                {
                    if (!secondConditionPort.TryGetValue(out string secondCondition) || string.IsNullOrEmpty(secondCondition))
                    {
                        graphLogger.LogWarning($"{node.GetType().Name} second condition port is not connected or doesn't containt a value.", node);
                    }
                }
            }
        }

        #endregion

        #region Dialogue Node
        private void ValidateNoChoiceDialogueNode(GraphLogger graphLogger)
        {
            List<NoChoiceDialogue> noChoiceNodes = GetNodes().OfType<NoChoiceDialogue>().ToList();

            foreach (NoChoiceDialogue node in noChoiceNodes)
            {

                IPort outputPort = node.GetOutputPortByName(Base.OUT_PORT_DEFAULT_NAME);

                if (!outputPort.isConnected)
                {
                    graphLogger.LogError($"{node.GetType().Name} output port is not connected.", node);
                }

                List<IPort> outputConnectedPorts = new List<IPort>();
                outputPort.GetConnectedPorts(outputConnectedPorts);

                if (outputConnectedPorts.Count >= 2)
                {
                    graphLogger.LogWarning("Only the line with the number 1 will be used the rest will be ignore for the output port.", node);
                }

                IPort inputPort = node.GetInputPortByName(Base.IN_PORT_DEFAULT_NAME);

                if (!inputPort.isConnected)
                {
                    graphLogger.LogError($"{node.GetType().Name} input port is not connected.", node);
                }

                IPort characterNamePort = node.GetInputPortByName(BaseDialogue.IN_PORT_CHARACTER_NAME);

                if (!characterNamePort.isConnected)
                {
                    if (!characterNamePort.TryGetValue(out string text) || string.IsNullOrEmpty(text))
                    {
                        graphLogger.LogWarning($"{node.GetType().Name} character port is not connected or doesn't containt a value.", node);
                    }
                }

                IPort dialogueNamePort = node.GetInputPortByName(BaseDialogue.IN_PORT_DIALOGUE_NAME);

                if (!dialogueNamePort.isConnected)
                {
                    if (!dialogueNamePort.TryGetValue(out string text) || string.IsNullOrEmpty(text))
                    {
                        graphLogger.LogWarning($"{node.GetType().Name} dialogue port is not connected or doesn't containt a value.", node);
                    }
                }
            }
        }

        private void ValidateChoiceDialogueNode(GraphLogger graphLogger)
        {
            List<ChoiceDialogue> noChoiceNodes = GetNodes().OfType<ChoiceDialogue>().ToList();

            foreach (ChoiceDialogue node in noChoiceNodes)
            {
                IPort inputPort = node.GetInputPortByName(Base.IN_PORT_DEFAULT_NAME);

                if (!inputPort.isConnected)
                {
                    graphLogger.LogError($"{node.GetType().Name} input port is not connected.", node);
                }

                IPort characterNamePort = node.GetInputPortByName(BaseDialogue.IN_PORT_CHARACTER_NAME);

                if (!characterNamePort.isConnected)
                {
                    if (!characterNamePort.TryGetValue(out string text) || string.IsNullOrEmpty(text))
                    {
                        graphLogger.LogWarning($"{node.GetType().Name} character port is not connected or doesn't containt a value.", node);
                    }
                }

                IPort dialogueNamePort = node.GetInputPortByName(BaseDialogue.IN_PORT_DIALOGUE_NAME);

                if (!dialogueNamePort.isConnected)
                {
                    if (!dialogueNamePort.TryGetValue(out string text) || string.IsNullOrEmpty(text))
                    {
                        graphLogger.LogWarning($"{node.GetType().Name} dialogue port is not connected or doesn't containt a value.", node);
                    }
                }

                INodeOption option = node.GetNodeOptionByName(ChoiceDialogue.PORT_OPTION_COUNT);
                option.TryGetValue(out int portCount);
                switch (portCount)
                {
                    case 0:
                        graphLogger.Log("Your node doesn't have any options, you can use NoChoiceDialogueNode instead.", node);
                        break;
                    case 1:
                        graphLogger.Log("Your node only have one option, you could use NoChoiceDialogueNode instead if you dont need the option.", node);
                        break;
                }

                for (int i = 0; i < portCount; i++)
                {
                    IPort choiceTextPort = node.GetInputPortByName($"Choice Text {i + 1}");
                    if (!choiceTextPort.isConnected)
                    {
                        if(!choiceTextPort.TryGetValue(out string text) || string.IsNullOrEmpty(text))
                        {
                            graphLogger.LogWarning($"{node.GetType().Name} choice text {i + 1} is not connected or doesn't containt a value.", node);
                        }
                    }

                    IPort choicePort = node.GetOutputPortByName($"Choice {i + 1}");
                    if (!choicePort.isConnected)
                    {
                        graphLogger.LogError($"{node.GetType().Name} choice {i + 1} port is not connected.", node);
                    }

                    List<IPort> outputConnectedPorts = new List<IPort>();
                    choicePort.GetConnectedPorts(outputConnectedPorts);

                    if (outputConnectedPorts.Count >= 2)
                    {
                        graphLogger.LogWarning($"Only the line with the number 1 will be used the rest will be ignore for the choice {i + 1}  port.", node);
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
