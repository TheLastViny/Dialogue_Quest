using System;

namespace DialogueSystem.Editor
{
    using Enums;

    /// <summary>
    /// Represents a conditional node with a string condition.
    /// </summary>
    /// <remarks>
    /// It is converted to a <see cref="Runtime.DSStringConditionalNode"/> for the runtime.
    /// </remarks>
    [Serializable]
    internal class StringConditional : BaseConditional
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DefineCondition<string>(context);

            AddOutputCondition(context);
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<EConditionalString>(OPTION_PORT_OPERATOR)
                .WithDisplayName("Operator")
                .WithDefaultValue(EConditionalString.Equal)
                .WithTooltip("The first condition will be on the left and the second one one the right.")
                .Build();
        }
    }
}
