using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Card/New Card", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    public int Value = 1;
    public int Cost = 1;
    public Color CardColor;
}
