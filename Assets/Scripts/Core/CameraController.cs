using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector2 movementBounds = new Vector2(50f, 50f);
    [SerializeField] private float mousePanSpeed = 0.5f;
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 15f;

    Camera mainCamera;
    Vector3 velocity = Vector3.zero;
    Vector3 targetPosition;
    Vector3 boundsCenter;
    Vector2 lastMousePosition;
    bool isMousePanning;

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("Camera component not found on this Gameobject!");
            enabled = false;
            return;
        }
        targetPosition = transform.position;
        boundsCenter = new Vector3(0, transform.position.y, transform.position.z);
    }
    private void Update()
    {
        HandleZoom();
        HandleKeyboardMovement();
        HandleMousePanning();
        UpdateCameraPosition();
    }
    void HandleKeyboardMovement()
    {
        Vector2 input = Keyboard.current.wKey.isPressed ? Vector2.up : Vector2.zero;
        input += Keyboard.current.sKey.isPressed ? Vector2.down : Vector2.zero;
        input += Keyboard.current.aKey.isPressed ? Vector2.left : Vector2.zero;
        input += Keyboard.current.dKey.isPressed ? Vector2.right : Vector2.zero;

        input += Keyboard.current.upArrowKey.isPressed ? Vector2.up : Vector2.zero;
        input += Keyboard.current.downArrowKey.isPressed ? Vector2.down : Vector2.zero;
        input += Keyboard.current.leftArrowKey.isPressed ? Vector2.left : Vector2.zero;
        input += Keyboard.current.rightArrowKey.isPressed ? Vector2.right : Vector2.zero;

        if (input.magnitude > 1f)
            input.Normalize();

        Vector3 movement = new Vector3(input.x, input.y, 0) * moveSpeed * Time.deltaTime;
        targetPosition += movement;

        float clampedX = Mathf.Clamp(targetPosition.x, boundsCenter.x - movementBounds.x, boundsCenter.x + movementBounds.x);
        float clampedY = Mathf.Clamp(targetPosition.y, boundsCenter.y - movementBounds.y, boundsCenter.y + movementBounds.y);
        targetPosition = new Vector3(clampedX, clampedY, targetPosition.z);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
    void HandleMousePanning()
    {
        if (Mouse.current.middleButton.wasPressedThisFrame)
        {
            isMousePanning = true;
            lastMousePosition = Mouse.current.position.ReadValue();
        }
        else if (Mouse.current.middleButton.wasReleasedThisFrame)
            isMousePanning = false;

        if (isMousePanning)
        {
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 mouseDelta = (lastMousePosition - currentMousePosition) * mousePanSpeed;

            float orthoSize = mainCamera.orthographicSize;
            float aspectRatio = mainCamera.aspect;
            float mouseX = (mouseDelta.x / Screen.width) * (orthoSize * 2 * aspectRatio);
            float mouseY = (mouseDelta.y / Screen.height) * (orthoSize * 2);

            targetPosition += new Vector3(mouseX, mouseY, 0);
            lastMousePosition = currentMousePosition;
        }
    }
    void HandleZoom()
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
                Vector3 newPos = transform.position + transform.forward * (scrollValue * zoomSpeed * Time.deltaTime);
                float distanceFromOrigin = Vector3.Distance(newPos, Vector3.zero);
                if (distanceFromOrigin > minZoom && distanceFromOrigin < maxZoom)
                    transform.position = newPos;
            }
        }
    }
    void UpdateCameraPosition()
    {
        float clampedX = Mathf.Clamp(targetPosition.x, boundsCenter.x - movementBounds.x, boundsCenter.x + movementBounds.x);
        float clampedY = Mathf.Clamp(targetPosition.y, boundsCenter.y - movementBounds.y, boundsCenter.y + movementBounds.y);
        targetPosition = new Vector3(clampedX, clampedY, targetPosition.z);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
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
