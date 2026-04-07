using UnityEngine;
using UnityEngine.AI;

// Base class - MeleeUnitAI and RangedUnitAI will extend this
public class UnitAI : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;   // seconds between attacks
    public float attackRange = 2f;      // distance to start attacking
    public string enemyTag = "Enemy";   // set to "Player" on enemy units, "Enemy" on player units

    [Header("NavMesh")]
    protected NavMeshAgent agent;

    protected Transform currentTarget;
    protected float attackTimer = 0f;
    protected float currentHealth;
    protected bool inCombat = false;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        if (!inCombat) return;

        attackTimer -= Time.deltaTime;

        // Find closest enemy if we don't have one
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = FindClosestEnemy();
        }

        if (currentTarget == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        if (distanceToTarget <= attackRange)
        {
            // In range — stop moving and attack
            agent.ResetPath();
            FaceTarget(currentTarget);

            if (attackTimer <= 0f)
            {
                Attack(currentTarget);
                attackTimer = attackCooldown;
            }
        }
        else
        {
            // Move toward target
            agent.SetDestination(currentTarget.position);
        }
    }

    protected virtual void Attack(Transform target)
    {
        // Deal damage to target
        UnitAI targetUnit = target.GetComponent<UnitAI>();
        if (targetUnit != null)
        {
            targetUnit.TakeDamage(attackDamage);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        inCombat = false;
        agent.ResetPath();
        // Override in subclass for death animations etc.
        gameObject.SetActive(false);
    }

    protected Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        Transform closest = null;
        float closestDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = enemy.transform;
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
    }

    public void StopCombat()
    {
        inCombat = false;
        agent.ResetPath();
        currentTarget = null;
    }
}