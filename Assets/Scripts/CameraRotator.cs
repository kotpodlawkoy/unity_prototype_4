using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float rotationSpeed;
    private float horizontalInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis ( "Horizontal" );
        transform.Rotate ( Vector3.up, -1f * horizontalInput * rotationSpeed * Time.deltaTime );
    }
}
