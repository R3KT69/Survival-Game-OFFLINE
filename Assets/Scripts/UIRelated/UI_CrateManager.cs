using UnityEngine;

public class UI_CrateManager : MonoBehaviour
{
    public CrateInventoryManager selected_crate;
    public Camera mainCamera;
    public HudConsole Hud;
    public GameObject Loot;
    public bool inRange;

    void Start()
    {
        Hud = GameObject.Find("HUD").GetComponent<HudConsole>();
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
        
        inRange = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CheckCrateRange())
        {
            Hud.TriggerInvOpen();
            inRange = true;
        } 
        
        Loot.SetActive(inRange);
    }


    public bool CheckCrateRange()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
           if (hit.collider.tag == "CRATE")
            {
                selected_crate = hit.collider.gameObject.GetComponent<CrateInventoryManager>();
                Debug.Log($"Crate found. ID: {selected_crate.name}");
                return true;
            }
        }
        return false;
    }

    
    
    
}
