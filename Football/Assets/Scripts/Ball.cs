using System;
using Unity.MLAgents.Integrations.Match3;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float wallBounceStrength;
    [SerializeField] private float dampingFactor;

    [SerializeField] private GameControll gameControll;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity -= rb.linearVelocity * dampingFactor * Time.deltaTime;
    }

    public void ApplyKick(Vector3 direction)
    {
        rb.AddForce(direction);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            Vector3 collisionPoint = collision.contacts[0].point;
            Vector3 centerToCollisionVector = collisionPoint - rb.transform.position;

            Debug.Log("Redirection Vector " + centerToCollisionVector);

            centerToCollisionVector.y = 0;
            centerToCollisionVector = -centerToCollisionVector;

            Debug.Log("Redirection Vector " + centerToCollisionVector * wallBounceStrength);

            rb.AddForce(centerToCollisionVector * wallBounceStrength);
            Debug.Log("Hit Wall");
        }
        if (collision.collider.CompareTag("RedGoal") || collision.collider.CompareTag("BlueGoal"))
        {
            gameControll.GoalShoot(collision.collider.tag);
        }

    }

}
