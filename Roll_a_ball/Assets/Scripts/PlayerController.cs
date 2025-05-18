using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    private int count;

    public float speed;
    public TextMeshProUGUI countText;
    public GameObject winText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        count = 0;
        rb = GetComponent <Rigidbody> ();
        SetCountText();
        winText.SetActive( false );
    }
    
    void OnMove( InputValue movementValue ){
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate() {
        Vector3 movement = new Vector3( movementX, 0.0f, movementY );
        rb.AddForce( movement * speed );
    }

    void OnTriggerEnter(Collider other) {
        if ( other.gameObject.CompareTag( "PickUp" ) ){
            count = count + 1;
            other.gameObject.SetActive( false );
            SetCountText();

            if ( count >= 8 ){
                winText.SetActive( true );
            }
        }
        
    }

    void SetCountText(){
        countText.text = "Count: " + count.ToString();
    }
}
