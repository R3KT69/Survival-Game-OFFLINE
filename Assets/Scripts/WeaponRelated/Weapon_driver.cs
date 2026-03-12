using UnityEngine;
using System.Collections;
using UnityEngine.Animations.Rigging;
using Unity.VisualScripting;

public class Weapon_driver : MonoBehaviour // Shooting Script
{
    public PlayerInventoryManager playerInventoryManager;
    public Weapon_global currentWeapon;
    public FpsHeadController fpsHeadController;
    public GameObject Aiming_R_Hip;
    public GameObject Aiming_R_Sight;
    public RigShifting rigShifting;
    public GameObject CommmonSound;

    public Vector3[] swingKeyframes = new Vector3[]
    {
        new Vector3(-90f, -180f, 0f),   // first swing position
        new Vector3(-30f, -180f, 0f),   // second position
        new Vector3(-150f, -180f, 0f)   // final swing position before returning
    };

    //public float fireRate = 0.2f; // seconds between shots (now wepaon dependent)
    private float lastShotTime = 0f;
    private Coroutine recoilResetCoroutine;
    public int ammoX, ammoY;
    
    void Start()
    {
        rigShifting = GetComponent<RigShifting>();
    }


    void Update()
    {
        if (currentWeapon.gameObject.activeSelf == false)
        {
            //Debug.Log("Weapon Null");
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (currentWeapon.wep_data.weaponType == WEP_ANIM.Melee)
            {
                wep_meele_tryShoot();
            }
            else if (currentWeapon.wep_data.weaponType == WEP_ANIM.Gun)
            {
                wep_gun_tryShoot();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            reload_weapon();
        }
    }

    void reload_weapon()
    {
        if (currentWeapon == null) return;

        playerInventoryManager.get_ammo_index("AMMO", out ammoX, out ammoY);

        int ammoNeeded = currentWeapon.wep_data.magSize - currentWeapon.runtimeAmmo;
        if (ammoNeeded <= 0) return;

        int ammoAvailable = playerInventoryManager.inv[ammoX,ammoY].runtimeCount; 
        int ammoToLoad = Mathf.Min(ammoNeeded, ammoAvailable);
        currentWeapon.runtimeAmmo += ammoToLoad;
        playerInventoryManager.inv[ammoX,ammoY].ReduceCount(ammoToLoad);

        if (playerInventoryManager.inv[ammoX,ammoY].runtimeCount <= 0)
        {
            Destroy(playerInventoryManager.inv[ammoX,ammoY].gameObject);
            playerInventoryManager.inv[ammoX,ammoY] = null;
        }
    }

    

    void wep_meele_tryShoot()
    {
        if (Time.time >= lastShotTime + currentWeapon.wep_data.fireRate)
        {
            SwingMeele();
            lastShotTime = Time.time;
        }
    }
    
    void wep_gun_tryShoot()
    {
        if (Time.time >= lastShotTime + currentWeapon.wep_data.fireRate)
        {
            if (currentWeapon.runtimeAmmo <= 0)
            {
                CommmonSound.GetComponent<AudioSource>().Play();
                Debug.Log("Ammo finished");
                return;
            }
            ShootWeapon();
            lastShotTime = Time.time;
        }
    }
    

    void SwingMeele()
    {
        swingKeyframes[2].z = Random.Range(30f, -60f); // Random swing

        Aiming_R_Hip.GetComponent<Weapon_PrimaryIK_Anim_Manager>().SimulateSwingExp(swingKeyframes, keyframeDuration: .15f);
        AddCameraRecoil_Smooth(-5f, -5f, .1f);
    }

    void ShootWeapon()
    {
        currentWeapon.TriggerShootingEffects();

        Debug.Log($"Ammo used by {currentWeapon.name}");
        currentWeapon.runtimeAmmo -= 1;
        
        if (rigShifting.isAiming)
        {
            Aiming_R_Sight.GetComponent<Weapon_PrimaryIK_Anim_Manager>().SimulateRecoil(Random.Range(-70, -80), 0, duration: 0.5f);
            AddCameraRecoil(-1.5f, -2.5f, 0.3f);
        }
        else
        {
            Aiming_R_Hip.GetComponent<Weapon_PrimaryIK_Anim_Manager>().SimulateRecoil(Random.Range(-60, -70), 0, duration: 0.5f);
            AddCameraRecoil(-2.5f, -5f, 0.3f);
        }
    }

    public void AddCameraRecoil(float xMin = -2f, float xMax = -3f, float returnDuration = 0.2f)
    {
        // Get the head controller
        var head = Camera.main.GetComponent<FpsHeadController>();

        // Apply instant recoil kick
        head.rotationOffset *= Quaternion.Euler(Random.Range(xMin, xMax), 0f, 0f);

        // Restart the smooth return
        if (recoilResetCoroutine != null)
            StopCoroutine(recoilResetCoroutine);
        recoilResetCoroutine = StartCoroutine(ResetCameraRecoil(head, returnDuration));
    }

    public void AddCameraRecoil_Smooth(float xMin = -5f,float xMax = -5f,float kickDuration = 0.05f,float returnDuration = 0.1f)
    {
        var head = Camera.main.GetComponent<FpsHeadController>();

        float recoilX = Random.Range(xMin, xMax);
        Quaternion targetOffset = head.rotationOffset * Quaternion.Euler(recoilX, 0f, 0f);

        if (recoilResetCoroutine != null)
            StopCoroutine(recoilResetCoroutine);

        recoilResetCoroutine = StartCoroutine(
            SmoothRecoil(head, targetOffset, kickDuration, returnDuration));
    }

    private IEnumerator ResetCameraRecoil(FpsHeadController head, float duration)
    {
        Quaternion startRot = head.rotationOffset;
        Quaternion endRot = Quaternion.identity; // neutral

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // linear
            head.rotationOffset = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        head.rotationOffset = endRot;
        recoilResetCoroutine = null;
    }

    private IEnumerator SmoothRecoil(FpsHeadController head, Quaternion target, float kickDuration, float returnDuration)
    {
        Quaternion start = head.rotationOffset;

        // Smooth kick
        float t = 0f;
        while (t < kickDuration)
        {
            t += Time.deltaTime;
            float lerpFactor = t / kickDuration;
            head.rotationOffset = Quaternion.Slerp(start, target, lerpFactor);
            yield return null;
        }

        // Smooth return
        t = 0f;
        Quaternion recoilPeak = head.rotationOffset;

        while (t < returnDuration)
        {
            t += Time.deltaTime;
            float lerpFactor = t / returnDuration;
            head.rotationOffset = Quaternion.Slerp(recoilPeak, Quaternion.identity, lerpFactor);
            yield return null;
        }

        head.rotationOffset = Quaternion.identity;
    }
    
    
}
