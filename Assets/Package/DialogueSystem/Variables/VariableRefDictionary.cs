using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Variables
{
    using Dictionnaries;

    /// <summary>
    /// Dictionnary that contain the data from all the blackboard
    /// </summary>
    [Serializable]
    public class VariableRefDictionary : SerializableDictionary<string, IVariableRef>
    {
        public bool GetValue<T>(string key, out T value)
        {
#if UNITY_EDITOR
            if (!TryGetValue(key, out var data))
            {
                throw new KeyNotFoundException($"{key} not found");
            }

            if (data is not VariableRef<T> typed)
            {
                throw new InvalidCastException($"{key} is not of type {typeof(T)}");
            }

            value =  typed.GetValue();
            return true;
            
#else
            if (!TryGetValue(key, out var data))
            {
                Debug.Log($"{key} not found");
                value = default(T);
                return false;
            }

            if (data is not VariableRef<T> typed)
            {
                Debug.Log($"{key} is not of type {typeof(T)}");
                value = default(T);
                return false;
            }

            value = typed.GetValue();
            return true;
#endif
        }

        public void ResetValue(string key)
        {
#if UNITY_EDITOR
            if (!TryGetValue(key, out var data))
            {
                throw new KeyNotFoundException($"{key} not found");
            }       
            
            data.ResetValue();
#else
            if (TryGetValue(key, out var data))
            {
                
                data.ResetValue();
            }
            else
            {
                Debug.Log($"{key} not found");
            }
#endif
        }

        public void ChangeValue<T>(string key, T newValue)
        {
#if UNITY_EDITOR
            if (!TryGetValue(key, out var data))
            {
                throw new KeyNotFoundException($"{key} not found");
            }

            data.SetValue(newValue);  
#else
            if (!TryGetValue(key, out var data))
            {
                Debug.Log($"{key} not found");
                return;
            }

            data.SetValue(newValue);
#endif
        }
    }
}
