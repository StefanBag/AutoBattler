using UnityEngine;
using UnityEngine.AI;

public enum UnitTeam { Player, Enemy }
public enum UnitTrait { Sun, Demon, Ocean, Nature, Fairy }

public class UnitAI : MonoBehaviour
{
    [Header("Identity")]
    public UnitTeam team = UnitTeam.Player;
    public UnitTrait trait1 = UnitTrait.Sun;
    public UnitTrait trait2 = UnitTrait.Ocean;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;
    public float attackRange = 2f;

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
        gameObject.tag = team == UnitTeam.Player ? "Player" : "Enemy";
        StartCombat(); // ← add this if you want units to fight immediately
    }

    protected virtual void Update()
    {
        if (!inCombat) return;

        attackTimer -= Time.deltaTime;

        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = FindClosestEnemy();
        }

        if (currentTarget == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        if (distanceToTarget <= attackRange)
        {
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
            agent.SetDestination(currentTarget.position);
        }
    }

    protected virtual void Attack(Transform target)
    {
        UnitAI targetUnit = target.GetComponent<UnitAI>();
        if (targetUnit != null)
        {
            targetUnit.TakeDamage(attackDamage);
        }
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
        // Find opposite team's tag
        string enemyTag = team == UnitTeam.Player ? "Enemy" : "Player";
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