using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public float zeroPercentWidth = 0f;
    public float maxPercentWidth = 250f;

    float _currentValue = 0f;

    float _startValue = 0f;

    float _finishValue = 1f;

    private float CurrentValue {
        get {
            return _currentValue;
        }
        set {
            _currentValue = value;
            UpdateProgressVisual();
        }
    }

    void Awake() {
        Refresh(0f);
    }

    public RectTransform foregroundProgressTransform;

    public void Refresh(float value) {
        if(value < 0) {
            value = 0f;
        }
        else if(value > 1) {
            value = 1f;
        }
        CurrentValue = value;
    }

    void UpdateProgressVisual() {
        var progress = ((_currentValue - _startValue)/_finishValue);
        foregroundProgressTransform.sizeDelta = new Vector2(maxPercentWidth * progress,foregroundProgressTransform.sizeDelta.y);
    }
}
