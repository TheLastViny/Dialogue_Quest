using System;

namespace DialogueSystem.Variables
{
    /// <summary>
    /// A float variable that is a reference in the system.
    /// </summary>
    [Serializable]
    public class FloatVariable : VariableRef<float>
    {
        public FloatVariable(float value, string name) : base(value, name)
        {
        }
    }
}
