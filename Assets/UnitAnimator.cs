// UnitAnimator.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class UnitAnimator : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private Coroutine attackCoroutine;

    private static readonly int IsRunning   = Animator.StringToHash("IsRunning");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent    = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        bool moving = agent.isActiveAndEnabled
                   && agent.isOnNavMesh
                   && !agent.isStopped
                   && agent.velocity.sqrMagnitude > 0.01f;

        animator.SetBool(IsRunning, moving && !animator.GetBool(IsAttacking));
    }

    public void StartAttackAnim()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        animator.SetBool(IsAttacking, true);

        // Wait one frame for animator to transition into the attack state
        yield return null;

        // Get the length of whichever clip is now playing
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float clipLength = stateInfo.length;

        // Clamp to something sane in case of loops or bad data
        clipLength = Mathf.Clamp(clipLength, 0.1f, 5f);

        yield return new WaitForSeconds(clipLength);

        animator.SetBool(IsAttacking, false);
        attackCoroutine = null;
    }
}