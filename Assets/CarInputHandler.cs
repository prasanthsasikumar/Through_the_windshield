using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using PG; // Namespace where ICarControl and VehicleController live

public class CarInputHandler : InitializePlayer, ICarControl
{
    private CarInputActions input;

    // ICarControl properties
    public float Acceleration { get; private set; }
    public float BrakeReverse { get; private set; }
    public float Horizontal { get; private set; }
    public float Pitch { get; private set; }
    public bool HandBrake { get; private set; }
    public bool Boost { get; private set; }
    public bool Reset { get; private set; }

    public event System.Action OnChangeViewAction;

    private void Awake()
    {
        input = new CarInputActions();

        // Bind input actions
        input.CarControls.Accelerate.performed += ctx => Acceleration = ApplyNonLinearCurve(ctx.ReadValue<float>());
        input.CarControls.Accelerate.canceled += ctx => Acceleration = 0f;

        input.CarControls.Brake.performed += ctx => BrakeReverse = ctx.ReadValue<float>();
        input.CarControls.Brake.canceled += ctx => BrakeReverse = 0f;

        input.CarControls.Steer.performed += ctx => Horizontal = ctx.ReadValue<float>();
        input.CarControls.Steer.canceled += ctx => Horizontal = 0f;

        input.CarControls.Pitch.performed += ctx => Pitch = ctx.ReadValue<float>();
        input.CarControls.Pitch.canceled += ctx => Pitch = 0f;

        input.CarControls.Handbrake.performed += ctx => HandBrake = true;
        input.CarControls.Handbrake.canceled += ctx => HandBrake = false;

        input.CarControls.Boost.performed += ctx => Boost = true;
        input.CarControls.Boost.canceled += ctx => Boost = false;

        input.CarControls.Reset.performed += ctx => HandleResetInput();
        input.CarControls.Reset.canceled += ctx => Reset = false;
    }

    private void OnEnable()
    {
        input.CarControls.Enable();
    }

    private void OnDisable()
    {
        input.CarControls.Disable();
    }

    public void ChangeView()
    {
        OnChangeViewAction?.Invoke();
    }

    public override bool Initialize(VehicleController vehicle)
    {
        base.Initialize(vehicle);
        
        if (Car)
        {
            var aiControl = Car.GetComponent<ICarControl>();
            if (aiControl == null || !(aiControl is PG.PositioningAIControl))
            {
                Car.CarControl = this;
            }
        }
        
        return IsInitialized;
    }

    public override void Uninitialize()
    {
        if (Car != null && Car.CarControl == this as ICarControl)
        {
            Car.CarControl = null;
        }
        
        base.Uninitialize();
    }

    private float ApplyNonLinearCurve(float value)
    {
        // Clamp to safety
        value = Mathf.Clamp01(value);
        float curved = Mathf.Pow(value, 2f);
        // Example: exponential curve (adjust the exponent as needed)
        Debug.Log($"Input: {value:F2}, Curved: {curved:F2}");
        return curved;
    }

    private void HandleResetInput()
    {
        Reset = true;
        GameObject spawnPosition = GameObject.Find("S34_Race_ForCrazyroad_SpawnPosition");
        if (spawnPosition != null)
        {
            //move spawn position to the front of the camera
            spawnPosition.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 5f;
        }

        if (Car != null)
        {
            Car.ResetVehicle();
        }
    }
}
