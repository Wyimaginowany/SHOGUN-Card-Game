using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyGroup", menuName = "Group/New Group", order = 2)]
public class EnemyGroupsScriptableObject : ScriptableObject
{
    public SingleStageGroup[] enemyGroups;
}

[System.Serializable]
public class SingleStageGroup
{
    public GameObject[] EnemiesOnStage;
}
