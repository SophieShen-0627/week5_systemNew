using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] List<Vector2> DeathPos = new List<Vector2>();
    [SerializeField] GameObject Blade;
    [SerializeField] ParticleSystem DeathParticle;
    [SerializeField] TMP_Text ScoreText;
    [SerializeField] TMP_Text LifeText;
    public bool IsDead = false;
    private Vector2 InitialPos;

    public int BladeNum = 1;
    public int LifeNum = 9;
    public int Score = 0;

    [SerializeField] float ScoreForAnotherBlade = 1000;
    public float CurrentBladeScore = 0;
    public bool AddBlade = false;                         //this is for enemy spawner to spawn boss;
    private float NextBladeDistance = 2.5f;
    private List<GameObject> ExtraBlade = new List<GameObject>();

    public bool CanBeHurt = true;

    private CameraShake shaker;

    private void Awake()
    {
        InitialPos = transform.position;
    }
    void Start()
    {
        shaker = FindObjectOfType<CameraShake>();
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
        if (IsDead) DoDeadPlayerSetting();

        if (LifeNum <= 0) GameEndState.instance.GameEnd = true;
        else
        {
            ScoreText.text = Score.ToString();
            LifeText.text = LifeNum.ToString();
        }


        if (CurrentBladeScore >= ScoreForAnotherBlade)
        {
            GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.SpawnAnotherBlade);
            AddBlade = true;
            CurrentBladeScore -= ScoreForAnotherBlade;
            GameObject temp = Instantiate(Blade, transform.position + transform.right * NextBladeDistance, Quaternion.identity, transform);

            NextBladeDistance += 0.5f;
            ScoreForAnotherBlade *= 2;

            ExtraBlade.Add(temp);
        }
    }

    private void DoDeadPlayerSetting()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1);
        GetComponent<PlayerMove>().enabled = false;
        Blade[] blades = GetComponentsInChildren<Blade>();
        foreach (var blade in blades)
        {
            blade.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        GetComponent<PlayerCollider>().enabled = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Bullet>() && CanBeHurt)
        {
            StartCoroutine(HitStop());
            LifeNum -= 1;

        }
    }

    IEnumerator HitStop()
    {
        Time.timeScale = 0;
        AudioManager.instance.GetComponent<AudioSource>().Stop();
        yield return new WaitForSecondsRealtime(.4f);

        Time.timeScale = 1;
        AudioManager.instance.GetComponent<AudioSource>().Play();


        GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.PlayerExplode);
        shaker.StartShake(0.5f, 1f, 4f);
        DeathParticle.Play();

        yield return new WaitForSecondsRealtime(0.5f);

        DeathPos.Add(transform.position);
        Vector2 Pos = transform.position;

        GameObject clone = Instantiate(gameObject, transform.position, Quaternion.identity);
        clone.GetComponent<PlayerCollider>().IsDead = true;


        ResetPlayerPos();

    }

    private void ResetPlayerPos()
    {
        StartCoroutine(Inevitable());

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

    }

    IEnumerator Inevitable()
    {
        CanBeHurt = false;
        yield return new WaitForSeconds(2);

        CanBeHurt = true;
    }
}
