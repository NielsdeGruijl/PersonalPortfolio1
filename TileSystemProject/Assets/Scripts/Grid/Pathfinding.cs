using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private NodeGridGenerator grid;

    private List<GameObject> nodes = new List<GameObject>();

    private List<Node> path = new List<Node>();

    private void Start()
    {
        grid = GetComponent<NodeGridGenerator>();
        nodes = grid.walkableNodes;
    }

    public List<Node> FindPath(Node startNode, Node targetNode)
    {
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        ResetPathFinder();

        open.Add(startNode);

        while (open.Count > 0)
        {
            Node currentNode = FindLowestFCost(open);
            open.Remove(currentNode);
            closed.Add(currentNode);

            if (currentNode == targetNode)
            {
                foreach (Node node in open)
                {
                    node.EnablePathHighlight(true);
                    node.spriteRenderer.color = Color.cyan;
                }

                foreach (Node node in closed)
                {
                    node.EnablePathHighlight(true);
                    node.spriteRenderer.color = Color.red;
                }

                Debug.Log("Path found!");    
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in currentNode.neighbours)
            {
                if (closed.Contains(neighbour) || !neighbour.walkable) continue;

                int newPathCostToNeighBour = currentNode.GCost + neighbour.CalculateDistance(currentNode);
                if (newPathCostToNeighBour < neighbour.GCost || !open.Contains(neighbour))
                {
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
        return null;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
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
            if (node.FCost < cheapestNode.FCost || node.FCost == cheapestNode.FCost && node.HCost < cheapestNode.HCost)
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
