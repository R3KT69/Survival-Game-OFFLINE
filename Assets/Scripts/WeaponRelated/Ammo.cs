using UnityEngine;

public enum AmmoType
{
    PISTOL_AMMO, RIFLE_AMMO, SHOTGUN_AMMO
}

public class Ammo : MonoBehaviour
{
    //public AmmoType ammoType;
    public int AmmoCount;
    

    void Start()
    {
        //SetAmmo(gameObject.GetComponent<Item>().Count);
    } 

    public void SetAmmo(int ammo)
    {
        AmmoCount = ammo;
    }
}
