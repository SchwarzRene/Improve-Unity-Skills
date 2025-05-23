using UnityEngine;
public class player : MonoBehaviour
{
    public float speed;
    
    private Vector3 movement;

    private Rigidbody rb;
    void Start()
    {
        movement = new Vector3(0, 0, 0);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.MovePosition(rb.position + new Vector3(0, 0, speed) * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.MovePosition( rb.position + new Vector3(0, 0, -speed) * Time.deltaTime );
        }
        */

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.linearVelocity = new Vector3(0, 0, speed);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.linearVelocity = new Vector3(0, 0, -speed);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
        }
    }
}
