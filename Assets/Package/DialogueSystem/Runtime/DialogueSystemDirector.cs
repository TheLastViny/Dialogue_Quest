using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem.Runtime
{
    using Variables;

    /// <summary>
    /// The main class that control the dialogue system.
    /// </summary>
    public class DialogueSystemDirector : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private GameObject _DialoguePanel;
        [SerializeField] private TextMeshProUGUI _CharcterNameText;
        [SerializeField] private TextMeshProUGUI _DialogueText;
        [SerializeField] private Button _ChoiceButtonPrefab;
        [SerializeField] private GameObject _ChoicePannel;

        [Header("Settings")]
        [SerializeField] private float _GlobalFadeDuration = 0.5f;
        [SerializeField] private float _GlobalTextDelayPerCharacter = 0.03f;

        [Header("Input")]
        [SerializeField] private MonoBehaviour _InputComponent;
        public IDialogueSystemInputProvider _InputProvider => _InputComponent as IDialogueSystemInputProvider;

        [Header("Game Reference")]
        [SerializeField] private VariableDatabase _VariableDatabase;

        private Transform _ChoicePanelTransform;
        private DialogueSystemRuntimeGraph _RuntimeGraph;
        private DSBaseNode _CurrentNode;
        private ExecuteStartNode _ExecuteStartNode;
        private ExecuteEndNode _ExecuteEndNode;
        private ExecuteDialogueNode _ExecuteDialogueNode;
        private ExecuteWaitNode _ExecuteWaitNode;
        private VariableRefDictionary _VariableRefDictionary;

        public IDialogueSystemInputProvider InputProvider { get => _InputProvider; }
        public GameObject DialoguePanel { get => _DialoguePanel; }
        public TextMeshProUGUI DialogueText { get => _DialogueText; }
        public GameObject ChoicePannel { get => _ChoicePannel; }
        public TextMeshProUGUI CharcterNameText { get => _CharcterNameText; }
        public Button ChoiceButtonPrefab { get => _ChoiceButtonPrefab; }
        public float GlobalTextDelayPerCharacter { get => _GlobalTextDelayPerCharacter; }
        public float GlobalFadeDuration { get => _GlobalFadeDuration; }
        public DialogueSystemRuntimeGraph RuntimeGraph { get => _RuntimeGraph; }
        public Transform ChoicePanelTransform { get => _ChoicePanelTransform; }

        private void Awake()    
        {
            _ExecuteStartNode = new();
            _ExecuteEndNode = new();
            _ExecuteDialogueNode = new();
            _ExecuteWaitNode = new();
            _VariableRefDictionary = _VariableDatabase.Variables;
            _ChoicePanelTransform = ChoicePannel.transform;
        }

        public async Task ExecuteDialogue(DialogueSystemRuntimeGraph runtimeGraph)
        {
            _RuntimeGraph = runtimeGraph;

            _CurrentNode = RuntimeGraph.Nodes[runtimeGraph.StartNodeID];
            bool isEnd = false;

            while (!isEnd)
            {
                switch (_CurrentNode)
                {
                    case DSStartNode startNode:
                        await _ExecuteStartNode.ExecutAsync(startNode, this);
                        _CurrentNode = runtimeGraph.Nodes[startNode.NextNodeID];
                        break;

                    case DSEndNode endNode:
                        await _ExecuteEndNode.ExecutAsync();
                        isEnd = true;
                        break;

                    case DSNoChoiceDialogueNode noChoiceDialogueNode:
                        await _ExecuteDialogueNode.ExecutAsync(noChoiceDialogueNode, this, _VariableRefDictionary);
                        _CurrentNode = runtimeGraph.Nodes[noChoiceDialogueNode.NextNodeID];
                        break;

                    case DSChoiceDialogueNode choiceDialogueNode:
                        await _ExecuteDialogueNode.ExecutAsync(choiceDialogueNode, this, _VariableRefDictionary);
                        _ChoicePannel.SetActive(false);
                        break;

                    case DSWaitForInputNode waitForInputNode:
                        await _ExecuteWaitNode.ExecutAsync(waitForInputNode, this);
                        _CurrentNode = runtimeGraph.Nodes[waitForInputNode.NextNodeID];
                        break;

                    case DSWaitForSecondNode waitForSecondNode:
                        await _ExecuteWaitNode.ExecutAsync(waitForSecondNode, this, _VariableRefDictionary);
                        _CurrentNode = runtimeGraph.Nodes[waitForSecondNode.NextNodeID];
                        break;

                    default:
                        Debug.LogError($"No executor found for node type: {_CurrentNode.GetType()}");
                        break;
                }
            }
        }

        public void SetNextNode(DSBaseNode node)
        {
            _CurrentNode = node;
        }
    }
}
