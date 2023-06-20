using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class UIEventTrigger : EventTrigger
{
    private Color32 normalColor = new Color32(255, 255, 255, 255);
    private Color32 highlightedColor = new Color32(255, 255, 255, 255);
    private Color32 pressedColor = new Color32(238, 127, 127, 255);
    private Color32 selectedColor = new Color32(243, 189, 189, 255);

    private Vector3 originalScale;
    private Button button;

    private EventSystem eventSystem;
    private MMFeedbacks buttonFeedbacks;

    private void Start()
    {
        eventSystem = EventSystem.current;
        originalScale = transform.localScale;
        buttonFeedbacks = GetComponentInChildren<MMFeedbacks>();

        button = GetComponent<Button>();
        if (button != null)
        {
            SetButtonColors();
        }
    }

    private void Update()
    {
        if(buttonFeedbacks == null)
        {
            buttonFeedbacks = GetComponentInChildren<MMFeedbacks>();
        }
    }

    private void SetButtonColors()
    {
        var colors = button.colors;

        colors.normalColor = normalColor;
        colors.highlightedColor = highlightedColor;
        colors.pressedColor = pressedColor;
        colors.selectedColor = selectedColor;

        button.colors = colors;
    }

    public override void OnSelect(BaseEventData data)
    {
        // Scale up with 15%
        //Vector3 newSize = transform.localScale * 1.15f;
        //StartCoroutine(Transition(newSize, 0.2f));

        if (buttonFeedbacks == null)
        {
            buttonFeedbacks = GetComponentInChildren<MMFeedbacks>();
        }

        buttonFeedbacks.PlayFeedbacks();
        MusicController.Instance.PlayMenuNavigationSFX();

    }

    public override void OnPointerEnter(PointerEventData data)
    {
        if (IsButtonSelected(this.gameObject))
        {
            return;
        }

        // Scale up with 15%
        //Vector3 newSize = transform.localScale * 1.15f;
        //StartCoroutine(Transition(newSize, 0.2f));

        buttonFeedbacks.PlayFeedbacks();
        SetSelectedButton(this.gameObject);
        MusicController.Instance.PlayMenuNavigationSFX();
    }

    public override void OnDeselect(BaseEventData data)
    {      

        buttonFeedbacks.StopFeedbacks();
        Vector3 newSize = originalScale;
        StartCoroutine(Transition(newSize, 0.2f));
    }

    public override void OnPointerExit(PointerEventData data)
    {
        buttonFeedbacks.StopFeedbacks();
        Vector3 newSize = originalScale;
        StartCoroutine(Transition(newSize, 0.2f));
    }

    public override void OnSubmit(BaseEventData data)
    {
        //transform.localScale = originalScale;

        //buttonFeedbacks.StopFeedbacks();
        //Vector3 newSize = originalScale;
        //StartCoroutine(Transition(newSize, 0.2f));
        MusicController.Instance.PlayButtonSubmitSFX();
    }

    public override void OnPointerClick(PointerEventData data)
    {
        //transform.localScale = originalScale;
        //buttonFeedbacks.StopFeedbacks();
        //Vector3 newSize = originalScale;
        //StartCoroutine(Transition(newSize, 0.2f));
        MusicController.Instance.PlayButtonSubmitSFX();
    }

    private IEnumerator Transition(Vector3 newSize, float transitionTime)
    {
        float timer = 0;
        Vector3 startSize = transform.localScale;

        while (timer < transitionTime)
        {
            timer += Time.unscaledDeltaTime;

            yield return null;

            transform.localScale = Vector3.Lerp(startSize, newSize, timer / transitionTime);
        }
    }

    private void SetSelectedButton(GameObject selectedButton)
    {
        eventSystem.SetSelectedGameObject(selectedButton, new BaseEventData(eventSystem));
    }

    private bool IsButtonSelected(GameObject button)
    {
        return button.Equals(eventSystem.currentSelectedGameObject);
    }
}
