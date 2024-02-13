using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip BGM;
    public AudioClip PlayerExplode;                //player collider
    public AudioClip BossExplode;                   // enemy
    public AudioClip SmallEnemyExplode;             //enemy
    public AudioClip BossSpawn;                    //enemy spawner
    public AudioClip BulletExplode;                 //blade
    public AudioClip SpawnAnotherBlade;             //player
    public AudioClip LoseSound;                     //win state controller

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GetComponent<AudioSource>().clip = BGM;
        GetComponent<AudioSource>().Play();
    }
}
