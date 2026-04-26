// TraitSystem.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TraitBonus
{
    public UnitTrait trait;
    public int requiredCount = 3;
    public int bonusHealth;
    public int bonusDamage;
}

public class TraitSystem : MonoBehaviour
{
    public static TraitSystem Instance { get; private set; }

    [Header("Define your trait bonuses here in the Inspector")]
    public List<TraitBonus> traitBonuses = new List<TraitBonus>();

    [Header("UI")]
    public Slider traitProgressSlider;

    private Image traitProgressFillImage;

    private HashSet<UnitTrait> activeTraits = new HashSet<UnitTrait>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void EvaluateTraits(List<UnitAI> boardUnits)
    {
        Dictionary<UnitTrait, List<UnitAI>> traitGroups = new Dictionary<UnitTrait, List<UnitAI>>();

        foreach (UnitAI unit in boardUnits)
        {
            if (unit == null) continue;
            if (unit.team != UnitTeam.Player) continue;

            AddToGroup(traitGroups, unit.unit_data.trait1, unit);
            if (unit.unit_data.trait2 != unit.unit_data.trait1)
                AddToGroup(traitGroups, unit.unit_data.trait2, unit);
        }

        // Find dominant trait (most units, whether or not threshold is met)
        UnitTrait? dominantTrait = null;
        int dominantCount = 0;

        foreach (var kvp in traitGroups)
        {
            TraitBonus bonus = GetBonus(kvp.Key);
            if (bonus == null) continue;

            // Track largest group regardless of threshold
            if (kvp.Value.Count > dominantCount)
            {
                dominantCount = kvp.Value.Count;
                dominantTrait = kvp.Key;
            }
        }

        // Always update the bar if we have any trait on the board
        if (dominantTrait.HasValue)
        {
            TraitBonus bonus = GetBonus(dominantTrait.Value);
            if (bonus != null)
            {
                float fill = bonus.requiredCount > 0 ? (float)dominantCount / bonus.requiredCount : 0f;
                Color traitColor = GetTraitColor(dominantTrait.Value);
                UpdateTraitProgressSlider(fill, traitColor);
                PowerBar.Instance?.UpdateBar(fill, traitColor);
            }
        }
        else
        {
            UpdateTraitProgressSlider(0f, Color.grey);
            PowerBar.Instance?.ResetBar();
        }

        // Only apply buffs if threshold is met
        foreach (UnitAI unit in boardUnits)
            unit?.RemoveTraitBuff();

        activeTraits.Clear();

        UnitTrait? buffTrait = null;
        int buffCount = 0;

        foreach (var kvp in traitGroups)
        {
            TraitBonus bonus = GetBonus(kvp.Key);
            if (bonus == null) continue;

            if (kvp.Value.Count >= bonus.requiredCount && kvp.Value.Count > buffCount)
            {
                buffCount = kvp.Value.Count;
                buffTrait = kvp.Key;
            }
        }

        if (buffTrait.HasValue)
        {
            TraitBonus bonus = GetBonus(buffTrait.Value);
            List<UnitAI> qualifyingUnits = traitGroups[buffTrait.Value];

            foreach (UnitAI unit in qualifyingUnits)
                unit?.ApplyTraitBuff(bonus.bonusHealth, bonus.bonusDamage);

            activeTraits.Add(buffTrait.Value);
            Debug.Log($"[TraitSystem] Active: {buffTrait.Value} ({buffCount} units) +{bonus.bonusHealth} HP / +{bonus.bonusDamage} DMG");
        }
        else
        {
            Debug.Log("[TraitSystem] No trait threshold met.");
        }
    }
    // Add this to TraitSystem.cs
    public void RefreshTraits()
    {
        // Scan all active board units live during buy phase
        List<UnitAI> boardUnits = new List<UnitAI>();
        UnitAI[] allUnits = FindObjectsByType<UnitAI>(FindObjectsSortMode.None);

        foreach (UnitAI unit in allUnits)
        {
            if (unit == null) continue;
            if (!unit.gameObject.activeInHierarchy) continue;
            if (unit.isOnBench) continue; // skip bench units
            if (unit.team != UnitTeam.Player) continue; // only player units count toward player trait buffs
            if (IsShopTemplate(unit.gameObject)) continue;

            boardUnits.Add(unit);
        }

        EvaluateTraits(boardUnits);
    }

    private bool IsShopTemplate(GameObject obj)
    {
        // Find units folder to exclude shop templates
        GameObject unitsObj = GameObject.Find("Units");
        if (unitsObj == null) return false;
        return obj.transform.IsChildOf(unitsObj.transform);
    }

    private void AddToGroup(Dictionary<UnitTrait, List<UnitAI>> groups, UnitTrait trait, UnitAI unit)
    {
        if (!groups.ContainsKey(trait))
            groups[trait] = new List<UnitAI>();
        groups[trait].Add(unit);
    }

    private void UpdateTraitProgressSlider(float fillAmount, Color fillColor)
    {
        if (traitProgressSlider == null) return;

        traitProgressSlider.value = Mathf.Clamp01(fillAmount);

        if (traitProgressFillImage == null && traitProgressSlider.fillRect != null)
            traitProgressFillImage = traitProgressSlider.fillRect.GetComponent<Image>();

        if (traitProgressFillImage != null)
            traitProgressFillImage.color = fillColor;
    }

    private TraitBonus GetBonus(UnitTrait trait)
    {
        return traitBonuses.Find(b => b.trait == trait);
    }

    private Color GetTraitColor(UnitTrait trait)
    {
        switch (trait)
        {
            case UnitTrait.Sun:
                return Color.yellow;
            case UnitTrait.Demon:
                return Color.red;
            case UnitTrait.Ocean:
                return Color.blue;
            case UnitTrait.Nature:
                return Color.green;
            case UnitTrait.Fairy:
                return new Color(1f, 0.4f, 0.8f);
            default:
                return Color.white;
        }
    }
}
