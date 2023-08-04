using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class Loaddata : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public Image image;
    void Start()
    {
        cam = GameObject.Find("Isometric Camera").GetComponent<CinemachineVirtualCamera>();
        if (cam.Follow == null)
        {
            cam.Follow = MovementPlayer.instance.transform;
        }

        if(MovementPlayer.instance.uiPunch == null)
        {
        MovementPlayer.instance.uiPunch = GameObject.Find("DelayImagePunch");
        MovementPlayer.instance.uiPunch.SetActive(false);
         image = GameObject.Find("HealWarning").GetComponent<Image>();

        }
        if (MovementPlayer.instance.warning_health == null)
        {
            MovementPlayer.instance.warning_health = image;
            MovementPlayer.instance.healbar = GameObject.Find("HealBar").GetComponent<HealBar>(); 
            MovementPlayer.instance.SetHealBar();
        }
        if (MovementPlayer.instance.joyStick == null)
        {
            MovementPlayer.instance.joyStick = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();
        MovementPlayer.instance.transform.position = new Vector3(0,0,0);
        }
        if (MovementPlayer.instance.attackButton == null)
        {
            MovementPlayer.instance.attackButton = GameObject.Find("ButtonPunch2").GetComponent<Button>();
            MovementPlayer.instance.attackButton.onClick.AddListener(MovementPlayer.instance.OnPunchButton);
        }
        if (MovementPlayer.instance.changeWeapon == null)
        {
            MovementPlayer.instance.changeWeapon = GameObject.Find("ButtonChangeWeapon").GetComponent<Button>();
            MovementPlayer.instance.changeWeapon.onClick.AddListener(GameObject.Find("R_hand_container").GetComponent<WeaponSwitching>().OnOffSwitchButton);
        }
        if (MovementPlayer.instance.buttonNextWave == null)
        {
            MovementPlayer.instance.buttonNextWave = GameObject.Find("ButtonNextWave").GetComponent<Button>();
            MovementPlayer.instance.buttonNextWave.onClick.AddListener(SpawnManager.instance.NextWave);
        }


        if (SpawnManager.instance.buttonContinute == null)
        {
            SpawnManager.instance.buttonContinute = GameObject.Find("Continute").GetComponent<Button>();
        }
        if (SpawnManager.instance.buttonContinuteShop == null)
        {
            SpawnManager.instance.buttonContinuteShop = GameObject.Find("ContinuteNextLevel").GetComponent<Button>();
        }
        //get component button continute.
        if (SpawnManager.instance.buttonNextWave == null)
        {
            SpawnManager.instance.buttonNextWave = GameObject.Find("ButtonNextWave").GetComponent<Button>();
            SpawnManager.instance.timeSpawn = GameObject.Find("Time: ").GetComponent<TextMeshProUGUI>();
        }
        //get component barrier UI text.
        if (MovementPlayer.instance.textBarrier == null)
        {
            MovementPlayer.instance.textBarrier = GameObject.Find("BarrierAmount").GetComponent<TextMeshProUGUI>();
            MovementPlayer.instance.textBarrier.text = MovementPlayer.instance.barrierAmount.ToString();


        }
        //get component text coin.
        if (MovementPlayer.instance.textCoin == null)
        {
            MovementPlayer.instance.textCoin = GameObject.Find("textCoin").GetComponent<TextMeshProUGUI>();
            MovementPlayer.instance.textCoin.text = MovementPlayer.instance.coin + "";
        }
        //get component coin next position.
/*        if (MovementPlayer.instance.CoinNextPos == null)
        {
            MovementPlayer.instance.CoinNextPos = GameObject.Find("Coin").GetComponent<Transform>();
        }*/
        //get component coint UI.
        if (true)
        {
            SpawnManager.instance.coin = GameObject.Find("$50").GetComponent<TextMeshProUGUI>();
            SpawnManager.instance.x = 1;
        }
        //get component coint UI shop.
        if (SpawnManager.instance.coinShop == null || SpawnManager.instance.hpShop == null || SpawnManager.instance.dayShop == null)
        {
            SpawnManager.instance.coinShop = GameObject.Find("amountCoinShop").GetComponent<TextMeshProUGUI>();
            SpawnManager.instance.hpShop = GameObject.Find("percentHpshop").GetComponent<TextMeshProUGUI>();
            SpawnManager.instance.dayShop = GameObject.Find("dayshop").GetComponent<TextMeshProUGUI>();
        }
        //get component Button UI shop.
        if (SpawnManager.instance.buyHp == null || SpawnManager.instance.buySpeed == null || SpawnManager.instance.buyBarrier == null)
        {
            SpawnManager.instance.buyHp = GameObject.Find("BuyHP").GetComponent<Button>();
            SpawnManager.instance.buyBarrier = GameObject.Find("BuyBarrier").GetComponent<Button>();
            SpawnManager.instance.buySpeed = GameObject.Find("BuySpeed").GetComponent<Button>();

        }

        //get component reware UI.
        if (SpawnManager.instance.rewardUI == null)
        {
            SpawnManager.instance.rewardUI = GameObject.Find("Reward");
            SpawnManager.instance.rewardUI.SetActive(false);
        }
        //get component reware UI.
        if (SpawnManager.instance.ShopUI == null)
        {
            SpawnManager.instance.ShopUI = GameObject.Find("Shop");
            SpawnManager.instance.ShopUI.SetActive(false);
            SpawnManager.instance.coinShop.text = MovementPlayer.instance.coin.ToString();
        }

        if (SpawnManager.instance.spawnzombie == null)
        {
            SpawnManager.instance.countWave = 0;
            SpawnManager.instance.check = true;
            //SpawnManager.instance.spawnzombie = StartCoroutine(SpawnManager.instance.DelaySpawnZombie());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
