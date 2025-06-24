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
    void Start()
    {
        joint1 = joint1object.GetComponent<ConfigurableJoint>();
        joint2 = joint2object.GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        joint1.targetRotation = Quaternion.Inverse(targetRotation);

        Quaternion targetRotation2 = Quaternion.Euler(0f, 0f, angle2);
        joint2.targetRotation = Quaternion.Inverse(targetRotation2);
    }
}