using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is create
    [SerializeField] GameObject joint1object;
    [SerializeField] GameObject joint2object;

    private ConfigurableJoint joint1;
    private ConfigurableJoint joint2;

    public float angle;
    public float angle2;

    public float projectionDistance = 1f;
    public float projectionAngle = 0.1f;

    public float positionSpring = 10000;
    public float positionDamper = 1000;
    private JointDrive drive;
    void Start()
    {
        drive = new JointDrive
        {
            positionSpring = 100000f,        // Adjust for stronger holding
            positionDamper = 100000f,         // Damping for stability
            maximumForce = Mathf.Infinity  // Infinite force to resist motion
        };

        joint1 = joint1object.GetComponent<ConfigurableJoint>();
        joint1.angularYZDrive = drive;
        joint1.projectionMode = JointProjectionMode.PositionAndRotation;



        joint2 = joint2object.GetComponent<ConfigurableJoint>();
        joint2.angularYZDrive = drive;
        joint2.projectionMode = JointProjectionMode.PositionAndRotation;


    }

    // Update is called once per frame
    void Update()
    {
        drive = new JointDrive
        {
            positionSpring = positionSpring,        // Adjust for stronger holding
            positionDamper = positionDamper,         // Damping for stability
            maximumForce = Mathf.Infinity  // Infinite force to resist motion
        };

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        joint1.projectionDistance = projectionDistance;
        joint1.projectionAngle = projectionAngle;
        joint1.angularYZDrive = drive;
        joint1.targetRotation = Quaternion.Inverse(targetRotation);


        Quaternion targetRotation2 = Quaternion.Euler(0f, 0f, angle2);
        joint2.projectionDistance = projectionDistance;
        joint2.projectionAngle = projectionAngle;
        joint2.angularYZDrive = drive;
        joint2.targetRotation = Quaternion.Inverse(targetRotation2);
    }
}