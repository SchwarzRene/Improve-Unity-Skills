using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private InputController inputController;

    private bool isWalking;
    void Start()
    {
        isWalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = inputController.GetInputVectorNormalized();

        
        //Transform
        Vector3 moveDir = new Vector3(inputVector.x, 0.0f, inputVector.y);
        transform.position += moveDir * playerSpeed * Time.deltaTime;

        isWalking = inputVector != Vector2.zero;

        //Rotation
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * playerSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
