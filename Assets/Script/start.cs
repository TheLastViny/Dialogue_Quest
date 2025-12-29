using NarrativeSystem.Dialogue.Runtime;
using UnityEngine;
using UnityEngine.Rendering;

public class start : MonoBehaviour
{
    [SerializeField] DialogueSystemRuntimeGraph graph;
    [SerializeField] DialogueSystemDirector director;

    void Start()
    {
        _ = director.ExecuteDialogue(graph);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
