using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    public Node GridPosition;

    private Rigidbody2D rb;
    
    private List<GameObject> nodes = new List<GameObject>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        nodes = GameManagerScript.instance.GetComponent<NodeGridGenerator>().walkableNodes;
        GetGridLocation();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = transform.right * 2;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        GetGridLocation();
    }

    private void GetGridLocation()
    {
        float xPos = Mathf.FloorToInt(transform.position.x) + 0.5f;
        float yPos = Mathf.FloorToInt(transform.position.y) + 0.5f;

        foreach(GameObject node in nodes)
        {
            if(new Vector3(xPos, yPos, 0) == node.transform.position)
            {
                Node nodeScript = node.GetComponent<Node>();
                GridPosition = nodeScript;
                return;
            }
        }
    }
}
