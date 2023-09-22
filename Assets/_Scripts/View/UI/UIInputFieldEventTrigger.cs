using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;

public class UIInputFieldEventTrigger : MonoBehaviour
{
    [SerializeField] GameObject targetButton;

    private TMP_InputField inputField;
    private EventSystem eventSystem;
    private PlayerInputActions playerActions;

    private bool isSubmit = false;

    private void OnEnable()
    {
        playerActions.Enable();

        playerActions.UI.Submit.performed += SubitHandler;
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    private void Awake()
    {
        playerActions = new PlayerInputActions();
        eventSystem = EventSystem.current;
        inputField = GetComponent<TMP_InputField>();

        inputField.onEndEdit.AddListener(OnEndEditHandler);
        inputField.onDeselect.AddListener(OnDeselectHandler);
    }

    private void OnEndEditHandler(string value)
    {
        if (isSubmit)
        {
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(targetButton, new BaseEventData(eventSystem));
        }
    }

    private void OnDeselectHandler(string value)
    {
        isSubmit = false;
    }

    private void SubitHandler(InputAction.CallbackContext context)
    {
        isSubmit = true;
    }

}
