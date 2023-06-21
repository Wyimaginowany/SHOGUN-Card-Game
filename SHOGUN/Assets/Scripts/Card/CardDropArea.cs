using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDropArea : MonoBehaviour
{
    [SerializeField] private PossibleAreas _area;

    public PossibleAreas GetDropArea()
    {
        return _area;
    }
}

public enum PossibleAreas { PlayArea, ThrowOutArea, SnapArea, AimArea};