using UnityEngine;
using TMPro;

public class BuyTimer : MonoBehaviour
{
    [Header("Settings")]
    public float buyPhaseDuration = 30f;

    [Header("References")]
    public GameObject playerHudCanvas;
    public TextMeshProUGUI countdownText;
    public FightPhase fightPhase;
    public Character character;

    private float timeRemaining;
    private bool isBuyPhase = false;

    void Update()
    {
        if (!isBuyPhase) return;

        timeRemaining -= Time.unscaledDeltaTime;
        countdownText.text = Mathf.CeilToInt(timeRemaining).ToString();

        if (timeRemaining <= 0f)
            StartFightPhase();
    }

    void StartFightPhase()
    {
        character.active = false;
        isBuyPhase = false;
        timeRemaining = 0f;
        countdownText.text = "Fight!";

        playerHudCanvas.SetActive(false);
        Time.timeScale = 1f;

        Invoke(nameof(HideCountdown), 1f);
        fightPhase.StartFight();
    }

    void HideCountdown()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    public void StartBuyPhase()
    {
        character.active = true; // ← re-enable player control
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