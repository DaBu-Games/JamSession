using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform floor;
    [SerializeField] private float spawnY;
    [SerializeField] private GameObject prefab; 
    [SerializeField] private Vector2 gridSize = new Vector2(100, 100);
    [SerializeField] private int objectCount = 50;
    
    [Header("Grid Settings")]
    [SerializeField] private int cellSize = 2; 
    
    private HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();

    private void Start()
    {
        int outOfBeatIndex = Random.Range(0, objectCount);
        
        for (int i = 0; i < objectCount; i++)
        {
            GameObject obj = Instantiate(prefab, GetPosition(), Quaternion.identity);
            
            IBeatScript script = null;
            switch (Random.Range(0, 3))
            {
                case 0: script = obj.AddComponent<BeatMover>(); break;
                case 1: script = obj.AddComponent<BeatRotate>(); break;
                case 2: script = obj.AddComponent<BeatScale>(); break;
            }
            
            if (i == outOfBeatIndex)
                script?.SetMoveToBeat();
        }
    }

    private Vector3 GetPosition()
    {
        while(true)
        {
            Vector3 pos = new Vector3(
                Random.Range(-gridSize.x / 2f, gridSize.x / 2f),
                spawnY,
                Random.Range(-gridSize.y / 2f, gridSize.y / 2f)
            );

            pos.x = Mathf.Round(pos.x / cellSize) * cellSize;
            pos.z = Mathf.Round(pos.z / cellSize) * cellSize;
            
            Vector2 gridPos = new Vector2(pos.x, pos.z);

            if (!occupiedPositions.Contains(gridPos))
            {
                occupiedPositions.Add(gridPos);
                return pos;
            }
        }
    }
}
