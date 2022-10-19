using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public PointsScript PS;
    public AudioClip coinSound;

    private void Start()
    {
        PS = GameObject.Find("Score").GetComponent<PointsScript>();
    }

    void Update()
    {
        transform.Rotate(90 * Time.deltaTime, 0, 0);       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "PlayerObj")
        {
            PS.points += 10;
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
            Destroy(gameObject);
        }
    }
}
