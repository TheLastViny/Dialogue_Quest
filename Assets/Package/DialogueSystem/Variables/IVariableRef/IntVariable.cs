using System;

namespace DialogueSystem.Variables
{
    /// <summary>
    /// A int variable that is a reference in the system.
    /// </summary>
    [Serializable]
    public class IntVariable : VariableRef<int>
    {
        public IntVariable(int value, string name) : base(value, name)
        {
        }
    }
}
