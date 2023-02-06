using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public SpriteRenderer sprite;

    public bool walkable;

    public bool highlighted = false;

    Ray ray;
    RaycastHit hit;

    private void Start()
    {
        gameObject.SetActive(true);

        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    private void Update()
    {
        sprite.enabled = highlighted;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
        
    }
}
