using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Lasso/Create EnemyType", order = 1)]
public class EnemyType : ScriptableObject
{
    public string enemyName;
    public float lifetime;
    public int score;
    public EnemyInstance prefab;
}