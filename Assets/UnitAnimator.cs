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

        yield return null; // wait one frame for animator to transition

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float clipLength = Mathf.Clamp(stateInfo.length, 0.1f, 5f);

        yield return new WaitForSeconds(clipLength);

        animator.SetBool(IsAttacking, false);
        attackCoroutine = null;
    }
}