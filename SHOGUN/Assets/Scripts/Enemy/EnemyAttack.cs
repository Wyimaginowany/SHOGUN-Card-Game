using System;

[Serializable]
public class EnemyAttack<TEnum> where TEnum : Enum
{
    public TEnum Attack;
    public String AttackName = "";
    public AttackTypes AttackType;
    public int AttackPriority = 0;
    public int AttackCooldown = 0;
    public int AttackMaxCooldown = 2;
    public bool RemoveAfterUsage = false;
    public String Description = "";
    public string AnimatorTrigger = "";
    public float AttackDuration = 2f;
}


public enum AttackTypes { Attack, Defend, Buff, Debuff }