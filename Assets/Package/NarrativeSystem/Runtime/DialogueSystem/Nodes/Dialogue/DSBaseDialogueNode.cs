using System;
using UnityEngine;

namespace NarrativeSystem.Dialogue.Runtime
{
    using Variables;

    /// <summary>
    /// The base class for the runtime data of all dialogues nodes.
    /// </summary>
    [Serializable]
    public abstract class DSBaseDialogueNode : DSBaseNode
    {
        [SerializeReference] public StringRef Dialogue;
        [SerializeReference] public StringRef CharacterName;



    }
}
