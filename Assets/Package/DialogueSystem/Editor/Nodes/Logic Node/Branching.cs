using System;
using UnityEngine;

namespace NarrativeSystem.Dialogue.Editor
{
    [Serializable]
    internal class Branching : Base
    {
        public const string IN_PORT_CONDITION = "Condition";
        public const string OUT_PORT_TRUE = "True";
        public const string OUT_PORT_FALSE = "False";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputExecutionPort(context);

            context.AddInputPort<bool>(IN_PORT_CONDITION)
                .WithDisplayName("Condition")
                .Build();

            context.AddOutputPort(OUT_PORT_TRUE)
                .WithDisplayName("True")
                .Build();

            context.AddOutputPort(OUT_PORT_FALSE)
                .WithDisplayName("False")
                .Build();
        }

    }
}

