using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GraphicRaycaster canvasRaycaster;

    [Header("References")]
    [SerializeField] private CameraController cameraController;

    [Header("Movement Settings")]
    [SerializeField] private Slider moveSpeedSlider;
    [SerializeField] private Slider mousePanSpeedSlider;

    [Header("Zoom Settings")]
    [SerializeField] private Slider zoomSpeedSlider;

    [Header("Value Display Text (Optional)")]
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    [SerializeField] private TextMeshProUGUI mousePanSpeedText;
    [SerializeField] private TextMeshProUGUI zoomSpeedText;


    void Awake()
    {

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (settingsButton != null)
            settingsButton.onClick.AddListener(ToggleSettings);
    }

    private void ToggleSettings()
    {
        if (settingsPanel != null)
        {
            bool isActive = !settingsPanel.activeSelf;
            settingsPanel.SetActive(isActive);

            // Disable camera controls and other interactions when settings are open
            if (cameraController != null)
                cameraController.enabled = !isActive;

            // Enable/disable raycasting on the main canvas
            if (canvasRaycaster != null)
                canvasRaycaster.enabled = !isActive;
        }
    }

    private void Start()
    {
        if (cameraController == null)
        {
#pragma warning disable
            cameraController = FindObjectOfType<CameraController>();
            if (cameraController == null)
            {
                Debug.LogError("No CameraController found in the scene!");
                return;
            }
        }

        // Initialize sliders with current values
        InitializeSliders();

        // Add listeners to sliders
        moveSpeedSlider.onValueChanged.AddListener(UpdateMoveSpeed);
        mousePanSpeedSlider.onValueChanged.AddListener(UpdateMousePanSpeed);
        zoomSpeedSlider.onValueChanged.AddListener(UpdateZoomSpeed);
    }

    private void InitializeSliders()
    {
        // Set up move speed slider
        moveSpeedSlider.value = cameraController.MoveSpeed;
        UpdateMoveSpeedText(cameraController.MoveSpeed);

        // Set up mouse pan speed slider
        mousePanSpeedSlider.value = cameraController.MousePanSpeed;
        UpdateMousePanSpeedText(cameraController.MousePanSpeed);

        // Set up zoom speed slider
        zoomSpeedSlider.value = cameraController.ZoomSpeed;
        UpdateZoomSpeedText(cameraController.ZoomSpeed);

    }

    private void UpdateMoveSpeed(float value)
    {
        cameraController.MoveSpeed = value;
        UpdateMoveSpeedText(value);
    }

    private void UpdateMousePanSpeed(float value)
    {
        cameraController.MousePanSpeed = value;
        UpdateMousePanSpeedText(value);
    }

    private void UpdateZoomSpeed(float value)
    {
        cameraController.ZoomSpeed = value;
        UpdateZoomSpeedText(value);
    }

    private void UpdateMoveSpeedText(float value)
    {
        if (moveSpeedText != null)
            moveSpeedText.text = $"Move Speed: {value:F1}";
    }

    private void UpdateMousePanSpeedText(float value)
    {
        if (mousePanSpeedText != null)
            mousePanSpeedText.text = $"Mouse Pan Speed: {value:F2}";
    }

    private void UpdateZoomSpeedText(float value)
    {
        if (zoomSpeedText != null)
            zoomSpeedText.text = $"Zoom Speed: {value:F1}";
    }

    private void OnDisable()
    {
        // Re-enable everything when the settings UI is disabled
        if (cameraController != null)
            cameraController.enabled = true;

        if (canvasRaycaster != null)
            canvasRaycaster.enabled = true;
    }
}