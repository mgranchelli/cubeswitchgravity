using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public Enemy enemyPrefab;
    private float respawnTime = 3f;
    private Vector2 screenBounds;
    private readonly float[] randomXPosition = new float[] { -0.53f, 0, 0.53f  };

    // Use this for initialization
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(EnemyWave());
    }

    private void SpawnEnemies()
    {
        Enemy e = Instantiate(enemyPrefab);
        e.SetSpeed(GameManager.Instance.enemySpeed);
        e.transform.position = new Vector2(randomXPosition[Random.Range(0, randomXPosition.Length)], screenBounds.y * 2);
    }

    IEnumerator EnemyWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            SpawnEnemies();
        }
    }
}
