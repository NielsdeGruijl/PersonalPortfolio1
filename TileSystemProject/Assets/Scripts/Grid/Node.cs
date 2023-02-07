using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Node : MonoBehaviour
{
    [SerializeField] TextMeshPro GCostText;
    [SerializeField] TextMeshPro HCostText;
    [SerializeField] TextMeshPro FCostText;

    public SpriteRenderer sprite;
    public GameObject neighbourHighlight;

    public SpriteRenderer spriteRenderer;

    public List<Node> neighbours;
    public Node parent;

    public bool walkable;
    public bool highlighted = false;

    [Header("Pathfinder costs")]
    public int GCost;
    public int HCost;
    public int FCost;

    private void Start()
    {
        gameObject.SetActive(true);

        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        spriteRenderer.color = Color.cyan;
    }

    private void Update()
    {
        sprite.enabled = highlighted;
    }

    public int CalculateDistance(Node targetNode)
    {
        int distX = Mathf.FloorToInt(Mathf.Abs(transform.position.x - targetNode.transform.position.x));
        int distY = Mathf.FloorToInt(Mathf.Abs(transform.position.y - targetNode.transform.position.y));

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
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
}
