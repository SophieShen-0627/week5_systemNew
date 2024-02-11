using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool IsDestroyed = false;
    [SerializeField] ParticleSystem destroy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDestroyed)
        {
            destroy.Play();
            gameObject.SetActive(false);
        }
    }
}
