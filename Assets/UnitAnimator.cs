using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class UnitAnimator : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

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

    public void TriggerAttack()
    {
        animator.SetBool(IsAttacking, true);
    }

    public void EndAttack()
    {
        animator.SetBool(IsAttacking, false);
    }
}