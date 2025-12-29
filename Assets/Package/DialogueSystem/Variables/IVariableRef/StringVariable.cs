using System;

namespace DialogueSystem.Variables
{
    /// <summary>
    /// A string variable that is a reference in the system.
    /// </summary>
    [Serializable]
    public class StringVariable : VariableRef<string>
    {
        public StringVariable(string value, string name) : base(value, name)
        {
        }
    }
}
