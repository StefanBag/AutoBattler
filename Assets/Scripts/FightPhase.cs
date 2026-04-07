using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightPhase : MonoBehaviour
{
    [Header("References")]
    public BuyTimer buyTimer;
    public Transform enemySpawnParent;   // parent object holding enemy spawn points
    public GameObject enemyPrefab;       // default enemy prefab to spawn

    [Header("Settings")]
    public float checkInterval = 1f;     // how often to check if all enemies are dead

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool fightActive = false;

    public void StartFight()
    {
        fightActive = true;
        activeEnemies.Clear();

        SpawnEnemies();

        // Activate all player units on the bench/field
        ActivatePlayerUnits();

        StartCoroutine(CheckFightOver());
    }

    void SpawnEnemies()
    {
        if (enemySpawnParent == null || enemyPrefab == null) return;

        foreach (Transform spawnPoint in enemySpawnParent)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            activeEnemies.Add(enemy);
        }
    }

    void ActivatePlayerUnits()
    {
        // Find all player units in the scene and activate their AI
        UnitAI[] playerUnits = FindObjectsByType<UnitAI>(FindObjectsSortMode.None);
        foreach (UnitAI unit in playerUnits)
        {
            unit.StartCombat();
        }
    }

    IEnumerator CheckFightOver()
    {
        while (fightActive)
        {
            yield return new WaitForSeconds(checkInterval);

            // Remove any destroyed enemies from the list
            activeEnemies.RemoveAll(e => e == null);

            if (activeEnemies.Count == 0)
            {
                EndFight();
            }
        }
    }

    void EndFight()
    {
        fightActive = false;

        // Stop all player unit AI
        UnitAI[] playerUnits = FindObjectsByType<UnitAI>(FindObjectsSortMode.None);
        foreach (UnitAI unit in playerUnits)
        {
            unit.StopCombat();
        }

        // Go back to buy phase
        buyTimer.StartBuyPhase();
    }
}