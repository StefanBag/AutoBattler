// UnitAI.cs
using UnityEngine;
using UnityEngine.AI;

public enum UnitTeam { Player, Enemy }

public class UnitAI : MonoBehaviour
{
    [Header("Identity")]
    public UnitTeam team = UnitTeam.Player;

    [Header("State")]
    public bool isOnBench = false;

    [Header("NavMesh")]
    protected NavMeshAgent agent;
    public UnitData unit_data;
    protected Transform currentTarget;
    protected float attackTimer = 0f;
    protected float currentHealth;
    protected bool inCombat = false;

    protected UnitAnimator unitAnimator;
    private Transform unitsFolder;

    // Runtime buff — never touches the ScriptableObject
    private int buffHealth = 0;
    private int buffDamage = 0;
    private bool buffApplied = false;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = unit_data.health;
        unitAnimator = GetComponent<UnitAnimator>();

        GameObject unitsObj = GameObject.Find("Units");
        if (unitsObj != null)
            unitsFolder = unitsObj.transform;
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
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
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
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
                agent.SetDestination(currentTarget.position);
        }
    }

    protected virtual void Attack(Transform target)
    {
        UnitAI targetUnit = target.GetComponent<UnitAI>();
        if (targetUnit != null)
            targetUnit.TakeDamage(unit_data.damage + buffDamage);

        unitAnimator?.StartAttackAnim();
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
            Die();
    }

    protected virtual void Die()
    {
        inCombat = false;

        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            agent.ResetPath();

        gameObject.SetActive(false);
    }

    // ── Trait Buff API ────────────────────────────────────────────
    public void ApplyTraitBuff(int bonusHealth, int bonusDamage)
    {
        if (buffApplied) RemoveTraitBuff();

        buffHealth = bonusHealth;
        buffDamage = bonusDamage;
        currentHealth += buffHealth;
        buffApplied = true;
    }

    public void RemoveTraitBuff()
    {
        if (!buffApplied) return;

        currentHealth -= buffHealth;
        currentHealth = Mathf.Max(currentHealth, 1f);
        buffHealth = 0;
        buffDamage = 0;
        buffApplied = false;
    }
    // ─────────────────────────────────────────────────────────────

    protected Transform FindClosestEnemy()
    {
        UnitTeam enemyTeam = team == UnitTeam.Player ? UnitTeam.Enemy : UnitTeam.Player;
        UnitAI[] allUnits = FindObjectsByType<UnitAI>(FindObjectsSortMode.None);

        Transform closest = null;
        float closestDist = Mathf.Infinity;

        foreach (UnitAI unit in allUnits)
        {
            if (unit == null) continue;
            if (unit == this) continue;
            if (!unit.gameObject.activeInHierarchy) continue;
            if (!unit.enabled) continue;
            if (unit.team != enemyTeam) continue;
            if (IsShopTemplate(unit.gameObject)) continue;

            float dist = Vector3.Distance(transform.position, unit.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = unit.transform;
            }
        }

        return closest;
    }

    bool IsShopTemplate(GameObject obj)
    {
        if (obj == null || unitsFolder == null) return false;
        return obj.transform.IsChildOf(unitsFolder);
    }

    void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    public void StartCombat()
    {
        inCombat = true;
        currentTarget = null;
        attackTimer = 0f;
        isOnBench = false;
        transform.SetParent(null);

        if (agent != null && agent.enabled)
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
                agent.Warp(hit.position);
        }
    }

    public void StopCombat()
    {
        inCombat = false;
        currentTarget = null;

        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            agent.ResetPath();
    }
}