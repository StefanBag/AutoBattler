// PowerBar.cs
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    public static PowerBar Instance { get; private set; }

    [Header("References")]
    public Slider slider;
    public Image fillImage;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Find fill image automatically if not assigned
        if (fillImage == null)
            fillImage = transform.Find("Fill Area/Fill")?.GetComponent<Image>();

        if (slider == null)
            slider = GetComponent<Slider>();
    }

    public void UpdateBar(float fillAmount, Color traitColor)
    {
        if (slider != null)
            slider.value = Mathf.Clamp01(fillAmount);

        if (fillImage != null)
            fillImage.color = traitColor;
    }

    public void ResetBar()
    {
        if (slider != null)
            slider.value = 0f;

        if (fillImage != null)
            fillImage.color = Color.grey;
    }
}