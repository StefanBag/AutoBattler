using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyUnits : MonoBehaviour
{
    List<BenchSlot> bench_slots = new List<BenchSlot>();
    public UnitData unit;
    public AudioSource audioSource;
    private TextMeshProUGUI unitNameText;
    private TextMeshProUGUI priceText;
    public Character character;

    void Start()
    {
        character = FindFirstObjectByType<Character>();
        unitNameText = transform.Find("UnitName").GetComponent<TextMeshProUGUI>();
        priceText = transform.Find("Price").GetComponent<TextMeshProUGUI>();

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