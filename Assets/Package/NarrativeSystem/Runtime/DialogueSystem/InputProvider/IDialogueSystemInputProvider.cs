using System.Threading.Tasks;


namespace NarrativeSystem.Dialogue.Runtime
{
    /// <summary>
    /// An interface that abstracts the input required by the dialogue system.
    /// </summary>
    public interface IDialogueSystemInputProvider
    {
        /// <summary>
        /// This method creates a <see cref="Task"/> that monitors for input to advance the dialogue system.
        /// The returned Task can be awaited to coordinate the visual novel flow with
        /// user interactions, allowing for proper sequencing with other async operations like
        /// the typewriter effect.
        /// </summary>
        /// <returns><
        /// A Task that completes when the user provides the necessary input (such as clicking,
        /// pressing space/enter, etc.) to progress to the next step in the visual novel.
        /// /returns>
        Task InputDetected();
    }
}
