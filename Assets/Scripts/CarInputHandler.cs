using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarInputHandler : MonoBehaviour
{
    [SerializeField] public InputAction playerMovement;

    private CarController carController;

    private void OnEnable()
    {
        playerMovement.Enable();
    }

    private void OnDisable()
    {
        playerMovement.Disable();
    }


    private void Awake()
    {
        carController = GetComponent<CarController>();
    }


    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        // Get input from Unity's input system
        //inputVector.x = Input.GetAxis("Horizontal");
        //inputVector.y = Input.GetAxis("Vertical");

        inputVector = playerMovement.ReadValue<Vector2>();

        // Send the input to the car controller
        carController.SetInputVector(inputVector);

        
    }
}
