﻿using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

    private float speed;
    public Manager manager;

    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        speed = Random.Range(1.4f, 2.5f);
    }

    void Update()
    {
        Vector3 pos = transform.position;
        if (manager.firePower)
            pos.x -= speed + manager.speedo * Time.deltaTime;
        else
            pos.x -= speed * Time.deltaTime;
        transform.position = pos;

        if (transform.position.x <= -7.0f) Remove();
    }

    void Remove()
    {
        Destroy(gameObject);
        Destroy(this);
    }
}
