using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private Vector2 movementBounds = new Vector2(50f, 50f);
    [SerializeField] private float mousePanSpeed = 0.5f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 15f;

    private Camera mainCamera;
    private Vector3 boundsCenter;
    private Vector2 lastMousePosition;
    private bool isMousePanning;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();

        if (mainCamera == null)
        {
            Debug.LogError("Camera component not found on this GameObject!");
            enabled = false;
            return;
        }

        boundsCenter = new Vector3(0, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        Vector3 movement = Vector3.zero;

        movement += HandleKeyboardMovement();

        movement += HandleMousePanning();

        if (movement != Vector3.zero)
        {
            Vector3 newPosition = transform.position + movement;
            newPosition.x = Mathf.Clamp(newPosition.x, boundsCenter.x - movementBounds.x, boundsCenter.x + movementBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, boundsCenter.y - movementBounds.y, boundsCenter.y + movementBounds.y);
            transform.position = newPosition;
        }

        HandleZoom();
    }

    private Vector3 HandleKeyboardMovement()
    {
        Vector2 input = Keyboard.current.wKey.isPressed ? Vector2.up : Vector2.zero;
        input += Keyboard.current.sKey.isPressed ? Vector2.down : Vector2.zero;
        input += Keyboard.current.dKey.isPressed ? Vector2.right : Vector2.zero;
        input += Keyboard.current.aKey.isPressed ? Vector2.left : Vector2.zero;

        // Alternative arrow key input
        input += Keyboard.current.upArrowKey.isPressed ? Vector2.up : Vector2.zero;
        input += Keyboard.current.downArrowKey.isPressed ? Vector2.down : Vector2.zero;
        input += Keyboard.current.rightArrowKey.isPressed ? Vector2.right : Vector2.zero;
        input += Keyboard.current.leftArrowKey.isPressed ? Vector2.left : Vector2.zero;

        if (input.magnitude > 1f)
            input.Normalize();

        return new Vector3(input.x, input.y, 0) * moveSpeed * Time.deltaTime;
    }

    private Vector3 HandleMousePanning()
    {
        Vector3 mouseDelta = Vector3.zero;

        if (Mouse.current.middleButton.wasPressedThisFrame)
        {
            isMousePanning = true;
            lastMousePosition = Mouse.current.position.ReadValue();
        }
        else if (Mouse.current.middleButton.wasReleasedThisFrame)
        {
            isMousePanning = false;
        }

        if (isMousePanning)
        {
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 delta = (lastMousePosition - currentMousePosition) * mousePanSpeed;

            float orthoSize = mainCamera.orthographicSize;
            float aspectRatio = mainCamera.aspect;
            mouseDelta.x = (delta.x / Screen.width) * (orthoSize * 2 * aspectRatio);
            mouseDelta.y = (delta.y / Screen.height) * (orthoSize * 2);

            lastMousePosition = currentMousePosition;
        }

        return mouseDelta;
    }

    private void HandleZoom()
    {
        float scrollValue = Mouse.current.scroll.ReadValue().y;

        if (scrollValue != 0)
        {
            if (mainCamera.orthographic)
            {
                float newSize = mainCamera.orthographicSize - (scrollValue * zoomSpeed * Time.deltaTime);
                mainCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            }
            else
            {
                Vector3 newPosition = transform.position + transform.forward * (scrollValue * zoomSpeed * Time.deltaTime);
                float distanceFromOrigin = Vector3.Distance(newPosition, boundsCenter);

                if (distanceFromOrigin > minZoom && distanceFromOrigin < maxZoom)
                {
                    transform.position = newPosition;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Vector3 previewCenter = new Vector3(0, transform.position.y, transform.position.z);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(previewCenter, new Vector3(movementBounds.x * 2, movementBounds.y * 2, 1));
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(boundsCenter, new Vector3(movementBounds.x * 2, movementBounds.y * 2, 1));
        }
    }
}
