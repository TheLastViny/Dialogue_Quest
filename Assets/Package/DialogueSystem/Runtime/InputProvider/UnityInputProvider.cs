using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DialogueSystem.Runtime
{
    public class UnityInputProvider : MonoBehaviour, IDialogueSystemInputProvider
    {
        /// <summary>
        ///  Defines the input actions for the dialgoue system.
        /// </summary>
        private DialogueSystemInput _inputActions;

        /// <summary>
        /// Used to signal when the next input event is dected.
        /// </summary>
        private TaskCompletionSource<bool> _nextTcs;


        /// <summary>
        /// On Awake we set up our input actions and subscribe to input events.
        /// </summary>
        private void Awake()
        {
            _inputActions = new DialogueSystemInput();
            if(_inputActions != null)
            {
                _inputActions.Gameplay.Next.performed += OnNextPressed;
            }
        }

        private void OnDestroy()
        {
            _inputActions.Gameplay.Next.performed -= OnNextPressed;
        }

        /// When the 'Next' input action is performed, we set the <see cref="_nextTcs"/> task as completed.
        /// This <see cref="Task"/> can be awaited on by <see cref="IDialogueNodeExecutor{TNode}"/>s.
        /// </summary>
        private void OnNextPressed(InputAction.CallbackContext _) => _nextTcs?.TrySetResult(true);

        /// <summary>
        /// Enables the input actions when the object is enabled.
        /// </summary>
        private void OnEnable() => _inputActions.Enable();

        /// <summary>
        /// Disables the input actions when the object is disabled.
        /// </summary>
        private void OnDisable() => _inputActions.Disable();

        /// <summary>
        /// Creates a <see cref="TaskCompletionSource{TResult}"/> to wait for the next input event.
        /// <br/><br/>
        /// If there is already a <see cref="TaskCompletionSource{TResult}"/> created and it's not already completed,
        /// we just return it. This allows nodes to wait for the next input event without mistakenly waiting for input
        /// more than once.
        /// </summary>
        public Task InputDetected()
        {
            if(_nextTcs == null || _nextTcs.Task.IsCompleted)
            {
                _nextTcs = new TaskCompletionSource<bool>();
            }

            return _nextTcs.Task;
        }
    }
}
