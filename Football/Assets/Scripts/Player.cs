using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.Mathematics;

public class Player : Agent
{
    [SerializeField] private float playerSpeed = 10;

    [SerializeField] private float kickStrength = 10;
    [SerializeField] private float kickAngle = 30;
    [SerializeField] private Ball ball;

    [SerializeField] private GameObject goal;

    private Rigidbody rb;
    public float stepCount;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = 3;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        var continuousActions = actionsOut.ContinuousActions;

        int movement = 0;
        int kick = 0;
        float direction = 0f;

        bool w = Input.GetKey(KeyCode.W);
        bool s = Input.GetKey(KeyCode.S);
        bool a = Input.GetKey(KeyCode.A);
        bool d = Input.GetKey(KeyCode.D);

        if (w && d) movement = 5;
        else if (w && a) movement = 6;
        else if (s && d) movement = 7;
        else if (s && a) movement = 8;
        else if (w) movement = 1;
        else if (s) movement = 2;
        else if (d) movement = 3;
        else if (a) movement = 4;

        if (Input.GetKey(KeyCode.Space)) kick = 1;

        // Optional: Assign a kick direction using keyboard (e.g., Q/E or mouse input)
        if (Input.GetKey(KeyCode.Q)) direction = -1f;
        else if (Input.GetKey(KeyCode.E)) direction = 1f;

        discreteActions[0] = movement;
        discreteActions[1] = kick;
        continuousActions[0] = direction;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(ball.transform.localPosition);
        sensor.AddObservation(ball.rb.linearVelocity);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.eulerAngles);
        sensor.AddObservation(rb.linearVelocity);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveAgent(actions);
    }


    private void MoveAgent(ActionBuffers actions)
    {
        int movementAction = actions.DiscreteActions[0]; // 0–8
        int kickAction = actions.DiscreteActions[1];     // 0 or 1
        float kickDirection = actions.ContinuousActions[0]; // continuous [-1, 1]

        Vector3 moveDir = Vector3.zero;
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;

        switch (movementAction)
        {
            case 1: moveDir = forward; break;
            case 2: moveDir = -forward; break;
            case 3: moveDir = right; break;
            case 4: moveDir = -right; break;
            case 5: moveDir = (forward + right).normalized; break;
            case 6: moveDir = (forward - right).normalized; break;
            case 7: moveDir = (-forward + right).normalized; break;
            case 8: moveDir = (-forward - right).normalized; break;
            default: moveDir = Vector3.zero; break;
        }

        rb.AddForce(moveDir * playerSpeed, ForceMode.VelocityChange);

        // 🔁 Use the continuous kick direction input
        bool ballKicked = kickAction == 1 ? CheckForKick(1f, kickDirection) : false;
        AddReward(-0.0002f);

    }

    private bool CheckForKick(float kick, float direction)
    {
        RaycastHit hit;
        Vector3 boxCenter = rb.transform.position + new Vector3(0, 0.25f, 0);
        Vector3 boxSize = Vector3.one * 0.25f;

        if (Physics.BoxCast(boxCenter, boxSize, rb.transform.forward, out hit, rb.transform.rotation, 0.25f))
        {
            if (hit.collider.gameObject.name == "Ball" && kick > 0.5f)
            {
                // Clamp direction [-1, 1] and convert to angle [-kickAngle, +kickAngle]
                direction = Mathf.Clamp(direction, -1f, 1f);
                float angle = Mathf.Lerp(-kickAngle, kickAngle, (direction + 1f) / 2f);

                // Get current movement direction
                Vector3 baseDirection = rb.linearVelocity.normalized;

                // Rotate it by the angle around the Y axis
                Vector3 rotatedDirection = Quaternion.AngleAxis(angle, Vector3.up) * baseDirection;

                // Apply the kick
                ball.ApplyKick(rotatedDirection * kickStrength);
                return true;
            }
        }

        return false;
    }

    public void Reset()
    {
        transform.localPosition = new Vector3(UnityEngine.Random.value * 8 - 4, 0.0f, UnityEngine.Random.value * 8 - 4);
        transform.eulerAngles = Vector3.zero;
        stepCount = 1;
    }

    public void GoalShot(float reward)
    {
        SetReward(reward);
    }

    

    public override void OnEpisodeBegin()
    {
        Reset();
    }
}
