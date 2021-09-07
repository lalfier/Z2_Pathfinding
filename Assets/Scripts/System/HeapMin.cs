
public class HeapMin
{
    private GridNode[] nodes;   // Array of all nodes inside grid
    private int nodeCount;  // Number of open grid nodes inside array

    public int Count { get { return nodeCount; } }  // Get number of open grid nodes inside array

    /// <summary>
    /// Constructor for heap min object.
    /// </summary>
    /// <param name="maxSize">Number of all nodes inside grid.</param>
    public HeapMin(int maxSize)
    {
        nodes = new GridNode[maxSize];
    }

    /// <summary>
    /// Adds open grid node to the array and sort it up.
    /// </summary>
    /// <param name="node">Grid node to add.</param>
    public void Add(GridNode node)
    {
        // Add new node as last and sort it up
        node.HeapIndex = nodeCount;
        nodes[nodeCount] = node;
        SortUp(node);
        nodeCount++;
    }

    /// <summary>
    /// Removes first node from the array and sort it down.
    /// </summary>
    /// <returns>Returns removed grid node.</returns>
    public GridNode RemoveFirst()
    {
        // Get first node
        GridNode firstNode = nodes[0];        
        nodeCount--;
        // Set new first node as last and sort it down
        nodes[0] = nodes[nodeCount];
        nodes[0].HeapIndex = 0;
        SortDown(nodes[0]);
        nodes[nodeCount] = null;
        return firstNode;
    }

    /// <summary>
    /// Check if array contains grid node.
    /// </summary>
    /// <param name="node">Grid node to check.</param>
    /// <returns>Returns true if array contains grid node.</returns>
    public bool Contains(GridNode node)
    {
        return Equals(nodes[node.HeapIndex], node);
    }

    /// <summary>
    /// Update node priority position.
    /// </summary>
    /// <param name="node">Grid node to update.</param>
    public void UpdateNode(GridNode node)
    {
        SortUp(node);
    }

    /// <summary>
    /// Clear array of all open grid nodes.
    /// </summary>
    public void Clear()
    {
        if(nodeCount > 0)
        {
            for (int i = 0; i < nodeCount; i++)
            {
                nodes[i] = null;
            }
            nodeCount = 0;
        }
    }

    private void SortUp(GridNode node)
    {
        // Get parent node index
        int parentIndex = (node.HeapIndex - 1) / 2;
        while (true)
        {
            // Check if node score is lower and swap it (up) with parent node
            GridNode parentNode = nodes[parentIndex];
            if(node.CompareTo(parentNode) > 0)
            {
                Swap(node, parentNode);
            }
            else
            {
                break;
            }

            // Get new parent node index
            parentIndex = (node.HeapIndex - 1) / 2;
        }
    }

    private void SortDown(GridNode node)
    {
        while (true)
        {
            // Get node child index on left and right
            int childIndexLeft = node.HeapIndex * 2 + 1;
            int childIndexRight = node.HeapIndex * 2 + 2;
            int swapIndex;

            // Check if node has children
            if(childIndexLeft < nodeCount)
            {
                swapIndex = childIndexLeft;

                if(childIndexRight < nodeCount)
                {
                    if(nodes[childIndexLeft].CompareTo(nodes[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                // Check if node score is bigger and swap it (down) with child node
                if (node.CompareTo(nodes[swapIndex]) < 0)
                {
                    Swap(node, nodes[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    private void Swap(GridNode childNode, GridNode parentNode)
    {
        // Swap nodes in array
        nodes[childNode.HeapIndex] = parentNode;
        nodes[parentNode.HeapIndex] = childNode;
        // Swap heap index of nodes
        int tempIndex = childNode.HeapIndex;
        childNode.HeapIndex = parentNode.HeapIndex;
        parentNode.HeapIndex = tempIndex;
    }
}
