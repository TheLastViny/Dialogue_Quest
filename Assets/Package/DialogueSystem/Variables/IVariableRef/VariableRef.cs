using System;
using UnityEngine;

namespace DialogueSystem.Variables
{
    /// <summary>
    /// Base class for all the reference value.
    /// </summary>
    /// <typeparam name="T">The type of the value (int, string float, ect...).</typeparam>
    [Serializable]
    public abstract class VariableRef<T> : IVariableRef
    {
        [SerializeField] private string _name;
        [SerializeField] protected T _initialValue;
        [SerializeField] protected T _currentValue;

        public Type ValueType => typeof(T);

        protected VariableRef(T value, string name)
        {
            _initialValue = value;
            _currentValue = value;
            _name = name;
        }

        public T GetValue() => _currentValue;

        public void SetValue(object value)
        {
        #if UNITY_EDITOR
            if (value is not T cast)
            {
                throw new InvalidCastException($"Cannot assing value of type {value.GetType()} to variable {_name} of {typeof(T)}");
            }
            _currentValue = cast;
        #else
            if (value is T cast)
            {
                _currentValue = cast;
            }
            else
            {
                Debug.LogWarning($"Cannot assing value of type {value.GetType()} to variable {_name} of {typeof(T)}");
            }
        #endif
        }

        public void ResetValue()
        {
            _currentValue = _initialValue;
        }
    }
}
