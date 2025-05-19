using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    public float speed;
    
    private Vector3 movement;

    private Rigidbody rb;
    void Start()
    {
        movement = new Vector3(0, 0, 0);
    }

    void OnMove(InputValue input)
    {
        Vector2 key_movement = input.Get<Vector2>();
        movement.x = key_movement.x * speed;
        movement.z = key_movement.y * speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + movement;
    }
}
