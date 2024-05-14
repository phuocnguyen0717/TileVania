using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickup;
    [SerializeField] int pointForPickup;
    bool wasCollected;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().AddToScore(pointForPickup);
            AudioSource.PlayClipAtPoint(coinPickup, Camera.main.transform.position);
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
