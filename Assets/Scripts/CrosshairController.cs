using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CrosshairController : MonoBehaviour
{
    public RectTransform crosshair;

    [Range(0.1f, 50f)]
    public float sensitivity = 5f;

    public float clampX = 400f;
    public float clampY = 300f;

    private Vector2 _crosshairPos;

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _crosshairPos += input * sensitivity;
        _crosshairPos.x = Mathf.Clamp(_crosshairPos.x, -clampX, clampX);
        _crosshairPos.y = Mathf.Clamp(_crosshairPos.y, -clampY, clampY);
        crosshair.anchoredPosition = _crosshairPos;
    }

    public Vector3 GetWorldPosition(float distance)
    {
        Vector3 screenPos = new Vector3(
            Screen.width / 2f + _crosshairPos.x,
            Screen.height / 2f + _crosshairPos.y,
            distance
        );
        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}