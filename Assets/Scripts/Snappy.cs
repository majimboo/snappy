using UnityEngine;
using System.Collections;

public class Snappy : MonoBehaviour {

    public GameObject bubble_wrap;
    public AudioSource powerUpRainFx;
    public AudioSource powerUpFireFx;

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

        // maintain top position
        var pos = transform.position;
        pos.y = Mathf.Clamp(transform.position.y, -640.0f, 650.0f);
        transform.position = pos;

        // out of bounds
        if (transform.localPosition.y <= -740) Die();
    }

    void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.collider.tag == "Enemy" && manager.rainPower == false && manager.firePower == false) Die();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "RainPower")
        {
            manager.rainPower = true;
            bubble_wrap.SetActive(true);
            powerUpRainFx.Play();
            StartCoroutine("endPowerRainUp");
        }

        if (other.tag == "FirePower")
        {
            manager.firePower = true;
            powerUpFireFx.Play();
            StartCoroutine("endPowerFireUp");
        }
    }

    IEnumerator endPowerRainUp()
    {
        yield return new WaitForSeconds(10);
        manager.rainPower = false;
        bubble_wrap.SetActive(false);
    }

    IEnumerator endPowerFireUp()
    {
        yield return new WaitForSeconds(4);
        manager.firePower = false;
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
        // Destroy(this);
    }

}
