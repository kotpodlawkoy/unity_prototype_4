using UnityEngine;

public class ParticleFollowPlayer : MonoBehaviour
{
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find ( "Player" );
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 resForce = player.gameObject.GetComponent < Rigidbody > ().GetAccumulatedForce ();
        transform.position = player.transform.position;
        transform.rotation = Quaternion.Euler ( 0f, 0f,
                                                Vector3.Angle ( Vector3.forward,
                                                                player.gameObject.GetComponent < Rigidbody > ().GetAccumulatedForce () ) );
 
        Vector3 normResForce = resForce.normalized;
        transform.Translate ( new Vector3 ( normResForce.x,
                                            -0.5f,
                                            normResForce.z ) );
    }
}
