using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetShieldCard : Card
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _groundLayer;

    private LineRendererController _lineRendererController;


    private RaycastHit _playerHit;
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

            if (Physics.Raycast(ray, out _playerHit, Mathf.Infinity, _playerLayer))
            {
                lineEndPoint = _playerHit.collider.gameObject.transform.position;
            }
            else if (Physics.Raycast(ray, out _groudHit, Mathf.Infinity, _groundLayer))
            {
                lineEndPoint = _groudHit.point;
            }
            _lineRendererController.DrawLineFromHand(lineEndPoint);

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

        if (!_combatManager.HaveEnoughMana(_cardData.Cost))
        {
            _cardVisualCanvasGroup.alpha = 1;
            ReturnCardToHand();
            return;
        }

        if (possibleDropAreas.Contains(PossibleAreas.AimArea))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit targetPlayerHit;
            if (Physics.Raycast(ray, out targetPlayerHit, Mathf.Infinity, _playerLayer))
            {
                targetPlayerHit.collider.GetComponent<PlayerHealth>().GiveShield(_cardData.Value);
                _cardVisualCanvasGroup.alpha = 1;
                PlayCard();
                return;
            }
        }
        _cardVisualCanvasGroup.alpha = 1;
        ReturnCardToHand();
    }
}
