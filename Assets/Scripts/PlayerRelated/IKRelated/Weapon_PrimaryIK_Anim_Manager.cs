using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public class Weapon_PrimaryIK_Anim_Manager : MonoBehaviour
{
    private MultiRotationConstraint multiRotConstraint;
    public Vector3 rotationOffset = new Vector3(-90f, -180f, 0f); // default resting offset
    private Coroutine recoilCoroutine;

    void Start()
    {
        multiRotConstraint = GetComponent<MultiRotationConstraint>();
        multiRotConstraint.data.offset = rotationOffset;
    }

    public void SimulateRecoil(float x, float z, float y = -180f, float duration = 0.2f)
    {
        Vector3 recoilOffset = new Vector3(x, y, z);

        if (recoilCoroutine != null)
            StopCoroutine(recoilCoroutine);

        recoilCoroutine = StartCoroutine(RecoilRoutine(recoilOffset, duration));
    }

    public void SimulateSwing(float x, float z, float y = -180f, float duration = 0.2f)
    {
        Vector3 recoilOffset = new Vector3(x, y, z);

        if (recoilCoroutine != null)
            StopCoroutine(recoilCoroutine);

        recoilCoroutine = StartCoroutine(MeleeSwingRoutine(recoilOffset, duration));
    }

    private IEnumerator RecoilRoutine(Vector3 targetOffset, float duration)
    {
        // Apply immediate recoil
        multiRotConstraint.data.offset = targetOffset;

        // Smoothly return to default offset
        float elapsed = 0f;
        Vector3 startOffset = targetOffset;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            multiRotConstraint.data.offset = Vector3.Lerp(startOffset, rotationOffset, t);
            yield return null;
        }

        // Ensure exact final offset
        multiRotConstraint.data.offset = rotationOffset;
        recoilCoroutine = null;
    }

    private IEnumerator MeleeSwingRoutine(Vector3 swingOffset, float duration)
    {
        if (multiRotConstraint == null) yield break;

        Vector3 startOffset = multiRotConstraint.data.offset; // current offset
        Vector3 midOffset = swingOffset; // the target swing offset

        float halfDuration = duration / 2f;
        float elapsed = 0f;

        // Swing toward the target offset
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            multiRotConstraint.data.offset = Vector3.Lerp(startOffset, midOffset, t);
            yield return null;
        }

        // Ensure we hit the exact swing offset
        multiRotConstraint.data.offset = midOffset;

        // Reset back to original offset
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            multiRotConstraint.data.offset = Vector3.Lerp(midOffset, rotationOffset, t);
            yield return null;
        }

        // Ensure exact final offset
        multiRotConstraint.data.offset = rotationOffset;
    }

    public void SimulateSwingExp(Vector3[] keyframes, float keyframeDuration = 0.1f)
    {
        if (recoilCoroutine != null)
            StopCoroutine(recoilCoroutine);

        recoilCoroutine = StartCoroutine(MeleeSwingRoutineExp(keyframes, keyframeDuration));
    }

    private IEnumerator MeleeSwingRoutineExp(Vector3[] keyframes, float keyframeDuration)
    {
        if (multiRotConstraint == null || keyframes.Length == 0) yield break;

        // Snap instantly to first keyframe
        multiRotConstraint.data.offset = keyframes[0];
        Vector3 startOffset = keyframes[0];

        // Loop through remaining keyframes (if any)
        for (int i = 1; i < keyframes.Length; i++)
        {
            Vector3 targetOffset = keyframes[i];
            float elapsed = 0f;

            while (elapsed < keyframeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / keyframeDuration;
                multiRotConstraint.data.offset = Vector3.Lerp(startOffset, targetOffset, t);
                yield return null;
            }

            // Snap exactly to this keyframe
            multiRotConstraint.data.offset = targetOffset;
            startOffset = targetOffset; // next lerp starts from here
        }

        // Smoothly return to resting offset
        float returnDuration = keyframeDuration;
        float returnElapsed = 0f;
        Vector3 finalStart = multiRotConstraint.data.offset;

        while (returnElapsed < returnDuration)
        {
            returnElapsed += Time.deltaTime;
            float t = returnElapsed / returnDuration;
            multiRotConstraint.data.offset = Vector3.Lerp(finalStart, rotationOffset, t);
            yield return null;
        }

        // Ensure exact final resting offset
        multiRotConstraint.data.offset = rotationOffset;
        recoilCoroutine = null;
    }
}
