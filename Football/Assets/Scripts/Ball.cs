using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float wallBounceStrength;
    [SerializeField] private float dampingFactor;

    [SerializeField] private GameControll gameControll;
    public Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity *= dampingFactor;
    }

    public void ApplyKick(Vector3 direction)
    {
        rb.AddForce(direction);
    }

    public void Reset()
    {
        transform.localPosition = new Vector3(Random.value * 8 - 4, 0.0f, Random.value * 8 - 4);
        rb.linearVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            Vector3 collisionPoint = collision.contacts[0].point;
            Vector3 centerToCollisionVector = collisionPoint - rb.transform.localPosition;

            centerToCollisionVector.y = 0;
            centerToCollisionVector = -centerToCollisionVector;

            rb.AddForce(centerToCollisionVector * wallBounceStrength);
        }
        if (collision.collider.CompareTag("RedGoal") || collision.collider.CompareTag("BlueGoal"))
        {
            gameControll.GoalShoot(collision.collider.tag);
        }

    }

}
