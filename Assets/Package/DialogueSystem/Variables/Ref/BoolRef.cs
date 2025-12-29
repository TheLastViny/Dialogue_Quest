using System;

namespace NarrativeSystem.Variables
{
    /// <summary>
    /// Base class for the bool in the narrative system.
    /// </summary>
    [Serializable]
    public abstract class BoolRef
    {
        protected BoolRef() {}

        public abstract bool GetValue(VariableRefDictionary ctx);
    }
}
