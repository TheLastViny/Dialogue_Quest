using System;

namespace NarrativeSystem.Dialogue.Editor
{
    /// <summary>
    /// Represents the node at the end of a dialogue.
    /// </summary>
    /// <remarks>
    /// It is converted to a <see cref="Runtime.DSEndNode"/> for the runtime.
    /// </remarks>
    [Serializable]    
    internal class End : Base
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputExecutionPort(context);
        }
    }
}
