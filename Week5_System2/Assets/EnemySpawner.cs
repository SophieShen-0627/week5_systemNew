using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] Transform FollowObject;
    [SerializeField] GameObject NormalEnemy;
    [SerializeField] GameObject BossEnemy;
    [SerializeField] float MovingSpeed = 2;
    private Vector3 dir;

    [SerializeField] List<Transform> Targets = new List<Transform>();
    private Transform[] TargetPositions;
    private bool[] HasEnemy;
    private GameObject[] Enemies;
    private int CurrentIndex = 0;
    [SerializeField] float timeToSpawn = 6;

    private PlayerCollider player;

    private void Awake()
    {
        dir = transform.position - FollowObject.position;
    }

    void Start()
    {

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
        transform.position = FollowObject.position + dir;

        if (player.AddBlade)
        {
            StartCoroutine(SpawnBoss());
            player.AddBlade = false;
        }
    }

    IEnumerator SpawnBoss()
    {
        yield return new WaitForSecondsRealtime(5);

        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] != null)
            {
                if (Enemies[i].GetComponent<Enemy>().IsBoss == false)
                {
                    Enemies[i].GetComponent<Enemy>().IsDestroyed = true;
                    Enemies[i] = null;
                    HasEnemy[i] = false;
                }
            }
        }

        Vector3 Pos = GetPosition().position;
        Enemies[CurrentIndex] = Instantiate(BossEnemy, transform.position, Quaternion.identity);

        StartCoroutine(MoveObject(Enemies[CurrentIndex], TargetPositions[CurrentIndex]));
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
        for (int i = 0; i < HasEnemy.Length; i++)
        {
            if (HasEnemy[i] == false)
            {
                CurrentIndex = i;
                HasEnemy[i] = true;
                return TargetPositions[i];
            }
        }

        return transform;
    }
}
