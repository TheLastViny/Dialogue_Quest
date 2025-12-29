using UnityEngine;

namespace DialogueSystem.Editor
{
    internal class DontWaitForInput : Base
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionsPorts(context);
        }
    }
}
