using System;

namespace DialogueSystem.Editor
{
    using Enums;

    [Serializable]
    internal class For : Base
    {
        public const string OUT_PORT_LOOP = "Loop";
        public const string IN_PORT_I = "Index";
        public const string IN_PORT_CONDITION = "End";
        public const string OPTION_PORT_CONDITION = "Condition";
        public const string IN_PORT_JUMP_SIZE = "JumpSize";
        public const string IN_PORT_JUMP = "Jump";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionsPorts(context);

            context.AddOutputPort(OUT_PORT_LOOP).WithDisplayName("Loop");

            context.AddInputPort<int>(IN_PORT_I)
                .WithDefaultValue(0)
                .WithDisplayName("Index")
                .Build();

            context.AddInputPort<int>(IN_PORT_CONDITION)
                .WithDefaultValue(0)
                .WithDisplayName("End Value")
                .Build();

            context.AddInputPort<int> (IN_PORT_JUMP_SIZE)
                .WithDefaultValue(1)
                .WithDisplayName("Step")
                .Build();
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<EConditionalNumber>(OPTION_PORT_CONDITION)
                .WithDefaultValue(EConditionalNumber.Equal)
                .WithTooltip("The condition used to stop the loop.")
                .WithDisplayName("Loop Condition")
                .Build();

            context.AddOption<ELoopJump>(IN_PORT_JUMP)
                .WithDefaultValue(ELoopJump.Add)
                .WithDisplayName("Step Opertaion")
                .Build();
        }
    }
}
