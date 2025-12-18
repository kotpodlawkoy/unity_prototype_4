using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float speed;
    public float missileImpulse;
    
    public GameObject enemy;
    public float aliveTimer = 5f;
    // Update is called once per frame
    void Start ()
    {
        Destroy ( gameObject, aliveTimer );
    }

    void Update()
    {
        if ( enemy == null )
        {
            Destroy ( gameObject );
        }
        transform.LookAt ( enemy.transform );
        transform.Rotate ( 90f, 0f, 0f );

        transform.Translate ( Vector3.up * speed * Time.deltaTime );
    }

    void OnCollisionEnter ( Collision col )
    {
        if ( col.gameObject.CompareTag ( "Enemy" ) )
        {
            Rigidbody enemyRB = enemy.GetComponent < Rigidbody > ();
            Vector3 forceDir = enemy.transform.position - transform.position;
            enemyRB.AddForce (  forceDir * missileImpulse, ForceMode.Impulse );
            Destroy ( gameObject );
        }
    }
}
