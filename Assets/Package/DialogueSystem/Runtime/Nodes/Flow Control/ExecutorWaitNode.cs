using System.Threading.Tasks;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    using Variables;

    /// <summary>
    /// The executor for the <see cref="DSWaitForInputNode"/> and <see cref="DSWaitForSecondNode"/>.
    /// </summary>
    public class ExecuteWaitNode : IDialogueSystemNodeExecutor<DSWaitForInputNode>,
        IDialogueSystemNodeExecutor<DSWaitForSecondNode>
    {
        public async Task ExecutAsync(DSWaitForInputNode node, DialogueSystemDirector ctx, VariableRefDictionary blackboardDict = null)
        {
            await ctx.InputProvider.InputDetected();
        }

        public async Task ExecutAsync(DSWaitForSecondNode node, DialogueSystemDirector ctx, VariableRefDictionary blackboardDict)
        {

            float timer = 0f;
            float timeWait = node.Seconds.GetValue(blackboardDict);

            while (timer <= timeWait)
            {
                timer += Time.deltaTime;
                await Task.Yield();
            }
        }
    }
}
