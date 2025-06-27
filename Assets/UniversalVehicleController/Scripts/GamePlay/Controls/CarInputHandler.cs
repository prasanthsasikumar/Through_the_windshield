using UnityEngine;
using UnityEngine.InputSystem;
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

    public event System.Action OnChangeViewAction;

    private void Awake()
    {
        input = new CarInputActions();

        // Bind input actions
        input.CarControls.Accelerate.performed += ctx => Acceleration = ctx.ReadValue<float>();
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
    private void Update()
    {
        // Monitor and log the status of all button presses
        Debug.Log($"[CarInputHandler] Input Status:" +
                 $"\n- Acceleration: {Acceleration:F2}" +
                 $"\n- BrakeReverse: {BrakeReverse:F2}" +
                 $"\n- Steering: {Horizontal:F2}" +
                 $"\n- Pitch: {Pitch:F2}" +
                 $"\n- HandBrake: {HandBrake}" +
                 $"\n- Boost: {Boost}");

        // Alternative implementation with optional on-screen display
        // You could add a UI Text component to display this information
        // UpdateInputDisplayUI();
    }
}
