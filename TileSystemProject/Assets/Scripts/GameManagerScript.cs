using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    [SerializeField] private PlayerScript player;
    [SerializeField] private EnemyScript target;

    private Pathfinding pathFinder;

    public float testFloat;

    bool pathGenerated = false;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;        
    }

    private void Start()
    {
        pathFinder = GetComponent<Pathfinding>();
        
    }

    private void Update()
    {
        /*        if (!pathGenerated)
                {

                    pathGenerated = true;
                }*/

        pathFinder.FindPath(player.GridPosition, target.GridPosition);
    }
}
