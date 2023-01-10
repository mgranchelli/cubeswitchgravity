using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    public Obstacle[] obstaclesPrefab;
    private Vector2 screenBounds;
    private float[] randomXPosition = new float[] { (float)-1.86, (float)1.86 };
    Vector2 position;

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(OstacleWave());
    }


    private void SpawnObstacle()
    {
        
        int randomObstacle = Random.Range(0, obstaclesPrefab.Length);
        Obstacle o = Instantiate(obstaclesPrefab[randomObstacle]);
        o.SetSpeed(GameManager.Instance.objectSpeed);
        int randomPosition = Random.Range(0, o.GetObstaclePositionX(o.name).Length);
        Vector2 positionToSpawn = new Vector2(randomXPosition[randomPosition], screenBounds.y * 2);
        position = positionToSpawn;

        var hitColliders = Physics2D.OverlapCircle(positionToSpawn, 1.2f);
        if (hitColliders.CompareTag("Score"))
        {
            print("SCORE GIA presente");
            Destroy(o.gameObject);
        }
        else
            o.transform.position = positionToSpawn;

        if (o.transform.position.x > 0)
        {
            o.transform.eulerAngles = Vector2.down * 180;
            o.GetComponent<Rigidbody2D>().gravityScale *= -1;
        }
            
    }

    IEnumerator OstacleWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameManager.Instance.respawnObstacleTime);
            SpawnObstacle();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(position, 1.2f);
    }
}
