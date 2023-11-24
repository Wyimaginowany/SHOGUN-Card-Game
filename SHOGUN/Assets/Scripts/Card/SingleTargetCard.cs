using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SingleTargetCard : Card
{
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _groundLayer;

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
        base.OnBeeingDragged();

        List<PossibleAreas> possibleDropAreas = GetDropAreas();

        
        if (possibleDropAreas.Contains(PossibleAreas.AimArea))
        {
            _lineRendererController.StartDrawing();

            _cardVisualCanvasGroup.alpha = 0;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Vector3 lineEndPoint = Vector3.zero;

            if (Physics.Raycast(ray, out _enemyHit, Mathf.Infinity, _enemyLayer))
            {
                //enemy under pointer
                lineEndPoint = _enemyHit.collider.gameObject.transform.position;
            }
            else if (Physics.Raycast(ray, out _groudHit, Mathf.Infinity, _groundLayer))
            {
               //ground hit             
                lineEndPoint = _groudHit.point;
            }
            _lineRendererController.DrawLineFromPlayer(lineEndPoint);

            return;
        }

        _cardVisualCanvasGroup.alpha = 1;
        _lineRendererController.StopDrawing();
    }

    protected override void EndDragging()
    {
        base.EndDragging();

        List<PossibleAreas> possibleDropAreas = GetDropAreas();

        _lineRendererController.StopDrawing();

        if (possibleDropAreas.Contains(PossibleAreas.ThrowOutArea))
        {
            ShuffleCardIntoDeck();
            return;
        }

        if (!_combatManager.HaveEnoughMana(CardData.Cost))
        {
            _cardVisualCanvasGroup.alpha = 1;
            ReturnCardToHand();
            return;
        }

        if (possibleDropAreas.Contains(PossibleAreas.AimArea))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit targetEnemyHit;
            if (Physics.Raycast(ray, out targetEnemyHit, Mathf.Infinity, _enemyLayer))
            {
                PlayCardOnTarget(targetEnemyHit.collider.GetComponent<EnemyHealth>());
                return;
            }
        }
        _cardVisualCanvasGroup.alpha = 1;
        ReturnCardToHand();
    }

    protected virtual void PlayCardOnTarget(EnemyHealth enemyHealth)
    {
        PlayCard();
        _cardVisualCanvasGroup.alpha = 1;
    }
}
