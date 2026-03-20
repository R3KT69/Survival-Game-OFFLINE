using UnityEngine;

public class UI_CrateManager : MonoBehaviour
{
    public CrateInventoryManager selected_crate;
    public Camera mainCamera;
    public HudConsole Hud;

    void Start()
    {
        Hud = GameObject.Find("HUD").GetComponent<HudConsole>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetCrate();
            Hud.TriggerInvOpen();
        }
    }


    public void GetCrate()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            if (hit.collider.tag == "CRATE")
            {
                selected_crate = hit.collider.gameObject.GetComponent<CrateInventoryManager>();
                Debug.Log($"Crate found. ID: {selected_crate.name}");
                
            }
        }
    }
    
    
}
