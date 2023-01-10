using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSpawnCollider : MonoBehaviour
{
    public GameObject positionSpawn;
    public GameObject prefab;
    private int position = 8;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(prefab, positionSpawn.transform.position, Quaternion.identity);

        }
    }



}
