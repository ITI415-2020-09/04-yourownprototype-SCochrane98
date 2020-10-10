using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            PinpointPinball.LivesGained();
        }
    }
}
