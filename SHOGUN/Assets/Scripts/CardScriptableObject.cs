using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Card/New Card", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    public int Value;
    public int Cost;
}
