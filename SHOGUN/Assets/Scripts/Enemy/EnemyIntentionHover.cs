using UnityEngine;

public class EnemyIntentionHover : MonoBehaviour
{
    [SerializeField] private GameObject descriptionBackground;

    void Start()
    {
        descriptionBackground.SetActive(false);
    }
    void OnMouseEnter()
    {
        descriptionBackground.SetActive(true);
    }

    void OnMouseExit()
    {
        descriptionBackground.SetActive(false);
    }
}