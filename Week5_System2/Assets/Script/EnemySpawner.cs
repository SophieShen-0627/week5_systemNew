using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] Transform FollowObject;
    [SerializeField] GameObject NormalEnemy;
    [SerializeField] GameObject BossEnemy;
    [SerializeField] float MovingSpeed = 2;
    [SerializeField] Image BossWarning;
    private Vector3 dir;

    [SerializeField] List<Transform> Targets = new List<Transform>();
    private Transform[] TargetPositions;
    private bool[] HasEnemy;
    private GameObject[] Enemies;
    private int CurrentIndex = 0;
    [SerializeField] float timeToSpawn = 6;

    private PlayerCollider player;
    private int BossCountDown = 5;
    private bool SpawnBossByTimeCount = false;

    private void Awake()
    {
        dir = transform.position - FollowObject.position;
    }

    void Start()
    {
        BossWarning.gameObject.SetActive(false);
        TargetPositions = Targets.ToArray();

        HasEnemy = new bool[Targets.Count];
        Enemies = new GameObject[Targets.Count];
        for (int i = 0; i < HasEnemy.Length; i++) HasEnemy[i] = false;

        player = FindObjectOfType<PlayerCollider>();

        StartCoroutine(SpawnNormalEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfEnemyIsDestroyed();
        //transform.position = FollowObject.position + dir;

        if (player.AddBlade)
        {
            StartCoroutine(SpawnBoss());
            player.AddBlade = false;
        }

        if (timeToSpawn <= 2 && !SpawnBossByTimeCount)
        {
            StartCoroutine(SpawnBossByTime());
            SpawnBossByTimeCount = true;
        }

        InstantSpawnEnemyWhenEmpty();
    }

    IEnumerator SpawnBossByTime()
    {
        yield return new WaitForSecondsRealtime(4);
        StartCoroutine(SpawnBoss());
    }
    private void InstantSpawnEnemyWhenEmpty()
    {
        bool HasNoEnemyOnScreen = false;
        for (int i = 0; i < HasEnemy.Length; i++)
        {
            if (HasEnemy[i])
            {
                HasNoEnemyOnScreen = false;
                break;
            }
            else HasNoEnemyOnScreen = true;
        }

        if (HasNoEnemyOnScreen)
        {
            if (timeToSpawn > 2) timeToSpawn -= 1f;

            Transform Pos = GetPosition();
            if (Pos != transform)
            {
                Enemies[CurrentIndex] = Instantiate(NormalEnemy, transform.position, Quaternion.identity);
                StartCoroutine(MoveObject(Enemies[CurrentIndex], Pos));
            }
        }
    }

    private void CheckIfEnemyIsDestroyed()
    {
        for (int i = 0; i < Targets.Count; i++)
        {
            if (Enemies[i] != null && Enemies[i].GetComponent<Enemy>().IsDestroyed)
            {
                Enemies[i] = null;
                HasEnemy[i] = false;
            }
        }
    }
    IEnumerator SpawnBoss()
    {
        BossCountDown = 5;
        StartCoroutine(SpawnBossCounter());
        yield return new WaitForSecondsRealtime(5);

        BossWarning.gameObject.SetActive(false);
        Transform Pos = GetPosition();
        if (Pos == transform)
        {
            int randomDestroy = Random.Range(0, Enemies.Length);
            if (Enemies[randomDestroy].GetComponent<Enemy>().IsBoss == false)
            {
                Enemies[randomDestroy].GetComponent<Enemy>().IsDestroyed = true;
                Enemies[randomDestroy] = null;
                HasEnemy[randomDestroy] = false;
            }
        }

        Transform PosNew = GetPosition();
        if (PosNew != transform)
        {
            Enemies[CurrentIndex] = Instantiate(BossEnemy, transform.position, Quaternion.identity);

            StartCoroutine(MoveObject(Enemies[CurrentIndex], PosNew));
        }

    }

    IEnumerator SpawnBossCounter()
    {
        if (BossCountDown > 0)
        {
            BossCountDown -= 1;
            GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.BossSpawn);

            BossWarning.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(.4f);
            BossWarning.gameObject.SetActive(false);

            yield return new WaitForSecondsRealtime(.6f);
            StartCoroutine(SpawnBossCounter());
        }
    }

    IEnumerator SpawnNormalEnemy()
    {
        Transform Pos = GetPosition();
        if (Pos != transform)
        {
            Enemies[CurrentIndex] = Instantiate(NormalEnemy, transform.position, Quaternion.identity);
            StartCoroutine(MoveObject(Enemies[CurrentIndex], Pos));
        }

        yield return new WaitForSeconds(timeToSpawn);
        StartCoroutine(SpawnNormalEnemy());
    }

    IEnumerator MoveObject(GameObject enemy, Transform target)
    {
        yield return new WaitForEndOfFrame();

        if (Vector3.Distance(enemy.transform.position, target.position) > 0.1f)
        {
            enemy.transform.position += (target.position - enemy.transform.position) * Time.deltaTime * MovingSpeed;
            StartCoroutine(MoveObject(enemy, target));
        }
    }

    private Transform GetPosition()
    {
        int start = Random.Range(0, TargetPositions.Length);

        for (int i = 0; i < HasEnemy.Length; i++)
        {
            int currentIndex = start + i;
            if (currentIndex >= TargetPositions.Length) currentIndex = currentIndex % TargetPositions.Length;
            
            if (HasEnemy[currentIndex] == false)
            {
                CurrentIndex = currentIndex;
                HasEnemy[currentIndex] = true;
                return TargetPositions[currentIndex];
            }
        }

        return transform;
    }
}
