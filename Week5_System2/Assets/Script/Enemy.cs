using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject Shooter;
    public bool IsDestroyed = false;
    public bool IsBoss = true;
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
        /*if (Vector2.Distance(player.transform.position, transform.position) <= 1.5f)
        {
            IsDestroyed = true;
        }*/

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

        GetComponent<SpriteRenderer>().enabled = false;
        DestroyParticle.Play();
        if (shaker)
        {
            shaker.StartShake(0.3f, 1f, 1.5f);
        }
    }

    IEnumerator BigExplosion()
    {
        Time.timeScale = 0;
        player.CanBeHurt = false;
        yield return new WaitForSecondsRealtime(0.2f);

        Time.timeScale = 1;
        player.Score += 5000;
        player.CurrentBladeScore += 5000;

        if (shaker)
        {
            shaker.StartShake(0.5f, 1f, 3f);
        }
        yield return new WaitForSecondsRealtime(0.5f);

        player.CanBeHurt = true;
        GetComponent<SpriteRenderer>().enabled = false;
        DestroyParticle.Play();
    }

}
