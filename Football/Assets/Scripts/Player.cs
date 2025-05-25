using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Player : Agent
{
    [SerializeField] private float playerSpeed = 10;
    [SerializeField] private float rotationSpeed = 10;


    [SerializeField] private float kickStrength = 10;
    [SerializeField] private float kickAngle = 30;
    [SerializeField] private Ball ball;

    [SerializeField] private GameObject goal;

    private Rigidbody rb;
    private Animator animator;

    public float stepCount;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = 3;

        animator = GetComponent<Animator>();
        animator.SetInteger("IsWalking", -1);
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
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

        var continousActions = actionsOut.ContinuousActions;
        continousActions[0] = forwardSpeed;
        continousActions[1] = sideSpeed;
        continousActions[2] = rotation;
        continousActions[3] = kick;
        continousActions[4] = kickDirection;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(ball.transform.position);
        sensor.AddObservation(ball.rb.linearVelocity);
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.eulerAngles);
        sensor.AddObservation(rb.linearVelocity);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        stepCount += 1;
        AddReward(-0.0005f);
        MoveAgent(actions);
    }


    private void MoveAgent(ActionBuffers actions)
    {
        float forwardSpeed = actions.ContinuousActions[0]; float sideSpeed = actions.ContinuousActions[1]; float rotation = actions.ContinuousActions[2]; float kick = actions.ContinuousActions[3]; float kickDirection = actions.ContinuousActions[4];

        bool ballKicked = CheckForKick(kick, kickDirection);

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
        rb.AddForce(moveDir * playerSpeed, ForceMode.VelocityChange);

        //Rotation
        Vector3 rotationVector = new Vector3(0, rotation * rotationSpeed, 0);
        transform.Rotate(rotationVector);


        //Animation
        if (ballKicked)
        {
            animator.SetInteger("IsWalking", -1);
            animator.SetBool("IsShooting", true);
        }
        else
        {
            animator.SetBool("IsShooting", false);

            if (forwardSpeed == 1)
            {
                animator.SetInteger("IsWalking", 0);
            }
            else if (forwardSpeed == -1)
            {
                animator.SetInteger("IsWalking", 2);
            }
            else if (sideSpeed == 1)
            {
                animator.SetInteger("IsWalking", 1);
            }
            else if (sideSpeed == -1)
            {
                animator.SetInteger("IsWalking", 3);
            }
            else
            {
                animator.SetInteger("IsWalking", -1);
            }
        }
    }

    private bool CheckForKick(float kick, float direction)
    {
        RaycastHit hit;
        Vector3 boxCenter = rb.transform.position + new Vector3(0, 0.25f, 0);
        Vector3 boxSize = Vector3.one * 0.25f;
        if (Physics.BoxCast(boxCenter, boxSize, rb.transform.forward, out hit, rb.transform.rotation, 0.25f))
        {

            if (hit.collider.gameObject.name == "Ball" && kick > 0.5)
            {
                direction = Mathf.Clamp(direction, -1f, 1f);
                float angle = Mathf.Lerp(-kickAngle, kickAngle, (direction + 1f) / 2f);

                Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 rotatedDirection = rotation * rb.transform.forward;
                ball.ApplyKick(rotatedDirection * kickStrength);

                return true;
            }
        }
        return false;
    }

    public void Reset()
    {
        transform.position = new Vector3(Random.value * 8 - 4, 0.0f, Random.value * 8 - 4);
        transform.eulerAngles = Vector3.zero;
        stepCount = 1;
    }
}
