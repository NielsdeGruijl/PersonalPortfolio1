using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Node GridPosition;

    private List<GameObject> nodes = new List<GameObject>();

    private void Start()
    {
        nodes = GameManagerScript.instance.GetComponent<NodeGridGenerator>().walkableNodes;
        GetGridLocation();
    }

    private void Update()
    {
        GetGridLocation();
    }

    private void GetGridLocation()
    {
        float xPos = Mathf.FloorToInt(transform.position.x) + 0.5f;
        float yPos = Mathf.FloorToInt(transform.position.y) + 0.5f;

        foreach (GameObject node in nodes)
        {
            if (new Vector3(xPos, yPos, 0) == node.transform.position)
            {
                Node nodeScript = node.GetComponent<Node>();
                GridPosition = nodeScript;
                return;
            }
        }
    }
}
