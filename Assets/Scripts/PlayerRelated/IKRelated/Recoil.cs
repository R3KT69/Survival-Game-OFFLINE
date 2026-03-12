using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Recoil : MonoBehaviour
{
    private MultiRotationConstraint recoilRig;
    void Start()
    {
        recoilRig = GetComponent<MultiRotationConstraint>();

        if (recoilRig == null)
        {
            Debug.Log("Recoil rig found.");
        }
    }

    
    void TriggerRecoil()
    {
        recoilRig.data.offset = new Vector3(-75f, recoilRig.data.offset.y, recoilRig.data.offset.z);
    }
}
