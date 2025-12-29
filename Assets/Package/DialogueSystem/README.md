# 

## Dialogue System

### Type of nodes
**StartNode** : This node should only exist in one exemplary in every dialogue graph. The graph 
won't save until there is one node of this type. Also it won't save either if there is more than one
of this type.

**EndNode** : This is a node that signals the end of a dialogue.

**NoChoiceDialogueNode** : This is a node with dialogue that doesn't need a choice after the text 
display.

**ChoiceDialogueNode** : This is a node with dialogue that need at least a   choice after the text 
display. Press enter or outside the node to update the number of choice.

### Runtime

#### Input 

In the dialogue system their is an input provider for advance the text. By default it is space and a left click button in the mouse.
If needed you can add more.

## Reference
Depot used for the dictionnary : [Seriazable Dictionary](https://github.com/JDSherbert/Unity-Serializable-Dictionary)