// FightPhase.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AI;

public class FightPhase : MonoBehaviour
{
    [Header("References")]
    public BuyTimer buyTimer;
    public TextMeshProUGUI resultText;

    [Header("Settings")]
    public float checkInterval = 1f;
    public float resultDisplayTime = 3f;

    private bool fightActive = false;
    private Coroutine fightRoutine;
    private List<BenchSlot> benchSlots = new List<BenchSlot>();
    private Transform unitsFolder;

    void Start()
    {
        GameObject bench = GameObject.Find("FriendlyBench");
        if (bench != null)
        {
            foreach (Transform child in bench.transform)
            {
                BenchSlot slot = child.GetComponent<BenchSlot>();
                if (slot != null)
                    benchSlots.Add(slot);
            }
        }

        GameObject unitsObj = GameObject.Find("Units");
        if (unitsObj != null)
            unitsFolder = unitsObj.transform;
    }

    public void StartFight()
    {
        if (fightActive) return;
        fightActive = true;

        UnitAI[] allUnits = FindObjectsByType<UnitAI>(FindObjectsSortMode.None);
        List<UnitAI> boardUnits = new List<UnitAI>();

        foreach (UnitAI unit in allUnits)
        {
            if (unit == null) continue;
            if (IsShopTemplate(unit.gameObject)) continue;

            if (IsOnBench(unit.gameObject))
            {
                SetBenchUnitState(unit, false);
                unit.StopCombat();
            }
            else
            {
                unit.enabled = true;

                NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
                if (agent != null) agent.enabled = true;

                Rigidbody rb = unit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                boardUnits.Add(unit);
                unit.StartCombat();
            }
        }

        // Evaluate traits before combat begins
        if (TraitSystem.Instance != null)
            TraitSystem.Instance.EvaluateTraits(boardUnits);

        if (fightRoutine != null)
            StopCoroutine(fightRoutine);

        fightRoutine = StartCoroutine(CheckFightOver());
    }

    IEnumerator CheckFightOver()
    {
        while (fightActive)
        {
            yield return new WaitForSeconds(checkInterval);

            bool anyPlayerAlive = false;
            bool anyEnemyAlive = false;
            List<string> playerNames = new List<string>();
            List<string> enemyNames = new List<string>();

            UnitAI[] allUnits = FindObjectsByType<UnitAI>(FindObjectsSortMode.None);

            foreach (UnitAI unit in allUnits)
            {
                if (unit == null) continue;
                if (!unit.gameObject.activeInHierarchy) continue;
                if (IsShopTemplate(unit.gameObject)) continue;
                if (unit.isOnBench || IsOnBench(unit.gameObject)) continue;

                if (unit.team == UnitTeam.Player)
                {
                    anyPlayerAlive = true;
                    playerNames.Add(unit.name + " [parent=" + (unit.transform.parent != null ? unit.transform.parent.name : "<root>") + "]");
                }
                else if (unit.team == UnitTeam.Enemy)
                {
                    anyEnemyAlive = true;
                    enemyNames.Add(unit.name + " [parent=" + (unit.transform.parent != null ? unit.transform.parent.name : "<root>") + "]");
                }
            }

            Debug.Log("Players alive: " + anyPlayerAlive + " | Enemies alive: " + anyEnemyAlive);

            if (playerNames.Count > 0)
            {
                Debug.Log("PLAYER UNITS COUNTED:");
                foreach (string name in playerNames)
                    Debug.Log(" - " + name);
            }

            if (enemyNames.Count > 0)
            {
                Debug.Log("ENEMY UNITS COUNTED:");
                foreach (string name in enemyNames)
                    Debug.Log(" - " + name);
            }

            if (!anyPlayerAlive || !anyEnemyAlive)
            {
                EndFight(anyPlayerAlive);
                yield break;
            }
        }
    }

    void EndFight(bool playerWon)
    {
        if (!fightActive) return;
        fightActive = false;

        if (fightRoutine != null)
        {
            StopCoroutine(fightRoutine);
            fightRoutine = null;
        }

        UnitAI[] allUnits = FindObjectsByType<UnitAI>(FindObjectsSortMode.None);

        foreach (UnitAI unit in allUnits)
        {
            if (unit == null) continue;
            if (IsShopTemplate(unit.gameObject)) continue;

            unit.StopCombat();

            if (IsOnBench(unit.gameObject))
                SetBenchUnitState(unit, true);
        }

        string message = playerWon ? "You Win!" : "You Lose!";

        if (resultText != null)
        {
            Transform t = resultText.transform;
            while (t != null)
            {
                if (!t.gameObject.activeSelf) t.gameObject.SetActive(true);
                t = t.parent;
            }

            resultText.text = message;
            resultText.color = playerWon ? Color.green : Color.red;
        }
        else
        {
            Debug.LogWarning("[FightPhase] resultText is not assigned — assign it in the Inspector to show '" + message + "' on screen.");
        }

        StartCoroutine(ReturnToLevelSelect(playerWon));
    }

    bool IsOnBench(GameObject obj)
    {
        foreach (BenchSlot slot in benchSlots)
        {
            if (slot != null && slot.unit == obj)
                return true;
        }
        return false;
    }

    bool IsShopTemplate(GameObject obj)
    {
        if (obj == null || unitsFolder == null) return false;
        return obj.transform.IsChildOf(unitsFolder);
    }

    void SetBenchUnitState(UnitAI unit, bool enabledState)
    {
        if (unit == null) return;

        NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
        Rigidbody rb = unit.GetComponent<Rigidbody>();

        if (!enabledState)
        {
            unit.StopCombat();

            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
                agent.ResetPath();

            if (agent != null) agent.enabled = false;

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            unit.enabled = false;
        }
        else
        {
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            if (agent != null) agent.enabled = true;
            unit.enabled = true;
        }
    }

    IEnumerator ReturnToLevelSelect(bool playerWon)
    {
        yield return new WaitForSecondsRealtime(resultDisplayTime);
        Time.timeScale = 1f;

        if (playerWon)
        {
            Character character = FindFirstObjectByType<Character>();
            int currentLevel = character != null
                ? character.level
                : 0;

            Character.SetCurrentLevel(currentLevel + 1);
        }

        SceneManager.LoadScene("LevelSelectScene");
    }
}
