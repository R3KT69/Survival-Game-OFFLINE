using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;


public class ChangingArm : MonoBehaviour
{
    public PlayerInventoryManager playerInventoryManager;
    public List<GameObject> Hip;
    public List<GameObject> Aim;
    public Weapon_driver weapon_Driver;
    public RigShifting rigShifting;
    public List<Rig> PistolRigs;
    public List<Rig> ShotgunRigs;
    public List<Rig> ToolsRigs;
    public List<Rig> UnarmedRigs;
    
    //public List<GameObject> weapons;
    public int MeeleIndex = 2;
    public int UnarmedIndex = 3;
    public int hotbarIndex;

    void Awake()
    {
        //allRigs = new List<List<Rig>> { PistolRigs, ShotgunRigs, ToolsRigs, ToolsRigs };
    }

    void Start()
    {
        ChangeArm(0,playerInventoryManager.hotbar, WEP_Type.OneHandedGun, playerInventoryManager.hotbar[0].gameObject.GetComponent<Weapon_global>());
    }

    void Update()
    {
        int keyInt = GetNumberKeyPressed(); // 1-6
        if (keyInt != -1)
        {
            hotbarIndex = keyInt - 1; // map 1->0, 2->1
            if (hotbarIndex >= 0 && hotbarIndex < playerInventoryManager.hotbar.Length)
            {
                Item item = playerInventoryManager.hotbar[hotbarIndex];
                if (item != null && item.gameObject != null)
                {
                    Weapon_global wg = item.gameObject.GetComponent<Weapon_global>();
                    if (wg != null)
                    {
                        ChangeArm(hotbarIndex, playerInventoryManager.hotbar, item.weaponType, wg);
                    }
                }
            }
        }
    }

    public int GetNumberKeyPressed()
    {
        // Check keys 0-6
        for (int i = 1; i <= 6; i++)
        {
            // KeyCode.Alpha0 = 0 key, Alpha1 = 1 key, etc.
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                return i; // store as int
            }
        }

        return -1; // no number key pressed
    }

    // Update is called once per frame
    void ChangeArm(int indice, Item[] hotbar, WEP_Type type, Weapon_global weapon)
    {
        if (type == WEP_Type.OneHandedGun) 
        {
            GameObject EquippedObj = hotbar[indice]?.gameObject; // the object we want to equip

            for (int i = 0; i < hotbar.Length; i++)
            {
                if (hotbar[i] != null && hotbar[i].gameObject != null)
                {
                    // Only enable the selected object, leave duplicates alone
                    hotbar[i].gameObject.SetActive(hotbar[i].gameObject == EquippedObj);
                }
            }

            SetActiveRigGroup(PistolRigs);

            weapon_Driver.Aiming_R_Hip = Hip[0];
            weapon_Driver.Aiming_R_Sight = Aim[0];

            weapon_Driver.currentWeapon = weapon;

            rigShifting.rigA = PistolRigs[0];
            rigShifting.rigB = PistolRigs[1];

            
        } 
        else if (type == WEP_Type.TwoHandedGun)
        {
            GameObject EquippedObj = hotbar[indice]?.gameObject; // the object we want to equip

            for (int i = 0; i < hotbar.Length; i++)
            {
                if (hotbar[i] != null && hotbar[i].gameObject != null)
                {
                    // Only enable the selected object, leave duplicates alone
                    hotbar[i].gameObject.SetActive(hotbar[i].gameObject == EquippedObj);
                }
            }
            
            SetActiveRigGroup(ShotgunRigs);

            weapon_Driver.Aiming_R_Hip = Hip[1];
            weapon_Driver.Aiming_R_Sight = Aim[1];

            weapon_Driver.currentWeapon = weapon;

            rigShifting.rigA = ShotgunRigs[0];
            rigShifting.rigB = ShotgunRigs[1];


            
        } 
        else if (type == WEP_Type.OneHandedTool)
        {
            GameObject EquippedObj = hotbar[indice]?.gameObject; // the object we want to equip

            for (int i = 0; i < hotbar.Length; i++)
            {
                if (hotbar[i] != null && hotbar[i].gameObject != null)
                {
                    // Only enable the selected object, leave duplicates alone
                    hotbar[i].gameObject.SetActive(hotbar[i].gameObject == EquippedObj);
                }
            }

            weapon_Driver.Aiming_R_Hip = Hip[2]; 

            weapon_Driver.currentWeapon = weapon;
            
            

            SetActiveRigGroup(ToolsRigs);

            rigShifting.rigA = null;
            rigShifting.rigB = null;
        }  
        else if (type == WEP_Type.Unarmed)
        {
            GameObject EquippedObj = hotbar[indice]?.gameObject; // the object we want to equip

            for (int i = 0; i < hotbar.Length; i++)
            {
                if (hotbar[i] != null && hotbar[i].gameObject != null)
                {
                    // Only enable the selected object, leave duplicates alone
                    hotbar[i].gameObject.SetActive(hotbar[i].gameObject == EquippedObj);
                }
            }

            weapon_Driver.Aiming_R_Hip = Hip[2];

            SetActiveRigGroup(UnarmedRigs);

            rigShifting.rigA = null;
            rigShifting.rigB = null;
        }
        
        
    }


    void SetActiveRigGroup(List<Rig> activeGroup, float weight = 1f)
    {
        // Disable all rigs first
        foreach (var group in new List<List<Rig>> { PistolRigs, ShotgunRigs, ToolsRigs, UnarmedRigs })
        {
            foreach (var rig in group)
            {
                rig.weight = 0f;
            }
        }

        // Enable only the active group
        foreach (var rig in activeGroup)
        {
            rig.weight = weight;
        }
    }
}
