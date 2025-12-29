using System;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Represente the base model for all nodes in the dialogue system.
    /// </summary>
    [Serializable]
    internal abstract class Base : Node
    {
        public const string IN_PORT_DEFAULT_NAME = "InExecutionPort";
        public const string OUT_PORT_DEFAULT_NAME = "OutExecutionPort";
        private string _ID;

        public Base()
        {
    
            if(string.IsNullOrEmpty(_ID))
            {
                _ID = Guid.NewGuid().ToString();
            }
        }

        public string GetID()
        {
            return _ID;
        }

        /// <summary>
        /// Add the input and ouput port for a node 
        /// </summary>
        /// <param name="context">The context used to define ports for this node</param>
        protected void AddInputOutputExecutionsPorts(IPortDefinitionContext context)
        {
            AddInputExecutionPort(context);

            AddOutputExecutionPort(context);
        }

        /// <summary>
        /// Add the input port for a node 
        /// </summary>
        /// <param name="context">The context used to define ports for this node</param>
        protected void AddInputExecutionPort(IPortDefinitionContext context) 
        { 
            context.AddInputPort(IN_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        /// <summary>
        /// Add the output port for a node
        /// </summary>
        /// <param name="context">The context used to define ports for this node</param>
        protected void AddOutputExecutionPort(IPortDefinitionContext context)
        {
            context.AddOutputPort(OUT_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}
