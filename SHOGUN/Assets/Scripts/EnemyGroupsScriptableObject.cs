using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyGroup", menuName = "Group/New Group", order = 2)]
public class EnemyGroupsScriptableObject : ScriptableObject
{
    public SingleStageGroup[] enemyGroups;
}

[System.Serializable]
public class SingleStageGroup
{
    public SingleStagePossibleGroup[] PossibleEnemyGroups;
}

[System.Serializable]
public class SingleStagePossibleGroup
{
    public GameObject[] EnemiesOnStage;
}
