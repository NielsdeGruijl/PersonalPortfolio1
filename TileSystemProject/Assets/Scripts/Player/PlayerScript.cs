using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    public Node positionNode;

    public List<Node> path = new List<Node>();
    public bool reachedDestination = true;

    private List<GameObject> nodes = new List<GameObject>();
    private Rigidbody2D rb;
    private LineRenderer lineRenderer;
    private Node target;

    private Vector2 moveDir;
    private float originalMoveSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer= GetComponent<LineRenderer>();

        rb.gravityScale = 0;

        nodes = GameManagerScript.instance.GetComponent<NodeGridGenerator>().walkableNodes;
        GetGridLocation();

        originalMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        if (!reachedDestination && path.Count > 0)
        {
            FollowPath();
        }

        if(rb.velocity.magnitude > 0)
        {
            GetGridLocation();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * moveSpeed;
    }

    private void GetGridLocation()
    {
        float xPos = Mathf.FloorToInt(transform.position.x) + 0.5f;
        float yPos = Mathf.FloorToInt(transform.position.y) + 0.5f;

        foreach(GameObject node in nodes)
        {
            if(new Vector3(xPos, yPos, 0) == node.transform.position)
            {
                if(positionNode!= null)
                    positionNode.occupied = false;

                Node nodeScript = node.GetComponent<Node>();
                positionNode = nodeScript;
                positionNode.occupied = true;
                return;
            }
        }
    }

    public void SetPath(Node target)
    {
        path = GameManagerScript.instance.GetComponent<Pathfinding>().FindPath(positionNode, target);
        reachedDestination = false;
        moveSpeed = originalMoveSpeed;
    }

    private void FollowPath()
    {
        if (target == null)
        {
            target = path[0];
            moveDir = (target.transform.position - transform.position).normalized;
            UpdateLine();
        }

        float distance = (target.transform.position - transform.position).magnitude;

        if(distance <= 0.01f)
        {
            path.Remove(target);
            target = null;
        }

        if (path.Count <= 0)
        {
            Debug.Log("destination reached");
            moveDir = Vector2.zero;
            moveSpeed = 0;
            reachedDestination = true;
        }
    }

    private void UpdateLine()
    {
        Vector3[] nodePositions = new Vector3[path.Count];

        for (int i = 0; i < path.Count; i++)
        {
            nodePositions[i] = path[i].transform.position + new Vector3(0, 0, -1);
        }

        lineRenderer.positionCount = path.Count;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPositions(nodePositions);
    }
}
