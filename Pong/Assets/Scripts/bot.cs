using UnityEditor.Callbacks;
using UnityEngine;

public class bot : MonoBehaviour
{
    public float speed;

    public GameObject ball;

    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 positionDifference = ball.transform.position - transform.position;
        
        if (positionDifference.z > 0)
        {
            rb.linearVelocity = new Vector3(0, 0, positionDifference.z ) * speed;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, 0, positionDifference.z ) * speed;
        }
    }
}
