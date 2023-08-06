using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadData_ : MonoBehaviour
{
    public Vector3 LoadEnemyPosition()
    {
        float enemyPosX = PlayerPrefs.GetFloat("EnemyPosX", 0f);
        float enemyPosY = PlayerPrefs.GetFloat("EnemyPosY", 0f);
        float enemyPosZ = PlayerPrefs.GetFloat("EnemyPosZ", 0f);

        return new Vector3(enemyPosX, enemyPosY, enemyPosZ);
    }
}
