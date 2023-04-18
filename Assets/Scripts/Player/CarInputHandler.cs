using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarInputHandler : MonoBehaviour
{
    private PlayerInputActions playerActions;


    private CarController carController;
    private DeliveryController deliveryController;

    private void OnEnable()
    {
        playerActions.Enable();

        playerActions.Player.Move.performed += Move;
        playerActions.Player.Move.canceled += Move;

        playerActions.Player.MoveLeftRight.performed += MoveLeftRight;
        playerActions.Player.MoveLeftRight.canceled += MoveLeftRight;

        playerActions.Player.MoveUp.performed += MoveUp;
        playerActions.Player.MoveUp.canceled += MoveUp;

        playerActions.Player.MoveDown.performed += MoveDown;
        playerActions.Player.MoveDown.canceled += MoveDown;

        playerActions.Player.Drop.performed += Drop;

    }

    private void OnDisable()
    {
        playerActions.Enable();
    }


    private void Awake()
    {
        carController = GetComponent<CarController>();
        deliveryController = GetComponent<DeliveryController>();

        playerActions = new PlayerInputActions();
    }


    void Update()
    {       

    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 inputVector = Vector2.zero;

        if (context.performed)
        {
            inputVector = context.ReadValue<Vector2>();
            carController.SetInputVector(inputVector);
        }
        else if (context.canceled)
        {
            carController.SetInputVector(Vector2.zero);
        }
    }

    private void MoveLeftRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float x = context.ReadValue<float>();
            carController.SetSteering(x);
        }
        else if (context.canceled)
        {
            carController.SetSteering(0);
        }
    }

    private void MoveUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float y = context.ReadValue<float>();
            carController.SetAcceleration(y);
        }
        else if (context.canceled)
        {
            carController.SetAcceleration(0);
        }
    }

    private void MoveDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float y = context.ReadValue<float>();
            carController.SetAcceleration(-y);
        }
        else if (context.canceled)
        {
            carController.SetAcceleration(0);
        }
    }

    private void Drop(InputAction.CallbackContext context)
    {
        deliveryController.DropPackage();
    }
}
