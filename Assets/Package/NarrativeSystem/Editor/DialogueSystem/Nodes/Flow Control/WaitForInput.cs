using System;

namespace NarrativeSystem.Dialogue.Editor
{
    /// <summary>
    /// Represents a node that waits for player input.
    /// </summary>
    /// <remarks>
    /// It is converted to a <see cref="Runtime.DSWaitForInputNode"/> for the runtime. <br></br>
    /// Some nodes automatically insert this node to ensure the dialogue does not continue without player input.
    /// </remarks>
    [Serializable]
    internal class WaitForInput : Base
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionsPorts(context);
        }
    }
}
