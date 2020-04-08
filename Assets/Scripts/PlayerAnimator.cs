using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private float locomationAnimationSmoothTime = .1f;

    private Animator animator;
    private Rigidbody rb;
    private Player player;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
    }

    private void Update() {
        float speedPercent = player.movement.magnitude;
        if (speedPercent > 1) {
            speedPercent = 1;
        }
        Debug.Log(speedPercent);
        animator.SetFloat("speedPercent", speedPercent, locomationAnimationSmoothTime, Time.deltaTime);
    }
}
