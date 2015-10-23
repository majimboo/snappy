﻿using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {
    private Manager manager;
    private float speed = 1.5f;

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

        if (transform.position.x <= -7f) Remove();

        // self destruct on gameover
        if (manager.gameOver) Remove();

        // remove power up if already have
        if (manager.rainPower || manager.firePower) Remove();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") Remove();
    }

    void Remove()
    {
        Destroy(gameObject);
        Destroy(this);
    }
}
