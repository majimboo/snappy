using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public float speed = 4f;

    public Manager manager;

    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    void Update()
    {
        Vector3 pos = transform.position;
        if (manager.firePower)
            pos.x -= speed + manager.speedo * Time.deltaTime;
        else
            pos.x -= speed * Time.deltaTime;
        transform.position = pos;

        if (transform.position.x <= -3.5f)
        {
            manager.AddScore();
            Remove();
        }

        // self destruct on gameover
        if (manager.gameOver) Remove();
    }

    void Remove()
    {
        Destroy(gameObject);
        Destroy(this);
    }
}
