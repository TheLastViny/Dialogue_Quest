using System;
using UnityEngine; 
namespace DialogueSystem.Variables
{
    /// <summary>
    /// Base class for the string in the narrative system.
    /// </summary>
    [Serializable]
    public abstract class StringRef
    {
        protected StringRef() { }

        public abstract string GetValue(VariableRefDictionary ctx);
    }

   
}
