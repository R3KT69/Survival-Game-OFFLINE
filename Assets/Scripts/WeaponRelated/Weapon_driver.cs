using UnityEngine;
using System.Collections;
using UnityEngine.Animations.Rigging;
using Unity.VisualScripting;

public class Weapon_driver : MonoBehaviour // Shooting Script
{
    public PlayerInventoryManager playerInventoryManager;
    public ChangingArm changingArm;
    public Weapon_global currentWeapon;
    public FpsHeadController fpsHeadController;
    public GameObject Aiming_R_Hip;
    public GameObject Aiming_R_Sight;
    public RigShifting rigShifting;
    public GameObject CommmonSound;
    public GameObject bulletObject;
    public bool canShoot;
    public bool isShooting;

    public Vector3 reload_offset_twohand;
    public Vector3 reload_offset_onehand;

    public MultiRotationConstraint onehandedIK;
    public MultiRotationConstraint twohandedIK;

    

    public Vector3[] swingKeyframes = new Vector3[]
    {
        new Vector3(-90f, -180f, 0f),   // first swing position
        new Vector3(-30f, -180f, 0f),   // second position
        new Vector3(-150f, -180f, 0f)   // final swing position before returning
    };

    //public float fireRate = 0.2f; // seconds between shots (now wepaon dependent)
    private float lastShotTime = 0f;
    private Coroutine recoilResetCoroutine;
    
    
    void Start()
    {
        rigShifting = GetComponent<RigShifting>();
    }

    private IEnumerator ShootingLockCoroutine(float duration)
    {
        isShooting = true;

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Reset
        isShooting = false;
    }


    void Update()
    {
        

        if (currentWeapon == null)
        {
            //Debug.Log("Current weapon not found/destroyed/dropped");
            return;
        }

        if (currentWeapon.gameObject.activeSelf == false)
        {
            //Debug.Log("Weapon Null");
            return;
        }

        if (canShoot && !changingArm.isUnarmed)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                if (currentWeapon.wep_data.weaponType == WEP_ANIM.Melee)
                {
                    wep_meele_tryShoot();
                }
                else if (currentWeapon.wep_data.weaponType == WEP_ANIM.Gun)
                {
                    isShooting = true;
                    wep_gun_tryShoot();
                    StartCoroutine(ShootingLockCoroutine(1f));
                }

            }

            // Automatic wep
            if (Input.GetMouseButton(0))
            {
                if (currentWeapon.wep_data.weaponType == WEP_ANIM.GunAuto) 
                {
                    isShooting = true;
                    wep_gun_tryShoot();
                    StartCoroutine(ShootingLockCoroutine(1f));
                }
                    
            }

            // Normal Shotgun
            if (Input.GetMouseButtonDown(0))
            {
                if (currentWeapon.wep_data.weaponType == WEP_ANIM.GunScatter)
                {
                    isShooting = true;

                    if (Time.time >= lastShotTime + currentWeapon.wep_data.fireRate)
                    {
                        ShootWeapon_Scatter(pelletCount: 8, spreadAngle: 5f); 
                        lastShotTime = Time.time;
                    }

                    StartCoroutine(ShootingLockCoroutine(1f));
                }
            }

