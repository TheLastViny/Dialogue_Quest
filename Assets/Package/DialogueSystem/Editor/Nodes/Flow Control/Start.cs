using System;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Represents the node at the start of a dialogue.
    /// </summary>
    /// <remarks>
    /// It is converted to a <see cref="Runtime.DSStartNode"/> for the runtime.
    /// </remarks>
    [Serializable]
    internal class Start : Base
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddOutputExecutionPort(context);
        }
    }
}
