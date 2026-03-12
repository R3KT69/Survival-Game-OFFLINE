using UnityEngine;
using System.Collections;
using UnityEngine.Animations.Rigging;
using System;

public class Weapon_global : MonoBehaviour
{
    [Header("Position Settings")]
    public Transform shootingPoint;
    public ParticleSystem wep_flash;
    public GameObject wep_light;
    public Animator wep_animator;
    public AudioSource wep_audioSource;
    public Weapon_attr wep_data;
    public int runtimeAmmo;
    
    public String type;
    
    public float lightDuration = 0.1f;
    private Coroutine lightRoutine;

    [Header("Transform Settings")]
    public Vector3 position;
    public Vector3 rotation;
    public float scale;

    public void TriggerShootingEffects()
    {
       // Debug.Log(wep_data.weaponName);
        
        if (wep_flash != null)
        {
            wep_flash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            wep_flash.Play();
        }

        if (wep_light != null)
        {
            wep_light.SetActive(true);

            // Restart light toggle coroutine
            if (lightRoutine != null)
                StopCoroutine(lightRoutine);

            lightRoutine = StartCoroutine(AutoDisableLight());
        }

        if (wep_animator != null)
        {
            wep_animator.Play("USP", -1, 0f);
        }

        if (wep_audioSource != null)
        {
            wep_audioSource.Play();
        }

    }
    
    private IEnumerator AutoDisableLight()
    {
        yield return new WaitForSeconds(lightDuration);
        if (wep_light != null)
        {
            wep_light.SetActive(false);
        }
    }
}
