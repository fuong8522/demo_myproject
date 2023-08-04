using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPause()
    {
        Time.timeScale = 0f;
    }
    public void SetResume()
    {
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        Destroy(MovementPlayer.instance);
        Destroy(SpawnManager.instance);
    }
    GameObject obltacleGreen;
    public void destroyObstacleGreen()
    {
        obltacleGreen = GameObject.Find("road-barrier-003 green(Clone)");
        Destroy(obltacleGreen);
        MovementPlayer.instance.barrierAmount++;
    }
}
