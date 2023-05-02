using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTrigger : EventTrigger
{


    public override void OnSelect(BaseEventData data)
    {
        Debug.Log("OnSelect");
        StartCoroutine(Transition(new Vector3(1.15f, 1.15f, 1.15f), 0.2f));
    }

    public override void OnDeselect(BaseEventData data)
    {
        Debug.Log("OnDeselect");
        StartCoroutine(Transition(Vector3.one, 0.2f));
        MusicController.Instance.PlayMenuNavigationSFX();
    }

    public override void OnSubmit(BaseEventData data)
    {
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
