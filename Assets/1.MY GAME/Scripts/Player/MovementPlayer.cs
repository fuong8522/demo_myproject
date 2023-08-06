using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MovementPlayer : MonoBehaviour
{
    public static MovementPlayer instance = null;
    public UnityEngine.UI.Image warning_health;
    public bool checkheal;
    private float blood_previous;
    public bool death;
    private float colorAlpha = 0;
    public Button attackButton;
    public Button changeWeapon = null;
    public Button buttonNextWave = null;
    public HealBar healbar;
    public CinemachineVirtualCamera virtualCamera;

    public int health;
    //Biến liên quan đến di chuyển.
    private CharacterController characterController;
    public float speed = 7;
    public FloatingJoystick joyStick;

    public bool checkPunch = true;

    //Biến liên quan đến tấn công.
    public GameObject uiPunch;

    public Transform cameraManager;
    public Animator animator;

    //Giới hạn trên dưới trái phải.
    private float boundaryLeftRight = 5f;
    private float boundaryUPDown = 0.2f;
    private float boundaryFrontBack = 22f;

    //Biến liên quan đến xoay player theo hướng di chuyển.
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    //Phạm vi tấn công.
    public float attackRange = 50f;
    //Coin
    public int coin;
    public TextMeshProUGUI textCoin;
    //UI Coin
    public Transform CoinNextPos;
    //Sound Coin Collect.
    public bool checkCollect = false;
    //Obstacle
    public GameObject prefabObstacle;
    public GameObject obltacleGreen;
    public bool checkCreate;
    public bool checkRed = false;
    //Traffic Barrier.
    public int barrierAmount;
    public TextMeshProUGUI textBarrier;
    public Button trafficBarrier;
    public Button buttonTrue;
    public GameObject selectTrueFalse;
    public TextMeshProUGUI dayText;
    public int dayNumber = 1;
    public GameObject gameOverPanel;
    public string key_health = "health";
    public string key_coin = "coin";
    public string key_barrierAmount = "barrierAmount";
    public string key_day = "day";

    //Save position enemy.

    public static MovementPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MovementPlayer>();
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

        LoadObstacle();
    }

    public void LoadObstacle()
    {
        int numberCreateSaved = PlayerPrefs.GetInt("lengthOfEnemies");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Obstacle");
        Vector3[] PositionsSaved = LoadEnemyPositions(numberCreateSaved);
        if (enemies.Length == 0)
        {
            for (int i = 0; i < PositionsSaved.Length; i++)
            {
                if (PlayerPrefs.HasKey("Enemy" + i + "PosX"))
                {
                    Instantiate(obltacleGreen, PositionsSaved[i], Quaternion.identity);
                    Debug.Log("check");
                }
                Debug.Log("number");
            }
        }

        enemies = GameObject.FindGameObjectsWithTag("Obstacle");
        for (int i = 0; i < PositionsSaved.Length; i++)
        {
            if (PlayerPrefs.HasKey("Enemy" + i + "PosX"))
                if (enemies[i].transform.IsChildOf(SpawnManager.instance.transform) == false)
                {
                    enemies[i].transform.SetParent(SpawnManager.instance.transform);
                }
        }
    }
    void Start()
    {
        healbar.SetMaxHeal(100);

        dayNumber = 1;
        blood_previous = 100;
        LoadFile();
        SaveFile();
        WarningHealth();
        death = false;
        animator = GetComponentInChildren<Animator>();
        uiPunch = GameObject.Find("DelayImagePunch");
        uiPunch.SetActive(false);
        attackButton = GameObject.Find("ButtonPunch2").GetComponent<Button>();
        attackButton.onClick.AddListener(OnPunchButton);

        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        checkPunch = false;

        MovementPlayer.instance.textBarrier.text = MovementPlayer.instance.barrierAmount.ToString();
    }

    void Update()
    {
        Movement();
        ConstrainMovement();
        CheckAnimationPunch();
        SetHealBar();
        WarningHealth();
        GameOver();
    }

    public void WarningHealth()
    {
        if (health < blood_previous)
        {
            colorAlpha = 0.6f;
            warning_health.color = new Color(warning_health.color.r, warning_health.color.g, warning_health.color.b, colorAlpha);
            blood_previous = health;
        }
        colorAlpha -= Time.deltaTime * 0.3f;
        warning_health.color = new Color(warning_health.color.r, warning_health.color.g, warning_health.color.b, colorAlpha);
    }
    public void ConstrainMovement()
    {
        //Giới hạn trái phải
        if (transform.position.x > boundaryLeftRight)
        {
            transform.position = new Vector3(boundaryLeftRight, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -boundaryLeftRight)
        {
            transform.position = new Vector3(-boundaryLeftRight, transform.position.y, transform.position.z);
        }
        //Giới hạn trên dưới
        if (transform.position.y > -boundaryUPDown)
        {
            transform.position = new Vector3(transform.position.x, -boundaryUPDown, transform.position.z);
        }
        if (transform.position.y < -boundaryUPDown)
        {
            transform.position = new Vector3(transform.position.x, -boundaryUPDown, transform.position.z);
        }
        //Giới hạn trước sau
        if (transform.position.z > 70)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 70);
        }
        if (transform.position.z < -boundaryFrontBack)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -boundaryFrontBack);
        }
    }
    public void Movement()
    {
        Vector3 movement = new Vector3(joyStick.Horizontal, 0, joyStick.Vertical).normalized;

        if (movement.magnitude >= 0.1f)
        {
            OnWalkTrue();
            //Tính góc xoay
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle - 45, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //Xoay theo hướng di chuyển
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle - 45, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * Time.deltaTime * speed);
        }
        else
        {
            OnWalkFalse();
        }
        OnIdle();
    }

    public void OnPunchButton()
    {
        FindEnemy();
        animator.SetTrigger("Punch");
        StartCoroutine(OnOffAnimationZombie());
    }

    public void OnIdle()
    {
        animator.SetBool("Idle", true);
    }
    public void OnWalkTrue()
    {
        animator.SetBool("Walk", true);
    }

    public void OnWalkFalse()
    {
        animator.SetBool("Walk", false);
    }
    IEnumerator OnOffAnimationZombie()
    {
        uiPunch.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        uiPunch.SetActive(false);
    }

    void CheckAnimationPunch()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("m_melee_combat_attack_A") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1)
        {
            checkPunch = true;
        }
        else
        {
            checkPunch = false;
        }
    }

    void FindEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50);
        foreach (Collider collider in hitColliders)
        {
            if (collider.tag == "Enemy")
            {
                transform.forward = collider.transform.position - transform.position;
            }
        }
    }


    public void SetHealBar()
    {
        healbar.SetHeal(health);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            Debug.Log("check health");
        }
    }

    public void UpdateCoin(int xCoin)
    {
        coin += xCoin;
    }
    public void UpdateUiCoin()
    {
        textCoin.text = coin + "";
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            checkCollect = true;
            UpdateCoin(1);
            UpdateUiCoin();
        }
    }

    //Function create obstacle.
    public void CreateGreenObstacle()
    {
        if (barrierAmount != 0)
        {
            Vector3 playerPosition = transform.position;
            Vector3 playerForward = transform.forward;

            // Xác định khoảng cách từ player đến đối tượng bạn muốn tạo
            float distanceFromPlayer = 3f; // Điều chỉnh khoảng cách tùy ý

            // Tính toán vị trí của đối tượng mới dựa trên vị trí và hướng nhìn của player
            Vector3 objectPosition = playerPosition + playerForward * distanceFromPlayer;

            // Tạo đối tượng mới tại vị trí đã tính toán
            Instantiate(prefabObstacle, objectPosition, Quaternion.identity);
            checkCreate = false;

            MovementPlayer.instance.barrierAmount--;
            MovementPlayer.instance.textBarrier.text = MovementPlayer.instance.barrierAmount.ToString();
        }
    }

    public void SetCheckCreate()
    {
        checkCreate = true;
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("Obstacle");
        Vector3[] enemiesPosition = new Vector3[enemies2.Length];
        for (int i = 0; i < enemies2.Length; i++)
        {
            enemiesPosition[i] = enemies2[i].transform.position;
        }
        SaveEnemyPositions(enemiesPosition);
        PlayerPrefs.SetInt("lengthOfEnemies", enemies2.Length);
        PlayerPrefs.Save();
        Debug.Log(enemies2.Length);
    }

    public void GameOver()
    {
        if (health < 0)
        {
            gameOverPanel.SetActive(true);
            Destroy(this.gameObject);
        }
    }
    public void GameOverInPause()
    {
        Destroy(this.gameObject);
    }

    public void SaveFile()
    {
        PlayerPrefs.SetInt(key_health, health);
        PlayerPrefs.SetInt(key_coin, coin);
        PlayerPrefs.SetInt(key_barrierAmount, barrierAmount);
        PlayerPrefs.SetInt(key_day, dayNumber);
    }
    public void LoadFilenewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt(key_health, 100);
        PlayerPrefs.SetInt(key_coin, 0);
        PlayerPrefs.SetInt(key_barrierAmount, 2);
        PlayerPrefs.SetInt(key_day, 1);
        PlayerPrefs.Save();
    }

    public void LoadFile()
    {
        health = PlayerPrefs.GetInt(key_health);
        coin = PlayerPrefs.GetInt(key_coin);
        barrierAmount = PlayerPrefs.GetInt(key_barrierAmount);
        dayNumber = PlayerPrefs.GetInt(key_day);

        textCoin.text = coin + "";
        textBarrier.text = barrierAmount.ToString();
        dayText.text = "Day " + dayNumber;
        PlayerPrefs.Save();
    }
    /*    public void SaveEnemyPosition()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Obstacle");
            for (int i = 0; i < enemies.Length; i++)
            {
                Debug.Log("i");
            }
            Debug.Log("checksave");
        }*/



    public void SaveEnemyPositions(Vector3[] enemyPositions)
    {
        for (int i = 0; i < enemyPositions.Length; i++)
        {
            PlayerPrefs.SetFloat($"Enemy{i}PosX", enemyPositions[i].x);
            PlayerPrefs.SetFloat($"Enemy{i}PosY", 0);
            PlayerPrefs.SetFloat($"Enemy{i}PosZ", enemyPositions[i].z);
        }
        PlayerPrefs.Save();
    }
    public Vector3[] LoadEnemyPositions(int numberOfEnemies)
    {
        Vector3[] enemyPositions = new Vector3[numberOfEnemies];
        for (int i = 0; i < numberOfEnemies; i++)
        {
            float enemyPosX = PlayerPrefs.GetFloat($"Enemy{i}PosX", enemyPositions[i].x);
            float enemyPosY = PlayerPrefs.GetFloat($"Enemy{i}PosY", 0f);
            float enemyPosZ = PlayerPrefs.GetFloat($"Enemy{i}PosZ", enemyPositions[i].z);

            enemyPositions[i] = new Vector3(enemyPosX, enemyPosY, enemyPosZ);
        }
        return enemyPositions;
    }

}
