using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject [] enemyPrefabs;
    public GameObject [] miniEnemyPrefabs;
    public GameObject bossPrefab;
    public GameObject [] powerupPrefabs;
    public float spawnRangeXandZ = 9f;
    private int waveNumber = 5;
    private int currentEnemyCount;

    // Update is called once per frame
    void Update()
    {
        currentEnemyCount = GameObject.FindObjectsByType < EnemyController > ( FindObjectsSortMode.None ).Length;
        if ( currentEnemyCount == 0 )
        {
            if ( waveNumber % 5 == 0)
            {
                SpawnBoss ();
                SpawnPowerup ();
                waveNumber ++;
            }
            else
            {
                SpawnEnemies ();
                SpawnPowerup ();
                waveNumber ++;
            }
        }
    }

    void SpawnEnemies ()
    {
        for ( int i = 0; i < waveNumber; i ++ )
        {
            if ( ( i + 1 ) % 3 == 0 )
            {
                Instantiate ( enemyPrefabs [ 1 ], GenerateSpawnPoint (), enemyPrefabs [ 1 ].transform.rotation );
                continue;
            }
            Instantiate ( enemyPrefabs [ 0 ], GenerateSpawnPoint (), enemyPrefabs [ 0 ].transform.rotation );
        }
    }

    void SpawnPowerup ()
    {
        int prefabType = Random.Range ( 0, 3 );
        Instantiate ( powerupPrefabs [ prefabType ],
                      GenerateSpawnPoint (),
                      powerupPrefabs [ prefabType ].transform.rotation );
    }

    void SpawnBoss ()
    {
        GameObject boss = Instantiate ( bossPrefab,
                                        GenerateSpawnPoint (),
                                        bossPrefab.transform.rotation );
        boss.GetComponent < EnemyController > ().miniEnemiesAmount = waveNumber;
    }

    public void SpawnMiniEnemies ( int amount )
    {
        for ( int i = 0; i < amount; i ++ )
        {
            int enemyType = Random.Range ( 0, 2 );
            Instantiate ( miniEnemyPrefabs [ enemyType ], GenerateSpawnPoint (), miniEnemyPrefabs [ enemyType ].transform.rotation );
        }
    }

    Vector3 GenerateSpawnPoint ()
    {
        return new Vector3 ( Random.Range ( -1 * spawnRangeXandZ, spawnRangeXandZ ),
                             0,
                             Random.Range ( -1 * spawnRangeXandZ, spawnRangeXandZ ) );
    }
}
