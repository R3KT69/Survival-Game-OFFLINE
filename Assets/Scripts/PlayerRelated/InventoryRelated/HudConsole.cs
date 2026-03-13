using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HudConsole : MonoBehaviour
{
    public List<Button> buttons;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI ammo;
    public GameObject console;
    public GameObject inventory;
    public bool isInventoryOpen;

    void Start()
    {
        ammo.text += "inf";
        isInventoryOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventory.SetActive(!inventory.activeSelf);
            isInventoryOpen = !isInventoryOpen;
            MouseLockToggle();
            
        }
    }

    void MouseLockToggle()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
