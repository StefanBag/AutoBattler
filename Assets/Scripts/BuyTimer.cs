using UnityEngine;
using TMPro;

public class BuyTimer : MonoBehaviour
{
    [Header("Settings")]
    public float buyPhaseDuration = 30f;

    [Header("References")]
    public GameObject playerHudCanvas;
    public TextMeshProUGUI countdownText;
    public FightPhase fightPhase;        // assign in inspector

    private float timeRemaining;
    private bool isBuyPhase = false;

    

    void Update()
    {
        if (!isBuyPhase) return;

        timeRemaining -= Time.unscaledDeltaTime;

        int seconds = Mathf.CeilToInt(timeRemaining);
        countdownText.text = seconds.ToString();

        if (timeRemaining <= 0f)
        {
            StartFightPhase();
        }
    }

    void StartFightPhase()
    {
        isBuyPhase = false;
        timeRemaining = 0f;
        countdownText.text = "Fight!";

        playerHudCanvas.SetActive(false);
        Time.timeScale = 1f;

        Invoke(nameof(HideCountdown), 1f);

        // Kick off the fight
        fightPhase.StartFight();
    }

    void HideCountdown()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    public void StartBuyPhase()
    {
        isBuyPhase = true;
        timeRemaining = buyPhaseDuration;
        Time.timeScale = 0f;
        playerHudCanvas.SetActive(true);

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = buyPhaseDuration.ToString();
        }
    }
}
