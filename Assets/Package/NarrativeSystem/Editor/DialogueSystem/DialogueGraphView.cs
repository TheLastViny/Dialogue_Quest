using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;


namespace NarrativeSystem.Dialogue.Editor
{
    public class DialogueGraphView : GraphView
    {
        internal DialogueGraph Graph { get; }

        internal DialogueGraphView(DialogueGraph graph)
        {
            Graph = graph;

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }
    }
}

