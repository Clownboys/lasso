using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWrangler : CollabXR.SingletonBehavior<GameWrangler>
{
    public enum GameState { Loading, Lobby, Started, Ended };
    public GameState state = GameState.Loading;
    public int score, highScore;
    public List<Wave> waves;
    public WaveSignpost waveSignpost;
    public ParticleSystem poofParticle;
    float nextSpawn;
    int currentWave, spawnsThisWave;
    List<EnemyInstance> enemyInstances;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        ChangeState(GameState.Lobby);
    }

    public void StartGame()
    {
        enemyInstances = new List<EnemyInstance>();
        ChangeState(GameState.Started);
    }

    public void EndGame()
    {
        ChangeState(GameState.Ended);
    }

    private void ChangeState(GameState newState)
    {
        MusicWrangler.Instance.CheckState(this.state, newState);
        this.state = newState;
    }

    private void Update()
    {
        if(state == GameState.Lobby)
        {

        }
        else if(state == GameState.Started)
        {
            if (Time.time > nextSpawn && spawnsThisWave < GetWave().spawns)
            {
                SpawnEnemy();
            }
            else if(spawnsThisWave >= GetWave().spawns && enemyInstances.Count == 0)
            {
                NewWave();
            }
        }
    }

    private void SpawnEnemy()
    {
        nextSpawn = Time.time + 5;
        int randomIndex = (int) Mathf.Floor(UnityEngine.Random.Range(0, GetWave().enemies.Count));
        EnemyType enemy = GetWave().enemies[randomIndex];
        Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0, UnityEngine.Random.Range(-1.0f, 1.0f));
        float randomDistance = UnityEngine.Random.Range(3.0f, 10.0f);
        Vector3 location = transform.position + (randomDirection * randomDistance);
        Debug.Log("Spawning " + enemy.enemyName + " at dir=" + randomDirection + " distance=" + randomDistance + " location="+location);
        EnemyInstance newEnemy = Instantiate(enemy.prefab, location, Quaternion.identity);
        enemyInstances.Add(newEnemy);
        newEnemy.InstantiateEnemy(enemy);
        spawnsThisWave++;
    }

    public void RemoveEnemy(EnemyInstance enemy)
    {
        enemyInstances.Remove(enemy);
        GameObject.Destroy(enemy.gameObject);
    }

    public void NewWave()
    {
        currentWave++;
        spawnsThisWave = 0;
        waveSignpost.NextWave(currentWave);
        nextSpawn = Time.time + 10;
    }

    public Wave GetWave()
    {
        if (currentWave < waves.Count)
        {
            return waves[currentWave];
        }
        return waves[waves.Count - 1];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyInstance enemy = other.GetComponent<EnemyInstance>();
            enemy.Poof();
            score += enemy.type.score;
        }

    }
}

[System.Serializable]
public class Wave
{
    public int spawns;
    public List<EnemyType> enemies;
}