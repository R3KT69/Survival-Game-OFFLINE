using System.Collections.Generic;
using UnityEngine;

public enum WEP_ANIM
{
    Gun,
    Melee
}

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon/WepAttr")]
public class Weapon_attr : ScriptableObject
{
    public string weaponName;
    public WEP_ANIM weaponType;
    public float damage;
    public float fireRate;
    public int magSize;
    public GameObject weaponPrefab;
}