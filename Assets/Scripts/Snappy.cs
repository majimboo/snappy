using UnityEngine;
using System.Collections;

public class Snappy : MonoBehaviour {

    private Vector2 jumpForce = new Vector2(0, 350);
    private Manager manager;

    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce(jumpForce);
        }

        // out of bounds
        if (transform.localPosition.y <= -740) Die();
    }

    void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.collider.tag == "Enemy") Die();
    }

    void Die()
    {
        int topScore = PlayerPrefs.GetInt("top_score");
        int coinsCollected = PlayerPrefs.GetInt("coins_collected");

        // new top score
        if (topScore < manager.score)
        {
            PlayerPrefs.SetInt("top_score", manager.score);

            if (manager.interstitial.IsLoaded() && manager.score < 50)
            {
                manager.interstitial.Show();
            }
        }
            
        PlayerPrefs.SetInt("coins_collected", coinsCollected + manager.coins);

        manager.GameOver();
        Remove();
    }

    void Remove()
    {
        Destroy(gameObject);
        Destroy(this);
    }

}
