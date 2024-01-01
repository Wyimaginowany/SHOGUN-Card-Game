using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    [SerializeField] private Transform _arrowFromPlayerStartPoint;
    [SerializeField] private Transform _arrowFromHandStartPoint;

    private LineRenderer _lineRenderer;
    private bool _lockToTarget = false;
    private float _timer = 0f;
    [SerializeField] private float _transitionTime = 0.5f;
    private Vector3 _enemyArrowEndPosition;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    public void DrawLineFromPlayer(Vector3 position)
    {
        if (position == null) return;

        _lockToTarget = false;
        _timer = 0f;
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
        _lockToTarget = false;
        _timer = 0f;
    }

    public void LockToEnemy(Vector3 enemyArrowEndPoint)
    {
        _lockToTarget = true;
        _enemyArrowEndPosition = enemyArrowEndPoint;
    }


    private void Update()
    {
        if (_lockToTarget)
        {
            _timer += Time.deltaTime;
            float percentageComplete = _timer / _transitionTime;
            Vector3 position = Vector3.Lerp(_lineRenderer.GetPosition(1), _enemyArrowEndPosition, percentageComplete);
            _lineRenderer.SetPosition(1, position);

            if (Vector2.Distance(transform.position, _enemyArrowEndPosition) <= 0.1f)
            {
                _lockToTarget = false;
                _timer = 0f;
            }
        }
    }
}
