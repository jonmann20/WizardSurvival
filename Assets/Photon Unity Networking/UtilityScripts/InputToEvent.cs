using UnityEngine;

/// <summary>
/// Utility component to forward mouse or touch input to clicked gameobjects.
/// Calls OnPress, OnClick and OnRelease methods on "first" game object.
/// </summary>
public class InputToEvent : MonoBehaviour
{

    private GameObject lastGo;
    public static Vector3 inputHitPos;
    public bool DetectPointedAtGameObject;
    public static GameObject goPointedAt { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (DetectPointedAtGameObject)
        {
            goPointedAt = RaycastObject(Input.mousePosition);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Press(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Release(touch.position);
            }

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Press(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Release(Input.mousePosition);
        }
    }

    private void Press(Vector2 screenPos)
    {
        lastGo = RaycastObject(screenPos);
        if (lastGo != null)
        {
            lastGo.SendMessage("OnPress", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void Release(Vector2 screenPos)
    {
        if (lastGo != null)
        {
            GameObject currentGo = RaycastObject(screenPos);
            if (currentGo == lastGo) lastGo.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
            lastGo.SendMessage("OnRelease", SendMessageOptions.DontRequireReceiver);
            lastGo = null;
        }
    }

    private GameObject RaycastObject(Vector2 screenPos)
    {
        RaycastHit info;
        if (Physics.Raycast(this.camera.ScreenPointToRay(screenPos), out info, 200))
        {
            inputHitPos = info.point;
            return info.collider.gameObject;
        }

        return null;
    }
}
