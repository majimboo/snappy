using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public int pointer = 1;

    private Manager manager;
    private float speed = 1.5f;

    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x -= speed * Time.deltaTime;
        transform.position = pos;

        if (transform.position.x <= -7f) Remove();

        // self destruct on gameover
        if (manager.gameOver) Remove();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            manager.coinSoundFx.Play();
            manager.AddCoin(pointer);

            Remove();
        }
    }

    void Remove()
    {
        Destroy(gameObject);
        Destroy(this);
    }
}
