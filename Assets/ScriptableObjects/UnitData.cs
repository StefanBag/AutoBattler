using UnityEditor;
using UnityEngine;

public enum UnitTrait
{
    Sun,
    Demon,
    Ocean,
    Nature,
    Fairy
}

[CreateAssetMenu(fileName = "NewItem", menuName = "MyGame/Unit")]
public class UnitData : ScriptableObject
{
    public string unit_name;
    public int damage;
    public int range;
    public int cost;
    public int cooldown;
    public int health;
    public GameObject model;
    public UnitTrait trait1;
    public UnitTrait trait2;
}
