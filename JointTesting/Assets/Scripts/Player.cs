using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Joint GameObjects")]
    [SerializeField] private GameObject jointParent;
    [SerializeField] private GameObject jointParent2;

    [Header("Target Angles")]
    public float angle;
    public float angle2;

    private HingeJoint joint;
    private HingeJoint joint2;

    private const float motorForce = 100f;
    private const float velocityMultiplier = 10f;

    void Start()
    {
        joint = jointParent.GetComponent<HingeJoint>();
        joint.useMotor = true;
        angle = joint.angle;

        joint2 = jointParent2.GetComponent<HingeJoint>();
        joint2.useMotor = true;
        angle2 = joint2.angle;
    }

    void Update()
    {
        UpdateJointMotor(joint, angle);
        UpdateJointMotor(joint2, angle2);
    }

    private void UpdateJointMotor(HingeJoint hinge, float targetAngle)
    {
        float currentAngle = hinge.angle;
        float angleDifference = targetAngle - currentAngle;

        JointMotor motor = hinge.motor;
        motor.force = motorForce;
        motor.targetVelocity = angleDifference * velocityMultiplier;

        hinge.motor = motor;
    }
}
