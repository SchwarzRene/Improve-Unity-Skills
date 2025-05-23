using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Player player;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsWalking", false);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsWalking", player.IsWalking() );
    }
}
