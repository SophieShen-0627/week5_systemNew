using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] List<Vector2> DeathPos = new List<Vector2>();
    [SerializeField] GameObject DeadBody;
    [SerializeField] GameObject Blade;

    private Vector2 InitialPos;

    public int BladeNum = 1;
    public int LifeNum = 9;
    public int Score = 0;

    [SerializeField] float ScoreForAnotherBlade = 1000;
    public float CurrentBladeScore = 0;
    public bool AddBlade = false;                         //this is for enemy spawner to spawn boss;
    private float NextBladeDistance = 4f;
    private List<GameObject> ExtraBlade = new List<GameObject>();

    public bool CanBeHurt = true;

    private void Awake()
    {
        InitialPos = transform.position;
    }
    void Start()
    {
        ResetPlayerPos();
        StartCoroutine(AddScoreTime());
    }

    IEnumerator AddScoreTime()
    {
        yield return new WaitForSeconds(1);
        Score += 10;
        CurrentBladeScore += 10;

        StartCoroutine(AddScoreTime());
    }
    // Update is called once per frame
    void Update()
    {
        if (LifeNum <= 0) GameEndState.instance.GameEnd = true;

        if (CurrentBladeScore >= ScoreForAnotherBlade)
        {
            AddBlade = true;
            CurrentBladeScore -= ScoreForAnotherBlade;
            GameObject temp = Instantiate(Blade, transform.position + transform.right * NextBladeDistance, Quaternion.identity, transform);

            NextBladeDistance += 2f;
            ScoreForAnotherBlade *= 2;

            ExtraBlade.Add(temp);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Bullet>() && CanBeHurt)
        {
            LifeNum -= 1;
            DeathPos.Add(transform.position);
            Vector2 Pos = transform.position;
            ResetPlayerPos();

            Instantiate(DeadBody, Pos, Quaternion.identity);
        }
    }


    private void ResetPlayerPos()
    {
        transform.position = InitialPos;
        BladeNum = 1;
        Score = 0;
        CurrentBladeScore = 0;

        if (ExtraBlade.Count != 0)
        {
            foreach (var temp in ExtraBlade)
            {
                Destroy(temp);
            }
        }
        ExtraBlade = new List<GameObject>();
        CanBeHurt = false;

        StartCoroutine(Inevitable());
    }

    IEnumerator Inevitable()
    {
        yield return new WaitForSeconds(2);

        CanBeHurt = true;
    }
}
