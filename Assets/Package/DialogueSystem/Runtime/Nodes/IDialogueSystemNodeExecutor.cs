using DialogueSystem.Variables;
using System.Threading.Tasks;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    /// <summary>
    /// The interface for an executor of a dialogue system node.
    /// </summary>
    /// <typeparam name="TNode">The type of node to execute</typeparam>
    public interface IDialogueSystemNodeExecutor<in TNode> where TNode : DSBaseNode
    {
        Task ExecutAsync(TNode node, DialogueSystemDirector ctx, VariableRefDictionary blackboardDictionary = null);
    }
}
