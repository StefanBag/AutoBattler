using UnityEngine;
using UnityEngine.AI;

public enum UnitTeam { Player, Enemy }

public class UnitAI : MonoBehaviour
{
    [Header("Identity")]
    public UnitTeam team = UnitTeam.Player;
    public UnitTrait trait1 = UnitTrait.Sun;
    public UnitTrait trait2 = UnitTrait.Ocean;

    [Header("NavMesh")]
    protected NavMeshAgent agent;
    public UnitData unit_data;
    protected Transform currentTarget;
    protected float attackTimer = 0f;
    protected float currentHealth;
    protected bool inCombat = false;

    // ADDED
    protected UnitAnimator unitAnimator;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = unit_data.health;
        // ADDED
        unitAnimator = GetComponent<UnitAnimator>();
    }

    protected virtual void Update()
    {
        if (!inCombat) return;

        attackTimer -= Time.deltaTime;

        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
            currentTarget = FindClosestEnemy();

        if (currentTarget == null) return;

        float dist = Vector3.Distance(transform.position, currentTarget.position);

        if (dist <= unit_data.range)
        {
            agent.ResetPath();
            FaceTarget(currentTarget);

            if (attackTimer <= 0f)
            {
                Attack(currentTarget);
                attackTimer = unit_data.cooldown;
            }
        }
        else
        {
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
                agent.SetDestination(currentTarget.position);
        }
    }

    protected virtual void Attack(Transform target)
    {
        // ADDED
        unitAnimator?.TriggerAttack();

        UnitAI targetUnit = target.GetComponent<UnitAI>();
        if (targetUnit != null)
            targetUnit.TakeDamage(unit_data.damage);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f) Die();
    }

    protected virtual void Die()
    {
        inCombat = false;
        agent.ResetPath();
        gameObject.SetActive(false);
    }

    protected Transform FindClosestEnemy()
    {
        UnitTeam enemyTeam = team == UnitTeam.Player ? UnitTeam.Enemy : UnitTeam.Player;
        UnitAI[] allUnits = FindObjectsByType<UnitAI>(FindObjectsSortMode.None);
        Transform closest = null;
        float closestDist = Mathf.Infinity;

        foreach (UnitAI unit in allUnits)
        {
            if (unit.team != enemyTeam || !unit.gameObject.activeInHierarchy) continue;
            float dist = Vector3.Distance(transform.position, unit.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = unit.transform;
            }
        }

        return closest;
    }

    void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    public void StartCombat()
    {
        inCombat = true;
        currentTarget = null;
        attackTimer = 0f;

        transform.SetParent(null);

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            agent.Warp(hit.position);
    }

    public void StopCombat()
    {
        inCombat = false;
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            agent.ResetPath();
        currentTarget = null;
    }
}