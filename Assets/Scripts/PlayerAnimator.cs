using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private float locomationAnimationSmoothTime = .1f;

    private Animator animator;
    private Player player;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
    }

    private void Update() {
        float speedPercent = player.CurrentVelocity / player.MaxVelocity;
        animator.SetFloat("speedPercent", speedPercent, locomationAnimationSmoothTime, Time.deltaTime);
    }
}
