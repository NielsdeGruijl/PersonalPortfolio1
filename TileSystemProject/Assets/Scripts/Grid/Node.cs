using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Node : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public GameObject neighbourHighlight;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    [HideInInspector] public bool highlighted = false;

    [Header("Tile info")]
    public bool isObstacle;
    public bool walkable;
    public bool occupied = false;

    [Header("PathFinder info")]
    [SerializeField] private TextMeshPro GCostText;
    [SerializeField] private TextMeshPro HCostText;
    [SerializeField] private TextMeshPro FCostText;
    [Space(10)]
    public List<Node> neighbours;
    public Node parent;

    [Header("Pathfinder costs")]
    public int GCost;
    public int HCost;
    public int FCost;


    private PlayerScript player;

    private void Start()
    {
        spriteRenderer.color = Color.cyan;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        sprite = GetComponent<SpriteRenderer>();
        neighbourHighlight = transform.GetChild(0).gameObject;
        spriteRenderer = neighbourHighlight.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        sprite.enabled = highlighted;

        if (occupied)
        {
            walkable= false;
        }
        if(!occupied && !walkable && !isObstacle)
        {
            walkable = true;
        }
    }

    public int CalculateDistance(Node targetNode)
    {
        int distX = Mathf.FloorToInt(Mathf.Abs(transform.position.x - targetNode.transform.position.x));
        int distY = Mathf.FloorToInt(Mathf.Abs(transform.position.y - targetNode.transform.position.y));

        /*        if (distX > distY)
                {
                    return 14 * distY + 10 * (distX - distY);
                }
                return 14 * distX + 10 * (distY - distX);*/

        return 10 * (distX + distY);
    }

    public void EnablePathHighlight(bool enable)
    {
        neighbourHighlight.SetActive(enable);

        FCost = GCost + HCost;

        GCostText.text = GCost.ToString();
        HCostText.text = HCost.ToString();
        FCostText.text = FCost.ToString();
    }

    private void OnMouseOver()
    {
        highlighted = true;
    }

    private void OnMouseExit()
    {
        highlighted = false;
    }

    private void OnMouseDown()
    {
        if(player != null && player.gameObject.activeSelf)
        {
            if (player.reachedDestination)
            {
                player.SetPath(this);
            }
        }
    }
}
