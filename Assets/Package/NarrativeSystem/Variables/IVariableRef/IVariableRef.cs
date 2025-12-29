using System;

namespace NarrativeSystem.Variables
{
    /// <summary>
    /// Interface for all the <see cref="VariableRef{T}"/>.
    /// </summary>
    public interface IVariableRef
    {
        Type ValueType { get; }
        void SetValue(object value);
        void ResetValue();

    }
}
