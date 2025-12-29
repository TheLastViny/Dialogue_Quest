using System;

namespace NarrativeSystem.Dialogue.Editor
{
    /// <summary>
    /// Represents a wait node with an number of secods.
    /// </summary>
    /// <remarks>
    /// It is converted to a <see cref="Runtime.DSWaitForSecondNode"/> for the runtime.
    /// </remarks>
    [Serializable]
    internal class WaitForSecond : Base
    {
        public const string IN_PORT_NUMBER_SECONDS = "NumberSeconds";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionsPorts(context);

            context.AddInputPort<float>(IN_PORT_NUMBER_SECONDS)
                .WithDefaultValue(0)
                .WithDisplayName("Seconds")
                .WithDefaultValue(0)
                .Build();
        }
    }
}
