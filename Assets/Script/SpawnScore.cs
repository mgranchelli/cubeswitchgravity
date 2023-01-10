using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScore : MonoBehaviour
{
    public Score scorePrefab;
    private float respawnTime = 3.3f;
    private Vector2 screenBounds;
    private readonly float[] randomXPosition = new float[] { (float)-1.8, (float)1.8 };
    private Vector2 position;

    // Use this for initialization
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(EnemyWave());
    }


    private void SpawnScores()
    {

        Score s = Instantiate(scorePrefab);
        s.SetSpeed(GameManager.Instance.objectSpeed);
        Vector2 positionToSpawn = new Vector2(randomXPosition[Random.Range(0, randomXPosition.Length)], screenBounds.y * 2 + 1);
        position = positionToSpawn;
        //print("Position toSpawn: " + positionToSpawn);

        //var hit = Physics2D.OverlapArea(scorePrefab.GetComponent<Renderer>().bounds.min, scorePrefab.GetComponent<Renderer>().bounds.max);


        var hitColliders = Physics2D.OverlapCircle(positionToSpawn, 0.5f);
        if (hitColliders.CompareTag("Obstacle"))
        {
            print("gia presente");
            Destroy(s.gameObject);
        }
        else
            s.transform.position = positionToSpawn;
        if (s.transform.position.x > 0)
            s.GetComponent<Rigidbody2D>().gravityScale *= -1;
    }

    IEnumerator EnemyWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            SpawnScores();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
     //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(position, 0.5f);
    }
}
