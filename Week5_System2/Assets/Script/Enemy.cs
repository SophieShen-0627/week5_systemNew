using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject Shooter;
    public bool IsDestroyed = false;
    [SerializeField] bool IsBoss = true;
    [SerializeField] CameraShake shaker;
    [SerializeField] ParticleSystem DestroyParticle;
    private PlayerCollider player;

    private bool HasPlayed = false;

    private void OnEnable()
    {
        player = FindObjectOfType<PlayerCollider>();
        if (!shaker) shaker = FindObjectOfType<CameraShake>();
    }
    private void Update()
    {
        if (IsDestroyed)
        {
            if (!HasPlayed)
            {
                if (Shooter) Shooter.SetActive(false);
                HasPlayed = true;
                GetComponent<Collider2D>().enabled = false;
                if (IsBoss)
                {
                    StartCoroutine(BigExplosion());
                    Time.timeScale = 0;
                }
                else
                {
                    StartCoroutine(NormalExplosion());
                }
            }
        }
    }

    IEnumerator NormalExplosion()
    {
        yield return new WaitForEndOfFrame();
        player.Score += 500;
        player.CurrentBladeScore += 500;

        if (shaker)
        {
            shaker.StartShake(0.15f, 1f, 1.5f);
        }
    }

    IEnumerator BigExplosion()
    {
        player.CanBeHurt = false;
        yield return new WaitForSecondsRealtime(0.5f);

        Time.timeScale = 1;
        player.Score += 5000;
        player.CurrentBladeScore += 5000;

        if (shaker)
        {
            shaker.StartShake(0.6f, 2.5f, 2);
        }
        yield return new WaitForSecondsRealtime(0.5f);

        player.CanBeHurt = true;
        GetComponent<SpriteRenderer>().enabled = false;
        DestroyParticle.Play();
    }

}
