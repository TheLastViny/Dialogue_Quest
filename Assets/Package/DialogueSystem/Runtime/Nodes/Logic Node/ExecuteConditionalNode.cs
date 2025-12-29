using System.Threading.Tasks;
using DialogueSystem.Utility;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    using Variables;

    /// <summary>
    /// Executor for <see cref="DSNumberConditionalNode"/>
    /// and <see cref="DSStringConditionalNode"/>.
    /// </summary>
    public class ExecuteConditionalNode :
        IDialogueSystemNodeExecutor<DSNumberConditionalNode>,
        IDialogueSystemNodeExecutor<DSStringConditionalNode>
    {
        public Task ExecutAsync(DSStringConditionalNode node, DialogueSystemDirector ctx, VariableRefDictionary blackboardDict)
        {
            bool condition = ExecuteConditionString(node);
            if (condition)
            {
                ctx.SetNextNode(ctx.RuntimeGraph.Nodes[node.TrueNextNodeID]);
            }
            else
            {
                ctx.SetNextNode(ctx.RuntimeGraph.Nodes[node.FalseNextNodeID]);
            }

            return Task.CompletedTask;
        }

        public Task ExecutAsync(DSNumberConditionalNode node, DialogueSystemDirector ctx, VariableRefDictionary blackboardDict = null)
        {
            bool condition = ExecuteConditionNumber(node);
            if (condition)
            {
                ctx.SetNextNode(ctx.RuntimeGraph.Nodes[node.TrueNextNodeID]);
            }
            else
            {
                ctx.SetNextNode(ctx.RuntimeGraph.Nodes[node.FalseNextNodeID]);
            }

            return Task.CompletedTask;
        }

        private bool ExecuteConditionNumber(DSNumberConditionalNode node)
        {
            switch (node.Condition)
            {
                case EConditionalNumbers.Equal:
                    return node.FirstCondition == node.SecondCondition;

                case EConditionalNumbers.NotEqual:
                    return node.FirstCondition != node.SecondCondition;

                case EConditionalNumbers.LessThanOrEqual:
                    return node.FirstCondition <= node.SecondCondition;

                case EConditionalNumbers.LessThan:
                    return node.FirstCondition < node.SecondCondition;

                case EConditionalNumbers.GreaterThanOrEqual:
                    return node.FirstCondition >= node.SecondCondition;

                case EConditionalNumbers.GreaterThan:
                    return node.FirstCondition > node.SecondCondition;

                default:
                    Debug.Log($"The {node.Condition} is not supported");
                    return false;
            }
        }

        private bool ExecuteConditionString(DSStringConditionalNode node)
        {
            switch (node.Condition)
            {
                case EConditionalString.Equal:
                    return node.FirstCondition == node.SecondCondition;

                case EConditionalString.NotEqual:
                    return node.FirstCondition != node.SecondCondition;

                case EConditionalString.Contains:
                    return node.FirstCondition.Contains(node.SecondCondition);

                case EConditionalString.StartsWith:
                    return node.FirstCondition.StartsWith(node.SecondCondition);

                case EConditionalString.EndsWith:
                    return node.FirstCondition.EndsWith(node.SecondCondition);

                case EConditionalString.NotContains:
                    return !node.FirstCondition.Contains(node.SecondCondition);

                case EConditionalString.NotStartsWith:
                    return !node.FirstCondition.StartsWith(node.SecondCondition);

                case EConditionalString.NotEndsWith:
                    return !node.FirstCondition.EndsWith(node.SecondCondition);

                default:
                    Debug.Log($"The {node.Condition} is not supported");
                    return false;
            }
        }
    }
}
