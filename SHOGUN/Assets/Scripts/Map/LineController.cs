using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    private LineRenderer _lineRenderer;
    [SerializeField] private float _xOffset=(float)-2.5;

    private void Awake() {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void setTarget(Transform target,Transform parent){
        _lineRenderer.positionCount=2;
        float lineTargetX=-(parent.localPosition.x-target.localPosition.x)/10;
        float lineTargetY=-(parent.localPosition.y-target.localPosition.y)/10;

        float startOffsetY=LineEquation(lineTargetX,lineTargetY,_xOffset);
        float endOffsetY=LineEquation(lineTargetX,lineTargetY,lineTargetX-_xOffset);
        
        _lineRenderer.SetPosition(0,new Vector3(_xOffset,startOffsetY));
        _lineRenderer.SetPosition(1,new Vector3(lineTargetX-_xOffset,endOffsetY));
    }

    private float LineEquation(float x2,float y2,float pointX){
        float m=y2/x2;
        float c= y2 - m* x2;
        return m*pointX+c;
    }
}
