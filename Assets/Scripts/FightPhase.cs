using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class FightPhase : MonoBehaviour
{
    [Header("References")]
    public BuyTimer buyTimer;
    public TextMeshProUGUI resultText;

    [Header("Settings")]
    public float checkInterval = 1f;
    public float resultDisplayTime = 3f;

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
                EndFight(anyPlayerAlive);
        }
    }

    void EndFight(bool playerWon)
    {
        fightActive = false;

        UnitAI[] allUnits = FindObjectsByType<UnitAI>(FindObjectsSortMode.None);
        foreach (UnitAI unit in allUnits)
            unit.StopCombat();

        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = playerWon ? "You Win!" : "You Lose!";
        }

        StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSecondsRealtime(resultDisplayTime);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}