using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTrigger : EventTrigger
{
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public override void OnSelect(BaseEventData data)
    {
        // Scale up with 15%
        Vector3 newSize = transform.localScale * 1.15f;
        StartCoroutine(Transition(newSize, 0.2f));
    }

    public override void OnDeselect(BaseEventData data)
    {
        Vector3 newSize = originalScale;
        StartCoroutine(Transition(newSize, 0.2f));
        MusicController.Instance.PlayMenuNavigationSFX();
    }

    public override void OnSubmit(BaseEventData data)
    {
        transform.localScale = originalScale;
        MusicController.Instance.PlayButtonSubmitSFX();
    }

    private IEnumerator Transition(Vector3 newSize, float transitionTime)
    {
        float timer = 0;
        Vector3 startSize = transform.localScale;

        while(timer < transitionTime)
        {
            timer += Time.unscaledDeltaTime;

            yield return null;

            transform.localScale = Vector3.Lerp(startSize, newSize, timer / transitionTime);
        }
    }
}
