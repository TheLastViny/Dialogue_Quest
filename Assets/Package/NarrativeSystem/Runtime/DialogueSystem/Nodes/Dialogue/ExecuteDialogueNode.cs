using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NarrativeSystem.Dialogue.Runtime
{
    using Variables;

    /// <summary>
    /// Executor for the <see cref="DSChoiceDialogueNode"/> and <see cref="DSNoChoiceDialogueNode"/>.
    /// </summary>
    public class ExecuteDialogueNode :
        IDialogueSystemNodeExecutor<DSNoChoiceDialogueNode>,
        IDialogueSystemNodeExecutor<DSChoiceDialogueNode>
    {
        public async Task ExecutAsync(DSNoChoiceDialogueNode node, DialogueSystemDirector ctx, VariableRefDictionary blackboardDict)
        {
            if (blackboardDict is null)
            {
#if UNITY_EDITOR

                throw new System.ArgumentNullException(nameof(blackboardDict), "The blackboard dictionary is null, it wont be able to acess the data.");
#else 
                Debug.LogError("The blackboard dictionary is null, it wont be able to acess the data.");
                return;
#endif
            }

            string dialogue = node.Dialogue.GetValue(blackboardDict);
            string characterName = node.CharacterName.GetValue(blackboardDict);

            if (string.IsNullOrEmpty(dialogue))
            {
                ctx.DialoguePanel.SetActive(false);
                return;
            }

            ctx.DialoguePanel.SetActive(true);
            
            ctx.CharcterNameText.text = characterName;

            await TypeTextWithSkipAsync(dialogue, ctx);
        }

        public async Task ExecutAsync(DSChoiceDialogueNode node, DialogueSystemDirector ctx, VariableRefDictionary blackboardDict)
        {
            if (blackboardDict is null)
            {
#if UNITY_EDITOR

                throw new System.ArgumentNullException(nameof(blackboardDict), "The blackboard dictionary is null, it wont be able to acess the data.");
#else 
                Debug.LogError("The blackboard dictionary is null, it wont be able to acess the data.");
                return;
#endif
            }

            string dialogue = node.Dialogue.GetValue(blackboardDict);
            string characterName = node.CharacterName.GetValue(blackboardDict);
            
            if (string.IsNullOrEmpty(dialogue))
            {
                ctx.DialoguePanel.SetActive(false);
                return;
            }

            ctx.DialoguePanel.SetActive(true);
            ctx.CharcterNameText.text = characterName;

            await TypeTextWithSkipAsync(dialogue, ctx);
            
            await ClearDialoguePanel(ctx);

            ctx.ChoicePannel.SetActive(true);

            await ShowChoices(node, ctx, blackboardDict);
        }

        static async Task TypeTextWithSkipAsync(string dialogueText, DialogueSystemDirector ctx)
        {
            TextMeshProUGUI label = ctx.DialogueText;
            float delayPerChar = ctx.GlobalTextDelayPerCharacter;
            IDialogueSystemInputProvider inputProvider = ctx.InputProvider;

            label.text = "";
            StringBuilder builder = new StringBuilder();

            bool insideIRichTag = false;

            Task skipInputDetected = inputProvider.InputDetected();

            foreach(char c in dialogueText)
            {
                if(c == '<')
                {
                    insideIRichTag = true;
                }

                builder.Append(c);
                
                if(c == '>')
                {
                    insideIRichTag = false;
                }

                if(insideIRichTag || char.IsWhiteSpace(c))
                {
                    continue;
                }

                label.text = builder.ToString();

                float timer = 0f;
                while(timer < delayPerChar)
                {
                    if (skipInputDetected.IsCompleted)
                    {
                        label.text = dialogueText;
                        return;
                    }
                    timer += Time.deltaTime;
                    await Task.Yield();
                }

            }
            
            label.text = dialogueText;
        }

        static async Task ClearDialoguePanel(DialogueSystemDirector ctx)
        {
            foreach (Transform children in ctx.ChoicePanelTransform)
            {
                Object.Destroy(children.gameObject);
            }

            await Task.Yield();
        }

        static Task ShowChoices(DSChoiceDialogueNode node, DialogueSystemDirector ctx, VariableRefDictionary blackboardDict)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            foreach (TextChoice choice in node.Choices)
            {
                TextChoice capturedChoice = choice;
                Button button = Object.Instantiate(ctx.ChoiceButtonPrefab, ctx.ChoicePanelTransform);
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

                string choiceText = choice.Text.GetValue(blackboardDict);

                buttonText.text = choiceText;

                button.onClick.AddListener(() =>
                {
                    ctx.SetNextNode(ctx.RuntimeGraph.Nodes[capturedChoice.NextNodeID]);

                    tcs.TrySetResult(true);
                });
            }

            return tcs.Task;
        }
    }
}
