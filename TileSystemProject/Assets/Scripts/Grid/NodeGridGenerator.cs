using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeGridGenerator : MonoBehaviour
{
    [SerializeField] List<Tilemap> tilemaps;
    [SerializeField] GameObject nodePrefab;
    [SerializeField] GameObject obstacleNode;
    [SerializeField] GameObject origin;

    public List<GameObject> originNodes;
    public List<GameObject> walkableNodes;
    public List<GameObject> obstacleNodes;

    float mapWidth;
    float mapHeight;

    private void Start()
    {
        print(tilemaps.Count);
    }

    public void GenerateNodeGrid()
    {
        int nameIndex = 0;

        ClearNodes();

        GameObject originObject = Instantiate(origin, tilemaps[0].origin, Quaternion.identity);
        originNodes.Add(originObject);

        GameObject originObject2 = Instantiate(origin, tilemaps[1].origin, Quaternion.identity);
        originNodes.Add(originObject2);

        foreach (Tilemap map in tilemaps)
        {
            TilemapType mapScript = map.GetComponentInParent<TilemapType>();

            mapWidth = map.size.x;
            mapHeight = map.size.y;

            float cellWidth = map.cellSize.x;
            float cellHeight = map.cellSize.y;

            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    float x = map.origin.x + (i * cellWidth + cellWidth / 2);
                    float y = map.origin.y + (j * cellHeight + cellHeight / 2);

                    if (map.HasTile(new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y), 0)))
                    {
                        if (mapScript.walkable)
                        {
                            GameObject nodeObject = Instantiate(nodePrefab, new Vector2(x, y), Quaternion.identity);
                            nodeObject.transform.SetParent(map.transform);
                            nodeObject.name = "node" + nameIndex;
                            nameIndex++;
                            Node nodeScript = nodeObject.GetComponent<Node>();

                            nodeScript.walkable = true;

                            walkableNodes.Add(nodeObject);
                        }
                        if (mapScript.obstacle)
                        {
                            GameObject nodeObject = Instantiate(obstacleNode, new Vector2(x, y), Quaternion.identity);
                            nodeObject.transform.SetParent(map.transform);
                            Node script = nodeObject.GetComponent<Node>();

                            script.isObstacle = true;
                            script.walkable = false;

                            obstacleNodes.Add(nodeObject);
                        }
                    }
                }
            }
        }

        CheckOverlap();

        SetNeighbours();
    }

    private void CheckOverlap()
    {
        foreach(GameObject oNode in obstacleNodes)
        {
            for(int i = 0; i < walkableNodes.Count; i++)
            {
                if (walkableNodes[i].transform.position == oNode.transform.position)
                {
                    DestroyImmediate(walkableNodes[i]);
                    walkableNodes.Remove(walkableNodes[i]);
                }
            }
        }
    }

    private void SetNeighbours()
    {
        for(int i = 0; i < walkableNodes.Count; i++)
        {
            foreach(GameObject node in walkableNodes)
            {
                float distance = (node.transform.position - walkableNodes[i].transform.position).magnitude;
                if(distance < 1.1f && distance > 0)
                {
                    walkableNodes[i].GetComponent<Node>().neighbours.Add(node.GetComponent<Node>());
                }
            }
        }
    }

    public void ClearNodes()
    {
        for(int i = 0; i < originNodes.Count; i++)
        {
            DestroyImmediate(originNodes[i]);
        }

        for(int i = 0; i < walkableNodes.Count; i++)
        {
            DestroyImmediate(walkableNodes[i]);
        }


        for (int i = 0; i < obstacleNodes.Count; i++)
        {
            DestroyImmediate(obstacleNodes[i]);
        }

        originNodes.Clear();
        walkableNodes.Clear();
        obstacleNodes.Clear();
    }
}
