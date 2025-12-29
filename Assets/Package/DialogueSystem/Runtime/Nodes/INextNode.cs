namespace DialogueSystem.Runtime
{
    /// <summary>
    /// Interface for the node / variables that need a next node.
    /// </summary>
    public interface INextNode
    {
        public string NextNodeID { get; set; }

    }
}
