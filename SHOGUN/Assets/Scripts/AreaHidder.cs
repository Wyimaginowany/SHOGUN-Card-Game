using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaHidder : MonoBehaviour
{
    private RawImage _areaImage;
    private float _imageStartAlpha = 0.3f;

    private void Start()
    {
        _areaImage = GetComponent<RawImage>();
        //_imageStartAlpha = _areaImage.color.a;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SwitchHideState();
        }
    }

    private void SwitchHideState()
    {
        var tempColor = _areaImage.color;
        if (tempColor.a != 0)
        {
            tempColor.a = 0;
        }
        else
        {
            tempColor.a = _imageStartAlpha;
        }

        _areaImage.color = tempColor;
    }
}