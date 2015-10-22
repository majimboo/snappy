using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

    private float speed;

    void Start()
    {
        speed = Random.Range(1.4f, 2.5f);
    }

    void Update()
    {
        Vector3 pos = transform.position;
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
