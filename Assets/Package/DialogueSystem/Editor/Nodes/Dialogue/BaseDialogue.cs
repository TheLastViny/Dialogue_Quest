using NarrativeSystem.Variables;
using System;

namespace NarrativeSystem.Dialogue.Editor
{
    /// <summary>
    /// Represents the base model for all dialogue nodes.
    /// </summary>
    [Serializable]
    internal abstract class BaseDialogue : Base
    {
        public const string IN_PORT_CHARACTER_NAME = "CharacterName";
        public const string IN_PORT_DIALOGUE_NAME = "Dialogue";

        /// <summary>
        /// Add character and dialogue nodes
        /// </summary>
        /// <param name="context">The context used to define ports for this node</param>
        protected void AddCharacterDialoguePort(IPortDefinitionContext context)
        {
            AddInputCharacterPort(context);

            AddInputDialoguePort(context);
        }

        /// <summary>
        /// Add the character name input port
        /// </summary>
        /// <param name="context">The context used to define ports for this node</param>
        protected void AddInputCharacterPort(IPortDefinitionContext context)
        {
            context.AddInputPort<string>(IN_PORT_CHARACTER_NAME)
                .WithDisplayName("Character Name")
                .Build();
        }

        /// <summary>
        /// Add the dialogue input port
        /// </summary>
        /// <param name="context">The context used to define ports for this node</param>
        protected void AddInputDialoguePort(IPortDefinitionContext context)
        {
            context.AddInputPort<string>(IN_PORT_DIALOGUE_NAME)
                .WithDisplayName("Dialogue")
                .Build();
        }
    }
}
