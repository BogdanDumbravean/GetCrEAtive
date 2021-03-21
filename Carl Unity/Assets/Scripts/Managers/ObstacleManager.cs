using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private const float minX = 20, maxX = 200;
    private const float midPoint1 = 100, midPoint2 = 150;

    public int nrObstacles1, nrObstacles2, nrObstacles3;
    public GameObject prefab;
    
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        for(int i = 0; i < nrObstacles1; ++i) {
            SpawnObstacle(minX, midPoint1);
        }
        for(int i = 0; i < nrObstacles2; ++i) {
            SpawnObstacle(midPoint1, midPoint2);
        }
        for(int i = 0; i < nrObstacles3; ++i) {
            SpawnObstacle(midPoint2, maxX);
        }
    }

    private void SpawnObstacle(float left, float right) {
        float laneHeight = Lanes.height[Random.Range(0, 3)];
        Instantiate(prefab, new Vector3(Random.Range(left, right), laneHeight, laneHeight - 1f), Quaternion.identity);
    }
}
