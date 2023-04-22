using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTrigger : EventTrigger
{
    private MusicController musicController;

    private void Start()
    {
        musicController = FindObjectOfType<MusicController>();
    }

    public override void OnDeselect(BaseEventData data)
    {
        musicController.PlayMenuNavigationSFX();
    }

    public override void OnSubmit(BaseEventData data)
    {
        musicController.PlayButtonSubmitSFX();
    }
}
