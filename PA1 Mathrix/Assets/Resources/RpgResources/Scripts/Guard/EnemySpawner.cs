using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject EnemyPrefab;
    public int NumberOfEnemies;

    public override void OnStartServer()
    {
        int SpawnPointIndex = Random.Range(1, GlobalVariables.singleton.GuardPoints.Count);
        var enemy = (GameObject)Instantiate(EnemyPrefab, GlobalVariables.singleton.GuardPoints[SpawnPointIndex],Quaternion.identity);
        enemy.transform.parent = GameObject.Find("Enemies").transform;
        NetworkServer.Spawn(enemy);
    }
}
