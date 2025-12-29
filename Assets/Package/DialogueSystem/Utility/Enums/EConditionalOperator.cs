using UnityEngine;

namespace DialogueSystem.Utility
{
    /// <summary>
    /// Enums for the conditional nodes.
    /// </summary>
    public enum EConditionalNumbers
    {
        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual
    }

    public enum EConditionalString
    {
        Equal,
        NotEqual,
        Contains,
        StartsWith,
        EndsWith,
        NotContains,
        NotStartsWith,
        NotEndsWith
    }
}
