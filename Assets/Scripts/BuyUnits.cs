using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyUnits : MonoBehaviour
{
    List<BenchSlot> bench_slots = new List<BenchSlot>();
    public UnitData unit;
    public AudioSource audioSource;
    private TextMeshProUGUI unitNameText;
    private TextMeshProUGUI priceText;
    private Graphic classSlot1;
    private Graphic classSlot2;
    public Character character;

    void Start()
    {
        character = FindFirstObjectByType<Character>();
        unitNameText = transform.Find("UnitName").GetComponent<TextMeshProUGUI>();
        priceText = transform.Find("Price").GetComponent<TextMeshProUGUI>();
        classSlot1 = transform.Find("ClassSlot1").GetComponent<Graphic>();
        classSlot2 = transform.Find("ClassSlot2").GetComponent<Graphic>();

        GameObject bench = GameObject.Find("FriendlyBench");
        if (bench != null)
        {
            foreach (Transform slot in bench.transform)
            {
                BenchSlot benchSlot = slot.GetComponent<BenchSlot>();
                if (benchSlot != null)
                    bench_slots.Add(benchSlot);
            }
        }

        Button_Setup();
    }

    public void Button_Setup()
    {
        AssignRandomUnit();

        if (unit != null)
        {
            unitNameText.text = unit.unit_name;
            priceText.text = $"${unit.cost}";
            SetClassSlotColors();
        }
    }

    void Update()
    {
        if (character != null)
            Debug.Log(character.money);
    }

    public void BuyUnit()
    {
        if (unit == null || character == null) return;

        BenchSlot available_slot = CheckAvailableSlots();
        Debug.Log(available_slot);

        if (available_slot != null && character.money >= unit.cost)
        {
            GameObject spawnedUnit = Instantiate(unit.model);

            UnitAI ai = spawnedUnit.GetComponent<UnitAI>();
            if (ai != null)
                ai.isOnBench = true;

            available_slot.AddUnit(spawnedUnit);

            if (audioSource != null)
                audioSource.Play();

            character.money -= unit.cost;
            unit = null;
            gameObject.SetActive(false);
        }
    }

    void AssignRandomUnit()
    {
        GameObject unitsFolder = GameObject.Find("Units");
        if (unitsFolder == null) return;

        List<GameObject> unitList = new List<GameObject>();

        foreach (Transform child in unitsFolder.transform)
            unitList.Add(child.gameObject);

        if (unitList.Count > 0)
        {
            GameObject picked = unitList[Random.Range(0, unitList.Count)];
            UnitAI ai = picked.GetComponent<UnitAI>();

            if (ai != null)
                unit = ai.unit_data;
        }
    }

    void SetClassSlotColors()
    {
        if (unit == null) return;

        SetClassSlotColor(classSlot1, unit.trait1);
        SetClassSlotColor(classSlot2, unit.trait2);
    }

    void SetClassSlotColor(Graphic classSlot, UnitTrait trait)
    {
        if (classSlot == null) return;

        classSlot.color = GetTraitColor(trait);
    }

    Color GetTraitColor(UnitTrait trait)
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

    BenchSlot CheckAvailableSlots()
    {
        foreach (BenchSlot slot in bench_slots)
        {
            if (slot != null && slot.unit == null)
                return slot;
        }

        return null;
    }
}
