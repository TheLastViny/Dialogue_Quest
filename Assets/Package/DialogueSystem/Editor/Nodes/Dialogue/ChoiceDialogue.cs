using System;
using Unity.GraphToolkit.Editor;

namespace NarrativeSystem.Dialogue.Editor
{
    /// <summary>
    /// Represents a dialogue node that does require a choice after the dialogue.
    /// </summary>
    /// <remarks>
    /// It is converted to a <see cref="Runtime.DSChoiceDialogueNode"/> for the runtime. 
    /// </remarks>
    [Serializable]
    internal class ChoiceDialogue : BaseDialogue
    {
        public const string PORT_OPTION_COUNT = "PortCount";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputExecutionPort(context);

            AddCharacterDialoguePort(context);

            INodeOption option = GetNodeOptionByName(PORT_OPTION_COUNT);
            option.TryGetValue(out int portCount);
            for (int i = 0; i < portCount; i++)
            {
                context.AddInputPort<string>($"Choice Text {i + 1}").Build();
                context.AddOutputPort($"Choice {i + 1}").Build();
            }
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(PORT_OPTION_COUNT)
                .WithDisplayName("Number of options")
                .WithDefaultValue(2)
                .WithTooltip("Number of options availabe for this node")
                .Delayed();
        }
    }
}
