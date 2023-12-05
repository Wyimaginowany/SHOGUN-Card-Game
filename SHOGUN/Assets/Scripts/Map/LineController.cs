using System.Collections;
using System.Collections.Generic;
using Radishmouse;
using UnityEngine;

public class LineController : MonoBehaviour
{

    
    public UILineRenderer _lineRenderer;
    
    [SerializeField] private float _xOffset=(float)-2.5;

    private void Awake() {
        _lineRenderer = GetComponent<UILineRenderer>();
    }

    public void setTarget(Transform target,Transform parent){
        _lineRenderer.points=new Vector2[2];
        
        float lineTargetX=-(parent.localPosition.x-target.localPosition.x)/10;
        float lineTargetY=-(parent.localPosition.y-target.localPosition.y)/10;

        float startOffsetY=LineEquation(lineTargetX,lineTargetY,_xOffset);
        float endOffsetY=LineEquation(lineTargetX,lineTargetY,lineTargetX-_xOffset);

        _lineRenderer.points[0]=(new Vector2(_xOffset,startOffsetY));
        _lineRenderer.points[1]=(new Vector2(lineTargetX-_xOffset,endOffsetY));
        
    }

    private float LineEquation(float x2,float y2,float pointX){
        float m=y2/x2;
        float c= y2 - m* x2;
        return m*pointX+c;
    }
}
