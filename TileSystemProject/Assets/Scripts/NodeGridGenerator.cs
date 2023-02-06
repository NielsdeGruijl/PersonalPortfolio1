using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeGridGenerator : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap tilemap2;
    [SerializeField] List<Tilemap> tilemaps;
    [SerializeField] GameObject node;
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

        ClearNodes();

        GameObject originObject = Instantiate(origin, tilemaps[0].origin, Quaternion.identity);
        originNodes.Add(originObject);

        GameObject originObject2 = Instantiate(origin, tilemaps[1].origin, Quaternion.identity);
        originNodes.Add(originObject2);

        foreach (Tilemap map in tilemaps)
        {
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
                        if (map == tilemaps[0])
                        {
                            GameObject nodeObject = Instantiate(node, new Vector2(x, y), Quaternion.identity);
                            nodeObject.transform.SetParent(map.transform);
                            Node script = nodeObject.GetComponent<Node>();

                            script.walkable = true;
                            script.sprite.color = new Color32(255, 255, 255, 100);

                            walkableNodes.Add(nodeObject);
                            //nodes.Add(nodeObject);
                        }
                        if (map == tilemaps[1])
                        {
                            GameObject nodeObject = Instantiate(obstacleNode, new Vector2(x, y), Quaternion.identity);
                            nodeObject.transform.SetParent(map.transform);
                            Node script = nodeObject.GetComponent<Node>();

                            script.walkable = false;
                            script.sprite.color = new Color32(255, 0, 0, 100);

                            obstacleNodes.Add(nodeObject);
                            //nodes.Add(nodeObject);
                        }
                    }
                }
            }
        }
    }

    private void CheckOverlap()
    {
        foreach(GameObject node in obstacleNodes)
        {

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
