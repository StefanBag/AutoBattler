using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightPhase : MonoBehaviour
{
    [Header("References")]
    public BuyTimer buyTimer;
    public Transform enemySpawnParent;
    public GameObject enemyPrefab;

    [Header("Settings")]
    public float checkInterval = 1f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool fightActive = false;

    public void StartFight()
    {
        fightActive = true;
        activeEnemies.Clear();

        SpawnEnemies();
        ActivatePlayerUnits();

        StartCoroutine(CheckFightOver());
    }

    void SpawnEnemies()
    {
        if (enemySpawnParent == null || enemyPrefab == null) return;

        foreach (Transform spawnPoint in enemySpawnParent)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            UnitAI ai = enemy.GetComponent<UnitAI>();
            if (ai != null) ai.StartCombat();
            activeEnemies.Add(enemy);
        }
    }

    void ActivatePlayerUnits()
    {
        UnitAI[] allUnits = FindObjectsByType<UnitAI>(FindObjectsSortMode.None);
        foreach (UnitAI unit in allUnits)
        {
            if (unit.team == UnitTeam.Player)
                unit.StartCombat();
        }
    }

    IEnumerator CheckFightOver()
    {
        while (fightActive)
        {
            yield return new WaitForSeconds(checkInterval);
            activeEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);

            if (activeEnemies.Count == 0)
                EndFight();
        }
    }

    void EndFight()
    {
        fightActive = false;

        UnitAI[] allUnits = FindObjectsByType<UnitAI>(FindObjectsSortMode.None);
        foreach (UnitAI unit in allUnits)
            unit.StopCombat();

        buyTimer.StartBuyPhase();
    }
}