using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCarController : MonoBehaviour
{
    public Transform playerTarget; // Reference to the player's Transform
    public float moveSpeed = 10f; // Move speed of the police car
    public float stoppingDistance = 2f; // Distance at which the car stops near the player
    public float evadeDistance = 10f; // Distance to keep from hideout areas

    void Update()
    {
        // Check if near a hideout area and adjust behavior
        if (IsNearHideoutArea())
        {
            // Implement behavior to evade or avoid hideout areas
            EvadeHideout();
        }
        else
        {
            // Normal behavior: Follow the player
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        // Move towards the player
        Vector3 moveDirection = (playerTarget.position - transform.position).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Rotate towards the player
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    void EvadeHideout()
    {
        // Implement behavior to evade hideout areas, for example:
        Vector3 moveDirection = (transform.position - playerTarget.position).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    bool IsNearHideoutArea()
    {
        // Check if there are any hideout areas nearby using Physics.OverlapSphere
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, evadeDistance);
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("hideout"))
            {
                return true; // Found a hideout area nearby
            }
        }
        return false; // No hideout area found nearby
    }
}
