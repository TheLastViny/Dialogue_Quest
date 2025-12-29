using System.Threading.Tasks;

namespace NarrativeSystem.Dialogue.Runtime
{
    using Variables;

    /// <summary>
    /// Executor for the <see cref="DSEndNode"/>.
    /// </summary>
    public class ExecuteEndNode : IDialogueSystemNodeExecutor<DSEndNode>
    {
        public Task ExecutAsync(DSEndNode node = null, DialogueSystemDirector ctx = null, VariableRefDictionary blackboardDict = null)
        {
            return Task.CompletedTask;
        }
    }
}
