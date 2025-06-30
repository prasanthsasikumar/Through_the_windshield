using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.XR;

public class MenuToggleManager : MonoBehaviour
{
    public GameObject menuPanel;                  // Assign in inspector

    private CarInputActions inputActions;
    private bool menuVisible = false;
    private float initialPositionTimer = 3.0f;    // Timer for initial positioning (3 seconds)
    private bool initialPositionComplete = false; // Flag to check if initial positioning is complete

    void Awake()
    {
        inputActions = new CarInputActions();
        inputActions.CarControls.ToggleMenu.performed += ctx => ToggleMenu();
    }

    void Start()
    {
        // Show menu at start and position it in front of user
        menuVisible = true;
        menuPanel.SetActive(true);
        PositionMenuInFrontOfUser();
        
        // Start coroutine to handle the initial positioning
        StartCoroutine(InitialPositioningCoroutine());
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        // Keep repositioning menu during the first 3 seconds
        if (!initialPositionComplete)
        {
            PositionMenuInFrontOfUser();
        }
    }

    private void ToggleMenu()
    {
        menuVisible = !menuVisible;
        menuPanel.SetActive(menuVisible);

        if (menuVisible)
        {
            PositionMenuInFrontOfUser();
        }
    }

    private void PositionMenuInFrontOfUser()
    {
        // Get the XR camera (in most Unity XR setups, this is still Camera.main)
        Camera xrCamera = Camera.main;
        if (xrCamera != null)
        {
            // Position the menu in front of the user's view
            menuPanel.transform.position = xrCamera.transform.position + xrCamera.transform.forward * 1f;
            
            // Make sure the menu isn't too low
            if (menuPanel.transform.position.y < 1f)
            {
                menuPanel.transform.position = new Vector3(menuPanel.transform.position.x, 1f, menuPanel.transform.position.z);
            }
            
            // Make the menu face the user
            menuPanel.transform.rotation = Quaternion.LookRotation(menuPanel.transform.position - xrCamera.transform.position);
        }
    }

    private IEnumerator InitialPositioningCoroutine()
    {
        // Wait for 3 seconds while continuously positioning the menu
        float timer = initialPositionTimer;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        // After 3 seconds, mark initial positioning as complete
        initialPositionComplete = true;
        
        // Hide the menu after initial positioning unless user has interacted with it
        if (!menuPanel.activeSelf)
        {
            menuVisible = false;
            menuPanel.SetActive(false);
        }
    }
}
