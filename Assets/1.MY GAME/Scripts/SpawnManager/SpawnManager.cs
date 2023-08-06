using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance = null;

    public static SpawnManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpawnManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject[] zombiePrefabs;
    public int timeDelay = 13;
    public float timeDelayCountDown = 0;
    public bool check = true;
    public int countWave;
    public Coroutine spawnzombie = null;
    public Button buttonNextWave;
    public Button buttonContinute;
    public Button buttonContinuteShop;
    public GameObject rewardUI = null;
    public GameObject ShopUI = null;
    public bool countCoint = false;
    public int x = 0;
    public TextMeshProUGUI timeSpawn;
    public TextMeshProUGUI coin;

    //UI Shop.
    public TextMeshProUGUI coinShop;
    public TextMeshProUGUI hpShop;
    public TextMeshProUGUI dayShop;

    public Button buyHp;
    public Button buySpeed;
    public Button buyBarrier;
    void Start()
    {
        timeDelay = 13;
        countWave = 0;
        spawnzombie = StartCoroutine(DelaySpawnZombie());
    }

     
    void Update()
    {
        SpawnWave();
        timeSpawn.text = "Time: " + (int)timeDelayCountDown;
        timeDelayCountDown -= Time.deltaTime;
        if (timeDelayCountDown < 0)
        {
            timeDelay = 0;
        }
        if (countCoint && x < 51 && rewardUI.gameObject.activeSelf == true)
        {

            coin.text = "$" + x.ToString();
            x += 1;
        } 

        if(MovementPlayer.instance == null)
        {
            Destroy(this.gameObject);
        }

        if(MovementPlayer.instance.health > 100)
        {
            MovementPlayer.instance.health = 100;
            hpShop.text = MovementPlayer.instance.health.ToString();
        }
    }

    public void SpawnWave()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Enemy");
        int zombieCount = coins.Length;

        if (zombieCount == 0 && check)
        {
            if (countWave < 1)
            {
                buttonNextWave.gameObject.SetActive(true);
                spawnzombie = StartCoroutine(DelaySpawnZombie());
                timeDelayCountDown = timeDelay;
            }
            else
            {
                check = false;
                StartCoroutine(DelayRewardUi());
                buttonContinute.onClick.AddListener(NextShop);
                buttonContinuteShop.onClick.AddListener(NextScene);
                buyBarrier.onClick.AddListener(BuyBarrier);
                buySpeed.onClick.AddListener(BuySpeed);
                buyHp.onClick.AddListener(BuyHp);
            }
        }

    }

    public void BuyBarrier()
    {
        if(MovementPlayer.instance.coin > 10)
        {
            MovementPlayer.instance.coin -= 10;
            MovementPlayer.instance.barrierAmount += 2;
            coinShop.text = MovementPlayer.instance.coin.ToString();


        }
        Debug.Log("barrier");
    }
    public void BuyHp()
    {
        if (MovementPlayer.instance.coin > 10)
        {
            if(MovementPlayer.instance.health < 100)
            {
                MovementPlayer.instance.coin -= 10;
                MovementPlayer.instance.health += 10;
            }
            else
            {
                MovementPlayer.instance.health = 100;
            }
            hpShop.text = MovementPlayer.instance.health.ToString();
            coinShop.text = MovementPlayer.instance.coin.ToString();

        }
        Debug.Log("hp");

    }
    public void BuySpeed()
    {
        if (MovementPlayer.instance.coin > 10)
        {
            MovementPlayer.instance.coin -= 10;
            MovementPlayer.instance.speed += 1;
            coinShop.text = MovementPlayer.instance.coin.ToString();

        }
        Debug.Log("speed");

    }

    public void NextWave()
    {
        if (spawnzombie != null)
        {
            StopCoroutine(spawnzombie);
            spawnzombie = null;
            SpawnZombie();
        }
    }
    public IEnumerator DelaySpawnZombie()
    {
        check = false;
        timeDelayCountDown = timeDelay;
        yield return new WaitForSeconds(timeDelay);
        SpawnZombie();
    }

    public IEnumerator DelayRewardUi()
    {
        yield return new WaitForSeconds(2);
        rewardUI.SetActive(true);
        
        countCoint = true;
    }
    public void DestroySpawnInPause()
    {
        Destroy(this.gameObject);
    }
    public void NextScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        MovementPlayer.instance.dayNumber++;
        MovementPlayer.instance.SaveFile();
    }
    public void NextShop()
    {
        MovementPlayer.instance.UpdateCoin(x - 1);
        MovementPlayer.instance.UpdateUiCoin();
        x = 0;

        rewardUI.SetActive(false);
        ShopUI.SetActive(true);
        coinShop.text = MovementPlayer.instance.coin.ToString();
        hpShop.text = MovementPlayer.instance.health.ToString();
        dayShop.text = "Day " + (SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SpawnZombie()
    {
        buttonNextWave.gameObject.SetActive(false);
        check = true;
        countWave++;

        int numberofwave = MovementPlayer.instance.dayNumber;
        for (int i = 0; i <= numberofwave; i++)
        {
        Vector3 spawnPos = new Vector3(Random.Range(-5, 5), 0, MovementPlayer.instance.transform.position.z + Random.RandomRange(19, 35));
            int zombieIndex = Random.Range(0, zombiePrefabs.Length);
            Instantiate(zombiePrefabs[zombieIndex], spawnPos, zombiePrefabs[zombieIndex].transform.rotation);
        }
    }
}
