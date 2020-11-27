using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEmpty : Menu
{
    public Text text1;
    public Slider sliderTimer;
    public RectTransform panelTimer;

    private Lerp<float> lerpTimer;
    private System.Action onTimerEnd;

    public override void OnDisabled()
    {
        
    }

    public override void OnEnabled()
    {
        text1.text = "";
        panelTimer.gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {
        //Lerp timer
        if(Lerp.Apply(lerpTimer, SetTimerValue))
        {
            lerpTimer = null;
            onTimerEnd();
        }
    }

    /// <summary>
    /// Sets the title text
    /// </summary>
    /// <param name="text">The text</param>
    public void SetText(string text)
    {
        text1.text = text;
    }

    /// <summary>
    /// Start a timer that calls an action on end
    /// </summary>
    /// <param name="time">Timer time</param>
    /// <param name="onEnd">Action to call on end</param>
    public void SetTimerActive(float time, System.Action onEnd)
    {
        panelTimer.gameObject.SetActive(true);
        lerpTimer = Lerp.Get(time, 1f, 0f);
        onTimerEnd = onEnd;
    }

    /// <summary>
    /// Display timer value
    /// </summary>
    /// <param name="f"></param>
    void SetTimerValue(float f)
    {
        sliderTimer.value = f;
    }

    /// <summary>
    /// Stops the timer
    /// </summary>
    public void StopTimer()
    {
        lerpTimer = null;
    }
}
