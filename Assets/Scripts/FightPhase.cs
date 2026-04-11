using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightPhase : MonoBehaviour
{
    [Header("References")]
    public BuyTimer buyTimer;

    [Header("Settings")]
    public float checkInterval = 1f;

    private bool fightActive = false;

    public void StartFight()
    {
        fightActive = true;

        UnitAI[] allUnits = FindObjectsByType<UnitAI>(FindObjectsSortMode.None);
        foreach (UnitAI unit in allUnits)
            unit.StartCombat();

        StartCoroutine(CheckFightOver());
    }

    IEnumerator CheckFightOver()
    {
        while (fightActive)
        {
            yield return new WaitForSeconds(checkInterval);

            bool anyPlayerAlive = false;
            bool anyEnemyAlive = false;

            UnitAI[] allUnits = FindObjectsByType<UnitAI>(FindObjectsSortMode.None);
            foreach (UnitAI unit in allUnits)
            {
                if (!unit.gameObject.activeInHierarchy) continue;
                if (unit.team == UnitTeam.Player) anyPlayerAlive = true;
                if (unit.team == UnitTeam.Enemy) anyEnemyAlive = true;
            }

            if (!anyPlayerAlive || !anyEnemyAlive)
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