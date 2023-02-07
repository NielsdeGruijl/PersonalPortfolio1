using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private NodeGridGenerator grid;

    private List<GameObject> nodes = new List<GameObject>();

    List<Node> path = new List<Node>();

    private void Start()
    {
        grid = GetComponent<NodeGridGenerator>();
        nodes = grid.walkableNodes;
    }

    public void FindPath(Node startNode, Node targetNode)
    {
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        ResetPathFinder();

        open.Add(startNode);

        while (open.Count > 0)
        {
            Node currentNode = FindLowestFCost(open);
            //Debug.Log($"Chosen node: {currentNode.name} gCost: {currentNode.GCost} hCost: {currentNode.HCost} fCost: {currentNode.FCost}");
            open.Remove(currentNode);
            closed.Add(currentNode);

            if (currentNode == targetNode)
            {
                Debug.Log("Path found!");

/*                foreach (Node node in open)
                {
                    node.EnablePathHighlight(true);
                }

                foreach (Node node in closed)
                {
                    node.EnablePathHighlight(true);
                    node.spriteRenderer.color = Color.red;
                }*/

                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in currentNode.neighbours)
            {
                if (closed.Contains(neighbour)) continue;

                int newPathCostToNeighBour = currentNode.GCost + neighbour.CalculateDistance(currentNode);
                if (newPathCostToNeighBour < neighbour.GCost || !open.Contains(neighbour))
                {
                    //neighbour.CalculateFCost(startNode, targetNode);
                    neighbour.GCost = newPathCostToNeighBour;
                    neighbour.HCost = neighbour.CalculateDistance(targetNode);
                    neighbour.parent = currentNode;

                    if (!open.Contains(neighbour))
                    {
                        open.Add(neighbour);
                    }
                }
            }
        }
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        if (path.Count > 0)
        {
            foreach (Node node in path)
            {
                node.EnablePathHighlight(false);
                node.spriteRenderer.color = Color.cyan;
            }
        }

        path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        foreach (Node node in path)
        {
            node.EnablePathHighlight(true);
            node.spriteRenderer.color = Color.magenta;
        }
    }

    private void ResetPathFinder()
    {
        foreach (GameObject node in nodes)
        {
            Node nodeScript = node.GetComponent<Node>();
            nodeScript.GCost = 0;
            nodeScript.HCost = 0;
            nodeScript.FCost = 0;
        }
    }

    Node FindLowestFCost(List<Node> open)
    {
        Node cheapestNode = open[0];
        
        foreach(Node node in open) 
        {
            // || node.FCost == cheapestNode.FCost && node.HCost < cheapestNode.HCost
            if (node.FCost < cheapestNode.FCost)
            {
                cheapestNode = node;
            }
/*            else if(node.FCost == cheapestNode.FCost && node.HCost < cheapestNode.HCost)
            {
                cheapestNode = node;
            }*/
        }

        return cheapestNode; 
    }
}
