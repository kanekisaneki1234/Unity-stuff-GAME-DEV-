using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BoarSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyReferences;
    private GameObject spawnedEnemy;

    // [SerializeField]
    public Transform spawnLoc;
    // private int spawnIndex;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnMonsters());
    }

    void Update()
    {
        // SceneController.done = 10; 
    }

    IEnumerator SpawnMonsters()
    {
        // while(true)
        {
            yield return new WaitForSeconds(5);
            spawnedEnemy = Instantiate(enemyReferences[0]);
            spawnedEnemy.transform.position = spawnLoc.position;
        }
    }

    

}
