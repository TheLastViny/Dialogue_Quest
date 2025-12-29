using System;

namespace NarrativeSystem.Dialogue.Editor
{
    using Enums;

    /// <summary>
    /// Represents a conditional node with a float condition.
    /// </summary>
    /// <remarks>
    /// It is converted to a <see cref="Runtime.DSNumberConditionalNode"/> for the runtime.
    /// </remarks>
    [Serializable]
    internal class FloatConditional : BaseConditional
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputExecutionPort(context);

            DefineCondition<float>(context);

            AddOutputCondition(context);
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<EConditionalNumber>(OPTION_PORT_OPERATOR)
                .WithDisplayName("Operator")
                .WithDefaultValue(EConditionalNumber.Equal)
                .WithTooltip("The first condition will be on the left and the second one one the right.")
                .Build();
        }
    }
}
