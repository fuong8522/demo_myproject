using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public void SaveEnemyPosition(Vector3 enemyPosition)
    {
        PlayerPrefs.SetFloat("EnemyPosX", enemyPosition.x);
        PlayerPrefs.SetFloat("EnemyPosY", enemyPosition.y);
        PlayerPrefs.SetFloat("EnemyPosZ", enemyPosition.z);
        PlayerPrefs.Save();
    }
}
