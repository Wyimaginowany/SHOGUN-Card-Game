using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleTargetCard : Card
{
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private GameObject _cardVisual;

    private LineRendererController _lineRendererController;

    private RaycastHit _enemyHit;
    private RaycastHit _groudHit;

    protected override void Start()
    {
        base.Start();
        _lineRendererController = (LineRendererController)FindObjectOfType(typeof(LineRendererController));
    }

    protected override void BeginDragging()
    {
        base.BeginDragging();
    }

    protected override void OnBeeingDragged()
    {
        List<PossibleAreas> possibleDropAreas = GetDropAreas();

        
        if (possibleDropAreas.Contains(PossibleAreas.AimArea))
        {
            _lineRendererController.StartDrawing();

            _cardVisual.SetActive(false);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Vector3 lineEndPoint = Vector3.zero;

            if (Physics.Raycast(ray, out _enemyHit, Mathf.Infinity, _enemyLayer))
            {
                Debug.Log("Enemy Snap Arrow");
                lineEndPoint = _enemyHit.collider.gameObject.transform.position;
            }
            else if (Physics.Raycast(ray, out _groudHit, Mathf.Infinity, _groundLayer))
            {
                Debug.Log("Ground HIT");              
                lineEndPoint = _groudHit.point;
            }
            _lineRendererController.DrawLineFromPlayer(lineEndPoint);

            return;
        }

        _cardVisual.SetActive(true);
        _lineRendererController.StopDrawing();
        transform.position = Input.mousePosition;
    }

    protected override void EndDragging()
    {
        List<PossibleAreas> possibleDropAreas = GetDropAreas();

        _lineRendererController.StopDrawing();

        if (possibleDropAreas.Contains(PossibleAreas.ThrowOutArea))
        {
            ShuffleCardIntoDeck();
            return;
        }

        if (!_combatManager.HaveEnoughMana(_cardData.Cost))
        {
            _cardVisual.SetActive(true);
            ReturnCardToHand();
            return;
        }

        if (possibleDropAreas.Contains(PossibleAreas.AimArea))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit targetEnemyHit;
            if (Physics.Raycast(ray, out targetEnemyHit, Mathf.Infinity, _enemyLayer))
            {
                targetEnemyHit.collider.GetComponent<EnemyTest>().TakeDamage(_cardData.Value);
                _cardVisual.SetActive(true);
                PlayCard();
                return;
            }
        }
        _cardVisual.SetActive(true);
        ReturnCardToHand();
    }
}
