using System.Threading.Tasks;

namespace DialogueSystem.Runtime
{
    using Variables;

    /// <summary>
    /// Executor for the <see cref="DSStartNode"/>.
    /// </summary>
    public class ExecuteStartNode : IDialogueSystemNodeExecutor<DSStartNode>
    {
        public Task ExecutAsync(DSStartNode node, DialogueSystemDirector ctx, VariableRefDictionary blackboardDict = null)
        {
            return Task.CompletedTask;
        }
    }
}
