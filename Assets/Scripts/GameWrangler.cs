using Obi;
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
    public AudioSource gameSFX;
    public AudioClip castanets, scoreSFX;
    public EnemyLatcher enemyLatcher;
    float nextSpawn;
    int currentWave, capturesThisWave;
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
            if (Time.time > nextSpawn && capturesThisWave < GetWave().quota && enemyInstances.Count < GetWave().maxOnField)
            {
                SpawnEnemy();
            }
            else if(capturesThisWave >= GetWave().quota && enemyInstances.Count == 0)
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
        Debug.Log(randomIndex + " " + enemy.enemyName);
        float x = UnityEngine.Random.Range(-1.0f, 1.0f);
        float z = UnityEngine.Random.Range(-1.0f, 1.0f);
        Vector3 randomDirection = new Vector3(x, 0, z);
        float randomDistance = UnityEngine.Random.Range(1.0f, 7.0f);
        Vector3 offset = (randomDirection * randomDistance);
        offset.x += Math.Sign(offset.x)*2.0f;
        offset.z += Math.Sign(offset.z)*2.0f;
        Vector3 location = transform.position + offset;
        Debug.Log("Spawning " + enemy.enemyName + " at dir=" + randomDirection + " distance=" + randomDistance + " location="+location);
        EnemyInstance newEnemy = Instantiate(enemy.prefab, location, Quaternion.identity);
        enemyInstances.Add(newEnemy);
        newEnemy.InstantiateEnemy(enemy);
    }

    public void RemoveEnemy(EnemyInstance enemy)
    {
        if (enemy.type.name != "StartSign")
        {
            enemyInstances.Remove(enemy);
        }
        GameObject.Destroy(enemy.gameObject);
    }

    public void NewWave()
    {
        currentWave++;
        capturesThisWave = 0;
        waveSignpost.NextWave(currentWave);
        nextSpawn = Time.time + 7;
        gameSFX.clip = castanets;
        gameSFX.Play();
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
            capturesThisWave += 1;

            enemyLatcher.Unlatch();

            gameSFX.clip = scoreSFX;
            gameSFX.Play();
        }
        else if(other.tag == "StartSign")
        {
            EnemyInstance enemy = other.GetComponent<EnemyInstance>();
            enemy.Poof();
            score = 0;
            capturesThisWave = 0;

            StartGame();
            enemyLatcher.Unlatch();

            gameSFX.clip = castanets;
            gameSFX.Play();
        }

    }
}

[System.Serializable]
public class Wave
{
    public int quota, maxOnField;
    public List<EnemyType> enemies;
}