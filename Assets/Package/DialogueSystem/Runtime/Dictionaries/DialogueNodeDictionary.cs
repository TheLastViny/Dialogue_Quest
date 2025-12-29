using System;

namespace DialogueSystem.Runtime
{
    using Dictionnaries;

    /// <summary>
    /// Dictionary used at run time that contain all the nodes in a dialogue.
    /// </summary>
    [Serializable]
    public class DialogueNodeDictionary : SerializableDictionary<string, DSBaseNode>
    {
    
    }
}
