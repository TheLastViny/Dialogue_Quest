using System;

namespace DialogueSystem.Variables
{
    /// <summary>
    /// A bool variable that is a reference in the system.
    /// </summary>
    [Serializable]
    public class BoolVariable : VariableRef<bool>
    {
        public BoolVariable(bool value, string name) : base(value, name)
        {
        }
    }
}
