using UnityEngine;

public class ball : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 startSpeed = new Vector3(Random.Range( -1, 1 ), 0, Random.Range( -1, 1 ));
        startSpeed.z += (float)0.1;
        rb.linearVelocity = startSpeed.normalized * speed;
    }

    // Update is called once per frame

    void Update()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 collisionEnterPoint = collision.contacts[0].point;
        Vector3 centerDistance = collisionEnterPoint - transform.position;
        centerDistance.y *= 0;
        rb.linearVelocity = -centerDistance.normalized * speed;
    }
}
