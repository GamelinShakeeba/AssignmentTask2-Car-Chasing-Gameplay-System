using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChase2 : MonoBehaviour
{
    public GameObject playerTarget; // Reference to the player's GameObject or Transform
    public GameObject policeCarPrefab; // Reference to the police car prefab

    public GameObject imgLevel1;
    public GameObject imgLevel2;
    public GameObject imgLevel3;

    private int currentWantedLevel = 0;
    public List<GameObject> policeCars = new List<GameObject>();

    void Start()
    {
        imgLevel1.SetActive(false);
        imgLevel2.SetActive(false);
        imgLevel3.SetActive(false);
    }

    void Update()
    {
        // Example: You may want to add logic here for reducing wanted level over time or based on player actions
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "npc")
        {
            currentWantedLevel++;
            Debug.Log("Wanted Level: " + currentWantedLevel);

            // Update UI indicators and spawn police units based on current wanted level
            UpdateUIAndPoliceUnits();
        }
    }

    void UpdateUIAndPoliceUnits()
    {
        // Update UI indicators based on current wanted level
        imgLevel1.SetActive(currentWantedLevel >= 1);
        imgLevel2.SetActive(currentWantedLevel >= 2);
        imgLevel3.SetActive(currentWantedLevel >= 3);

        // Spawn or despawn police units based on current wanted level
        UpdatePoliceUnits();
    }

    void UpdatePoliceUnits()
    {
        // Despawn existing police cars
        DespawnPoliceCars();

        // Spawn police units based on current wanted level
        switch (currentWantedLevel)
        {
            case 1:
                SpawnPoliceCars(1);
                break;
            case 2:
                SpawnPoliceCars(1); // Example: Spawn 1 police cars for wanted level 2
                break;
            case 3:
                SpawnPoliceCars(1); // Example: Spawn 1 police cars for wanted level 3
                break;
            default:
                break;
        }
    }

    void SpawnPoliceCars(int numberOfCars)
    {
        for (int i = 0; i < numberOfCars; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition(i, numberOfCars);
            GameObject newPoliceCar = Instantiate(policeCarPrefab, spawnPosition, Quaternion.identity);
            policeCars.Add(newPoliceCar);

            // Attach PoliceCarController script to the spawned police car
            PoliceCarController carController = newPoliceCar.GetComponent<PoliceCarController>();
            if (carController == null)
            {
                carController = newPoliceCar.AddComponent<PoliceCarController>();
            }

            // Assign the player target's Transform component to the PoliceCarController
            carController.playerTarget = playerTarget.transform; // Assign player target's Transform

            carController.moveSpeed = 10f; // Adjust move speed as needed
            carController.stoppingDistance = 2f; // Adjust stopping distance as needed
        }
    }

    void DespawnPoliceCars()
    {
        foreach (var car in policeCars)
        {
            Destroy(car);
        }
        policeCars.Clear();
    }

    Vector3 CalculateSpawnPosition(int index, int numberOfCars)
    {
        // Calculate spawn positions based on index and number of cars
        Vector3 offset = Vector3.zero;

        switch (numberOfCars)
        {
            case 1:
                offset = new Vector3(0f, 0.5f, -10f); // Single car directly behind
                break;
            case 2:
                offset = (index == 0) ? new Vector3(-5f, 0.5f, -10f) : new Vector3(5f, 0.5f, -10f); // Two cars, left and right
                break;
            case 3:
                if (index == 0)
                    offset = new Vector3(-5f, 0.5f, -10f); // Left car
                else if (index == 1)
                    offset = new Vector3(5f, 0.5f, -10f); // Right car
                else if (index == 2)
                    offset = new Vector3(0f, 0.5f, 10f); // Behind car
                break;
        }

        return playerTarget.transform.position + offset;
    }
}
