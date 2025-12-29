using System;

namespace NarrativeSystem.Dialogue.Editor
{
    /// <summary>
    /// Represents a dialogue node that does not require a choice after the dialogue.
    /// </summary>
    /// <remarks>
    /// It is converted to a <see cref="Runtime.DSNoChoiceDialogueNode"/> for the runtime. <br></br>
    /// A <see cref="WaitForInput"/> is automatically inserted after this node during the asset import.
    /// If one is already connected or a <see cref="WaitForSecond"/> is connected in the graph, no additional node is added.
    /// </remarks>
    [Serializable]
    internal class NoChoiceDialogue : BaseDialogue
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionsPorts(context);

            AddCharacterDialoguePort(context);
        }
    }
}