            // Auto Shotgun
            if (Input.GetMouseButton(0))
            {
                if (currentWeapon.wep_data.weaponType == WEP_ANIM.GunAutoScatter)
                {
                    isShooting = true;

                    if (Time.time >= lastShotTime + currentWeapon.wep_data.fireRate)
                    {
                        ShootWeapon_Scatter(pelletCount: 6, spreadAngle: 5f);
                        lastShotTime = Time.time;
                    }

                    StartCoroutine(ShootingLockCoroutine(1f));
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isReloading)
            {
                Debug.Log("Cannot reload: already reloading");
                return;
            }

            if (rigShifting.isAiming)
            {
                Debug.Log("Cannot reload: currently aiming");
                return;
            }

            if (isShooting)
            {
                Debug.Log("Cannot reload: currently shooting");
                return;
            }

            if (!playerInventoryManager.check_item("AMMO"))
            {
                Debug.Log("Cannot reload: no ammo in inventory");
                return;
            }
            
            canShoot = false;
            StartCoroutine(ReloadWithDelay(currentWeapon.wep_data.reloadTime));
        }
    }

    private bool isReloading = false;
    IEnumerator ReloadWithDelay(float delay)
    {
        isReloading = true;
        canShoot = false;

        Vector3 startOneHand = onehandedIK.data.offset;
        Vector3 startTwoHand = twohandedIK.data.offset;

        float t = 0f;
        float transitionTime = 0.3f; // duration to reach reload offset

        // Lerp to reload offsets
        while (t < 1f)
        {
            t += Time.deltaTime / transitionTime;
            onehandedIK.data.offset = Vector3.Lerp(startOneHand, reload_offset_onehand, t);
            twohandedIK.data.offset = Vector3.Lerp(startTwoHand, reload_offset_twohand, t);
            yield return null; // wait for next frame
        }

        // Wait for reload duration minus transition time
        yield return new WaitForSeconds(delay - transitionTime * 2f);

        // Perform actual reload
        reload_weapon();

        // Lerp back to default offsets
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / transitionTime;
            onehandedIK.data.offset = Vector3.Lerp(reload_offset_onehand, startOneHand, t);
            twohandedIK.data.offset = Vector3.Lerp(reload_offset_twohand, startTwoHand, t);
            yield return null;
        }

        isReloading = false;
        canShoot = true;
    }

    private int ammoX, ammoY;
    void reload_weapon()
    {
        if (currentWeapon == null) return;

        if (!playerInventoryManager.get_item_index("AMMO", out ammoX, out ammoY)) return;

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
                EmptyMagSfx();
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

        if (bulletObject != null)
        {
            GameObject bullet = Instantiate(
                bulletObject, 
                currentWeapon.shootingPoint.position, 
                currentWeapon.shootingPoint.rotation
            );

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = currentWeapon.shootingPoint.forward * currentWeapon.wep_data.bulletVel;
                rb.useGravity = true;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }

            
            Destroy(bullet, 5f);
        }
        
        // Recoil usage
        // SimulateRecoil(x: vertical recoil [-75, -90] (lower is more recoil) , z:horizontal sway [-1, 1] (higher is more recoil), y : -180 default, duration: 0.2 default
        if (rigShifting.isAiming)
        {
            Aiming_R_Sight.GetComponent<Weapon_PrimaryIK_Anim_Manager>().SimulateRecoil(Random.Range(-80, -80), Random.Range(-1.0f, 1.0f), duration: 0.5f);
            AddCameraRecoil(-1.5f, -2.5f, 0.3f);
        }
        else
        {
            Aiming_R_Hip.GetComponent<Weapon_PrimaryIK_Anim_Manager>().SimulateRecoil(Random.Range(-75, -78), Random.Range(-2.5f, 2.5f), duration: 0.5f);
            AddCameraRecoil(-2.5f, -5f, 0.3f);
        }
    }

    void EmptyMagSfx()
    {
        AudioClip audio = CommmonSound.GetComponent<AudioCollection>().soundeffects[0];
        CommmonSound.GetComponent<AudioSource>().clip = audio;
        CommmonSound.GetComponent<AudioSource>().Play();
    }

    void ShootWeapon_Scatter(int pelletCount, float spreadAngle)
    {
        if (currentWeapon.runtimeAmmo <= 0)
        {
            EmptyMagSfx();
            Debug.Log("Ammo finished");
            return;
        }

        currentWeapon.TriggerShootingEffects();
        currentWeapon.runtimeAmmo--;

        for (int i = 0; i < pelletCount; i++)
        {
            if (bulletObject == null) continue;

            Quaternion baseRot = currentWeapon.shootingPoint.rotation;

            // Random spread for each pellet
            float spreadX = Random.Range(-spreadAngle, spreadAngle);
            float spreadY = Random.Range(-spreadAngle, spreadAngle);
            Quaternion pelletRot = baseRot * Quaternion.Euler(spreadX, spreadY, 0f);

            GameObject pellet = Instantiate(
                bulletObject,
                currentWeapon.shootingPoint.position,
                pelletRot
            );

            Rigidbody rb = pellet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = pelletRot * Vector3.forward * currentWeapon.wep_data.bulletVel;
                rb.useGravity = true;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }

            Destroy(pellet, 5f);
        }

        // Recoil
        if (rigShifting.isAiming)
        {
            Aiming_R_Sight.GetComponent<Weapon_PrimaryIK_Anim_Manager>()
                .SimulateRecoil(Random.Range(-80, -80), 0, duration: 0.5f);
            AddCameraRecoil(-1.5f, -2.5f, 0.3f);
        }
        else
        {
            Aiming_R_Hip.GetComponent<Weapon_PrimaryIK_Anim_Manager>()
                .SimulateRecoil(Random.Range(-75, -78), Random.Range(-2.5f, 2.5f), duration: 0.5f);
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
