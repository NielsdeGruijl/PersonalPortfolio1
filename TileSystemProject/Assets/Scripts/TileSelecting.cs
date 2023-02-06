using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelecting : MonoBehaviour
{
    [SerializeField] NodeGridGenerator gridGenerator;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Node nodeScript;

    Camera cam;

    Vector3 mousePos;
    Vector3Int mouseCellPos;

    TileBase bob;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        
    }
}
