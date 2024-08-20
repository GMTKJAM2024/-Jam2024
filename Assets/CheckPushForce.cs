using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPushForce : MonoBehaviour
{
    public float initialPushForce = 10f; // Initial force applied when the object enters the trigger
    public float forceIncreaseRate = 50f; // Rate at which the force increases

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has a PufferFishController component
        PufferFishController pufferFish = other.GetComponent<PufferFishController>();

        if (pufferFish != null && pufferFish.popping)
        {
            ApplyPushForce(other, initialPushForce);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the other object has a PufferFishController component
        PufferFishController pufferFish = other.GetComponent<PufferFishController>();

        if (pufferFish != null && pufferFish.popping)
        {
            // Increase the force over time as long as the object remains within the trigger
            float addedForce = forceIncreaseRate * Time.deltaTime;
            ApplyPushForce(other, addedForce);
        }
    }

    private void ApplyPushForce(Collider other, float force)
    {
        
        // Apply an upward force to the object
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log("Push");
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
    }
}
