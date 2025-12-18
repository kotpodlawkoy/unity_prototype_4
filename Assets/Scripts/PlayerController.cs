using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public ParticleSystem explosion;
    public float timeToSmash = 0.5f;
    public float gravityMultiplier;
    private bool smashing = false;
    public float upImpulse;
    public float smashImpulse;
    public float explosionRadius = 9f;
    private bool isInAir = false;
    public GameObject missilePrefab;
    public Powerups hasPowerup = Powerups.None;
    public float poweredForce;
    public GameObject powerupIndicator;
    public float powerupIndicatorRotationSpeed;
    public float pushForce;
    private float verticalInput; 
    private Rigidbody playerRB;
    private GameObject focalPoint;
    private EnemyController [] enemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GetComponent < Rigidbody > ();
        focalPoint = GameObject.Find ( "Focal Point" );
        Physics.gravity *= gravityMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis ( "Vertical" );
        playerRB.AddForce ( focalPoint.transform.forward * verticalInput * pushForce * Time.deltaTime );

        powerupIndicator.transform.position = transform.position + new Vector3( 0f, -0.4f, 0f ); 
        powerupIndicator.transform.Rotate ( Vector3.up, powerupIndicatorRotationSpeed * Time.deltaTime );

        if ( Input.GetKeyDown ( KeyCode.Space ) && hasPowerup == Powerups.Smash && !smashing )
        {
            //playerRB.AddForce ( Vector3.up * upImpulse, ForceMode.VelocityChange );
            //isInAir = true;
            smashing = true;
            StartCoroutine ( Smash ( timeToSmash ) );
        }
    }

    void OnTriggerEnter ( Collider other )
    {
        if ( other.gameObject.CompareTag ( "Powerup" ) )
        {
            hasPowerup = other.gameObject.GetComponent < Powerup > ().type;
            if ( hasPowerup == Powerups.Missile )
            {
                SpawnMissile ();
            }
            else
            {
                powerupIndicator.SetActive ( true );
                StartCoroutine ( CountDownRoutine () );
            }
            Destroy ( other.gameObject );
        }
    }

    void OnCollisionEnter ( Collision collision )
    {
        if ( collision.gameObject.CompareTag ( "Enemy" ) && hasPowerup == Powerups.Shove )
        {
            Vector3 lookDirection = collision.gameObject.transform.position - gameObject.transform.position;
            Rigidbody enemyRB = collision.gameObject.GetComponent < Rigidbody > ();
            enemyRB.AddForce ( lookDirection * poweredForce, ForceMode.Impulse );
        }
        //if ( isInAir && hasPowerup == Powerups.Smash && collision.gameObject.CompareTag ( "Ground" ) )
        //{
        //    isInAir = false;
        //    enemies = GameObject.FindObjectsByType < EnemyController > ( FindObjectsSortMode.None );
        //    for ( int i = 0; i < enemies.Length; i ++ )
        //    {
        //        Rigidbody enRB = enemies [ i ].gameObject.GetComponent < Rigidbody > ();
        //        Vector3 forceVec = enemies [ i ].gameObject.transform.position - transform.position;
        //        enRB.AddForce ( forceVec * smashImpulse, ForceMode.Impulse );
        //    }
        //}
    }

    IEnumerator CountDownRoutine ()
    {
        yield return new WaitForSeconds ( 7 );
        hasPowerup = Powerups.None;
        powerupIndicator.SetActive ( false );
    }

    void SpawnMissile ()
    {
        enemies = GameObject.FindObjectsByType < EnemyController > ( FindObjectsSortMode.None );
        for ( int i = 0; i < enemies.Length; i ++ )
        {
            GameObject missile = Instantiate ( missilePrefab,
                                               transform.position,
                                               missilePrefab.transform.rotation );
            missile.GetComponent < MissileController > ().enemy = enemies [ i ].gameObject;
        }
    }

    IEnumerator Smash ( float smashTime )
    {
        enemies = GameObject.FindObjectsByType < EnemyController > ( FindObjectsSortMode.None );
        float jumpTime = Time.time + smashTime;
        float floorY = transform.position.y;
        while ( Time.time < jumpTime )
        {
            playerRB.linearVelocity = new Vector3 ( 0f, upImpulse, 0f );
            yield return null;
        }

        while ( transform.position.y > floorY )
        {
            playerRB.linearVelocity = new Vector3 ( 0f, -2 * upImpulse, 0f );
            yield return null;
        }
        foreach ( var ene in enemies )
        {
            if ( ene != null )
            {
                ene.GetComponent < Rigidbody > ().AddExplosionForce ( smashImpulse, transform.position, explosionRadius, 0f, ForceMode.Impulse );
            }
        }
        explosion.Play();
        smashing = false;
    }
}
