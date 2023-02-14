using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minMoveCooldown;
    [SerializeField] private float maxMoveCooldown;

    public Node positionNode;

    private List<GameObject> nodes = new List<GameObject>();
    private List<Node> path = new List<Node>();

    private bool reachedDestination = true;

    public Node target;
    private Vector2 moveDir;

    private float originalMovespeed;
    private Rigidbody2D rb;

    private LineRenderer lineRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer= GetComponent<LineRenderer>();

        rb.gravityScale = 0;

        originalMovespeed = moveSpeed;

        nodes = GameManagerScript.instance.GetComponent<NodeGridGenerator>().walkableNodes;
        GetGridLocation();
    }

    private void Update()
    {
        GetGridLocation();

        WanderBehaviour();

        if (!reachedDestination && path.Count > 0)
        {
            FollowPath();
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

        foreach (GameObject node in nodes)
        {
            if (new Vector3(xPos, yPos, 0) == node.transform.position)
            {
                Node nodeScript = node.GetComponent<Node>();
                positionNode = nodeScript;
                return;
            }
        }
    }

    private void WanderBehaviour()
    {
        if (reachedDestination)
        {
            moveSpeed = originalMovespeed;
            Node destination = nodes[Random.Range(0, nodes.Count)].GetComponent<Node>();
            path = GameManagerScript.instance.GetComponent<Pathfinding>().FindPath(positionNode, destination);
            reachedDestination = false;
        }
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

        if (distance <= 0.01f)
        {
            path.Remove(target);
            target = null;
        }

        if (path.Count <= 0)
        {
            Debug.Log("destination reached");
            moveDir = Vector2.zero;
            moveSpeed = 0;
            StartCoroutine(StartMoveCooldown());
        }
    }

    IEnumerator StartMoveCooldown()
    {
        yield return new WaitForSeconds(Random.Range(minMoveCooldown, maxMoveCooldown));
        reachedDestination = true;
    }

    private void UpdateLine()
    {
        Vector3[] nodePositions = new Vector3[path.Count];
        
        for(int i = 0; i < path.Count; i++) 
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
