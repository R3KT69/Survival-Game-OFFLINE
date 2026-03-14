using UnityEngine;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isHovering;
    public bool isCurrentlyHeld;
    public bool justReleased;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        Debug.Log($"Mouse entered object: {gameObject.name}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        Debug.Log($"Mouse exited object: {gameObject.name}");
    }

    void Update()
    {
        // Holding
        isCurrentlyHeld = isHovering && Input.GetMouseButton(0);

        // Release detection
        if (isHovering && Input.GetMouseButtonUp(0))
        {
            justReleased = true;
        }
        else
        {
            justReleased = false;
        }
    }
}