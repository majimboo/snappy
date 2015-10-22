using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System.Collections;

public class Manager : MonoBehaviour
{
    // snappy
    public GameObject snappy;

    // scoring
    public Text scoreTxt;
    public Text coinTxt;
    public int score = 0;
    public int coins = 0;

    // messages
    public GameObject startGameText;
    public GameObject gameOverText;
    public Text gameOverScoreText;

    // soundefx
    public AudioSource introSoundFx;
    public AudioSource mainThemeFx;
    public AudioSource coinSoundFx;

    // prefabs
    public GameObject cloud;
    public GameObject g_coin;
    public GameObject s_coin;
    public GameObject r_rocket;
    public GameObject b_rocket;
    public GameObject g_rocket;
    public GameObject eagle;

    // sprites
    private Sprite[] clouds;

    // booleans
    public bool gameOver = false;
    private bool gameStart = false;
    private bool spawning = true;

    // ad
    public InterstitialAd interstitial;
    private BannerView bannerView;

    void Awake()
    {
        // load ads
        RequestBanner();
    }

    void Start()
    {
        // loading
        clouds = Resources.LoadAll<Sprite>("Images/clouds");

        // execute the spawner
        StartCoroutine("Spawner");
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-0119739277456732/9607267301";
        string adUnitIdI = "ca-app-pub-0119739277456732/5307518505";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-0119739277456732/6982728107";
        string adUnitIdI = "ca-app-pub-0119739277456732/7702581707";
#else
        string adUnitId = "unexpected_platform";
        string adUnitIdI = "unexpected_platform";
#endif

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        bannerView.LoadAd(new AdRequest.Builder().Build());
        bannerView.Hide();

        interstitial = new InterstitialAd(adUnitIdI);
        interstitial.LoadAd(new AdRequest.Builder().Build());
    }

    void Update()
    {
        // tap back
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();

        // update scores
        scoreTxt.text = GetScore().ToString().PadLeft(6, '0');
        coinTxt.text = GetCoin().ToString().PadLeft(6, '0');

        // detect taps and touches
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 point = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(point), Vector2.zero);

            if (hitInfo && hitInfo.collider.name == "main")
            {
                startGameText.SetActive(false);
                gameStart = true;
                introSoundFx.Stop();
                mainThemeFx.Play();
                snappy.GetComponent<Rigidbody2D>().isKinematic = false;
                snappy.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 250));
            }

            if (hitInfo && hitInfo.collider.name == "play_again")
            {
                bannerView.Hide();
                Application.LoadLevel("Game");
            }
        }
    }

    public void GameOver()
    {
        gameOver = true;
        gameStart = false;
        mainThemeFx.Stop();
        gameOverText.SetActive(true);
        gameOverScoreText.text = score.ToString().PadLeft(6, '0');

        bannerView.Show();
    }

    int GetCoin()
    {
        if (gameStart) return PlayerPrefs.GetInt("coins_collected") + coins;

        return PlayerPrefs.GetInt("coins_collected");
    }

    int GetScore()
    {
        if (gameStart) return score;

        return PlayerPrefs.GetInt("top_score");
    }

    public void AddCoin(int pointer = 1)
    {
        coins = coins + pointer;
    }

    public void AddScore()
    {
        score++;
    }

    IEnumerator Spawner()
    {
        while (spawning)
        {
            // figures speed of spawning
            int delay = Random.Range(2, 4);

            // 90% chance to spawn clouds
            if (Random.Range(1, 100) <= 90) CreateCloud();

            // 60% chance to spawn coins
            if (Random.Range(1, 100) <= 60) CreateCoin();

            // 80% chance to spawn red misiles
            if (Random.Range(1, 100) <= 80) CreateRedMissile();

            // 10% chance to spawn black misiles
            if (Random.Range(1, 100) <= 10) CreateBlackMissile();

            // 50% chance to spawn green misiles
            if (Random.Range(1, 100) <= 50) CreateGreenMissile();

            // 50% chance to spawn eagles if score is above 100
            if (Random.Range(1, 100) <= 50 && score >= 100) CreateEagle();

            yield return new WaitForSeconds(delay);
        }
    }

    void CreateCloud()
    {
        int chosenCloud = Random.Range(0, clouds.Length);
        float[] pRotation = { 0, 180f };

        cloud.GetComponent<SpriteRenderer>().sprite = clouds[chosenCloud];

        float cloudX = Random.Range(7, 10);
        float cloudY = Random.Range(-1, 5);

        Quaternion rRotation = Quaternion.Euler(0, pRotation[Random.Range(0, pRotation.Length)], 0);

        Instantiate(cloud, new Vector3(cloudX, cloudY, 1f), rRotation);
    }

    int CreateCoin()
    {
        if (gameStart == false) return 0;

        GameObject coin = s_coin;

        // 5% chance of gold
        if (Random.Range(1, 100) <= 5) coin = g_coin;

        Vector3 position = new Vector3(Random.Range(4, 10), Random.Range(-4f, 4f), 0);
        // coinSize for detection seems a bit small
        Vector2 coinSize = coin.GetComponent<BoxCollider2D>().size;

        // create if no over lap
        RaycastHit2D hit = Physics2D.BoxCast(position, coinSize, 0f, transform.forward);
        if (!hit.collider)
        {
            Instantiate(coin, position, Quaternion.identity);
        }

        return 1;
    }

    int CreateRedMissile()
    {
        if (gameStart == false) return 0;

        GameObject rocket = r_rocket;
        Instantiate(rocket, new Vector3(Random.Range(5, 10), Random.Range(-4.5f, 4.5f), 0f), Quaternion.identity);

        return 1;
    }

    int CreateGreenMissile()
    {
        if (gameStart == false) return 0;

        GameObject rocket = g_rocket;
        Instantiate(rocket, new Vector3(Random.Range(5, 10), Random.Range(-2.83f, 3.3f), 0f), Quaternion.identity);

        return 1;
    }

    int CreateBlackMissile()
    {
        if (gameStart == false) return 0;

        GameObject rocket = b_rocket;
        Instantiate(rocket, new Vector3(Random.Range(5, 10), Mathf.Clamp(snappy.transform.position.y, -3.88f, 4.3f), 0f), Quaternion.identity);

        return 1;
    }

    int CreateEagle()
    {
        if (gameStart == false) return 0;

        Instantiate(eagle, new Vector3(Random.Range(5, 10), Random.Range(-2.83f, 3.3f), 0f), Quaternion.identity);

        return 1;
    }
}
