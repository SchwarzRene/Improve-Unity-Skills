using System;
using System.Security.Cryptography;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 10;
    [SerializeField] private float rotationSpeed = 10;

    
    [SerializeField] private float kickStrength = 10;
    [SerializeField] private float kickAngle = 30;
    [SerializeField] private Ball ball;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = 3;
    }

    public float[] GetPlayerInput()
    {
        //Get Input for transformation movement
        float forwardSpeed = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            forwardSpeed = +1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            forwardSpeed = -1;
        }

        float sideSpeed = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            sideSpeed = +1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            sideSpeed = -1;
        }

        float rotation = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            rotation = -1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotation = 1;
        }

        float kick = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            kick = 1;
        }
        float kickDirection = 0;
        return new float[] { forwardSpeed, sideSpeed, rotation, kick, kickDirection };
    }

    void Update()
    {

        float[] userInput = GetPlayerInput();
        float forwardSpeed = userInput[0]; float sideSpeed = userInput[1]; float rotation = userInput[2]; float kick = userInput[3]; float kickDirection = userInput[4];

        CheckForKick( kick, kickDirection );

        //The player moves with 3 values. 
        //One is for forth and back this happends with the forward vector
        //One is for sidewise. this happends with a 90 degree rotated vector
        //One is the rotation itself

        Vector3 forwardVector = rb.transform.forward;

        Vector2 plainForwardVector = new Vector2(forwardVector.x, forwardVector.z).normalized;
        //Rotate 90 Degrees
        Vector2 rightForward = new Vector2(-plainForwardVector.y, plainForwardVector.x);

        plainForwardVector = plainForwardVector * forwardSpeed;
        rightForward = rightForward * sideSpeed;

        //Combine the vectors
        Vector2 newForward = plainForwardVector + rightForward;
        Vector3 moveDir = new Vector3(newForward.x, 0.0f, newForward.y);

        //Tranformation
        
        rb.AddForce(moveDir * playerSpeed * Time.deltaTime, ForceMode.VelocityChange );
        Debug.Log(rb.linearVelocity);
        //Rotation
        Vector3 rotationVector = new Vector3(0, rotation * Time.deltaTime * rotationSpeed, 0);
        transform.Rotate(rotationVector);
    }

    private void CheckForKick( float kick, float direction )
    {
        RaycastHit hit;
        Vector3 boxCenter = rb.transform.position + new Vector3(0, 0.25f, 0);
        Vector3 boxSize = Vector3.one * 0.25f;
        if (Physics.BoxCast(boxCenter, boxSize, rb.transform.forward, out hit, rb.transform.rotation, 0.25f))
        {
            Debug.Log(hit.collider.gameObject.name);
            if ( hit.collider.gameObject.name == "Ball" && kick > 0.5 )
            {
                direction = Mathf.Clamp(direction, -1f, 1f);
                float angle = Mathf.Lerp(-kickAngle, kickAngle, (direction + 1f) / 2f); 

                Quaternion rotation = Quaternion.Euler(0f, angle, 0f); 
                Vector3 rotatedDirection = rotation * rb.transform.forward; 
                ball.ApplyKick(rotatedDirection * kickStrength);
            }
        }
    }
}
