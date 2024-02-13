using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] float rotationSpeed = 50f; // 旋转速度，度/秒
    private float angle = 0f; // 当前角度
    private float radius = 2f; // 旋转半径

    void Start()
    {
        if (!player) player = GetComponentInParent<PlayerCollider>().transform;

        radius = Vector2.Distance(player.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            player = GetComponentInParent<Transform>();
        }

        transform.right = player.position - transform.position;

        angle -= rotationSpeed * Time.deltaTime;

        angle %= 360;

        float x = player.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float y = player.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        transform.position = new Vector3(x, y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>())
        {
            collision.GetComponent<Bullet>().IsDestroyed = true;
            if (player.GetComponent<PlayerCollider>() && !player.GetComponent<PlayerCollider>().IsDead)
            {
                player.GetComponent<PlayerCollider>().Score += 200;
                player.GetComponent<PlayerCollider>().CurrentBladeScore += 200;
            }
        }

        if (collision.GetComponent<Enemy>())
        {
            collision.GetComponent<Enemy>().IsDestroyed = true;
        }
    }
}
