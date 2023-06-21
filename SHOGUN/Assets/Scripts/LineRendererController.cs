using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    [SerializeField] private Transform _arrowFromPlayerStartPoint;
    [SerializeField] private Transform _arrowFromHandStartPoint;

    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    public void DrawLineFromPlayer(Vector3 position)
    {
        if (position == null) return;

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, _arrowFromPlayerStartPoint.position);
        _lineRenderer.SetPosition(1, position);
    }

    public void DrawLineFromHand(Vector3 position)
    {
        if (position == null) return;

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, _arrowFromHandStartPoint.position);
        _lineRenderer.SetPosition(1, position);
    }

    public void StartDrawing()
    {
        if (!_lineRenderer.enabled) _lineRenderer.enabled = true;
    }

    public void StopDrawing()
    {
        if (_lineRenderer.enabled) _lineRenderer.enabled = false;
    }

}
