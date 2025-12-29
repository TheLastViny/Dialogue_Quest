using System;

namespace NarrativeSystem.Dialogue.Editor
{
    /// <summary>
    /// Represents the base model for all conditional nodes
    /// </summary>
    [Serializable]
    internal abstract class BaseConditional : Base
    {
        public const string IN_PORT_FIRST_CONDITION = "FirstCondition";
        public const string IN_PORT_SECOND_CONDITION = "SecondCondition";
        public const string OUT_PORT_RESULT = "Result";
        public const string OPTION_PORT_OPERATOR = "Operator";


        protected void DefineCondition<T>(IPortDefinitionContext context)
        {
            context.AddInputPort<T>(IN_PORT_FIRST_CONDITION)
                .WithDisplayName("First Condition")
                .Build();

            context.AddInputPort<T>(IN_PORT_SECOND_CONDITION)
                .WithDisplayName("Second Condition")
                .Build();
        }

        protected void AddOutputCondition(IPortDefinitionContext context)
        {
            context.AddOutputPort<bool>(OUT_PORT_RESULT)
                .WithDisplayName("True")
                .Build();
        }
    }
}
