using UnityEngine;

namespace NarrativeSystem.Dialogue.Editor
{
    internal class DontWaitForInput : Base
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionsPorts(context);
        }
    }
}
