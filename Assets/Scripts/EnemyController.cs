using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isBoss;
    public int miniEnemiesAmount;

    public float pushForce;
    public float lowerBorder;

    public float spawnWaveTimer = 5f;
    private float timeNextSpawn = 0f;

    private GameObject player;
    private Rigidbody enemyRB;
    private SpawnManager spawnManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find ( "Player" );
        enemyRB = GetComponent < Rigidbody > ();
        if ( isBoss )
        {
            spawnManager = GameObject.Find ( "SpawnManager" ).GetComponent < SpawnManager > ();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = ( player.transform.position - gameObject.transform.position ).normalized;
        enemyRB.AddForce ( lookDirection * pushForce * Time.deltaTime );

        if ( transform.position.y < lowerBorder )
        {
            Destroy ( gameObject );
        }

        if ( isBoss )
        {
            if ( Time.time > timeNextSpawn )
            {
                spawnManager.SpawnMiniEnemies ( miniEnemiesAmount );
                timeNextSpawn = Time.time + spawnWaveTimer;
            }
        }
    }
}
