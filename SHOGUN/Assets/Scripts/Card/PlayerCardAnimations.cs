using System;
using CardEnums;
using UnityEngine;
public class PlayerCardAnimations : MonoBehaviour
{
    public static void TriggerAnimation(CardAnimation cardAnimation)
    {
        PlayerHealth playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        Animator playerAnimator =  playerHealth.GetAnimator();

        switch(cardAnimation)
        {
            case CardAnimation.SingleTargetAttack :
                playerAnimator.SetTrigger("SingleTargetAttack");
                break;
            case CardAnimation.MultipleTargetAttack :
                playerAnimator.SetTrigger("MultipleTargetAttack");
                break;
            case CardAnimation.SelfHealing :
                playerAnimator.SetTrigger("SelfHealing");
                break;
            case CardAnimation.SelfArmoring :
                playerAnimator.SetTrigger("SelfArmoring");
                break;

        }
    }
}
