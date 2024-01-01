using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SecondSlider : MonoBehaviour
{
    [SerializeField] private Slider _secondSlider;
    [SerializeField] private float _timeBeforeChangingSecondSlider = 1;
    [SerializeField] private float _transitionTime = 1;

    private Slider _mainSlider;
    private float _newSliderValue;
    private float _oldSliderValue;
    private float _timer = 0;
    private bool _shouldUpdate = false;

    private void Start()
    {
        _mainSlider = GetComponent<Slider>();
        _mainSlider.onValueChanged.AddListener(delegate { OnMainSliderValueChange(); });
    }

    private void Update()
    {
        if (!_shouldUpdate) return;

        _timer += Time.deltaTime;
        float percentageComplete = _timer / _transitionTime;
        float value = Mathf.Lerp(_oldSliderValue, _newSliderValue, percentageComplete);
        _secondSlider.value = value;

        if (value == _newSliderValue)
        {
            _timer = 0;
            _shouldUpdate = false;
        }
    }

    public void OnMainSliderValueChange()
    {
        _newSliderValue = _mainSlider.value;
        _oldSliderValue = _secondSlider.value;
        StartCoroutine(StartChangingValue());
    }

    IEnumerator StartChangingValue()
    {
        yield return new WaitForSeconds(_timeBeforeChangingSecondSlider);
        if (_oldSliderValue != 0) _shouldUpdate = true;
    }
}
