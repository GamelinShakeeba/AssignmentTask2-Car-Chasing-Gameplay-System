using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChase : MonoBehaviour
{
    int currentWantedLevel;
    public GameObject imgLevel1;
    public GameObject imgLevel2;
    public GameObject imgLevel3;

    public GameObject policeCarPrefab; // Reference to the police car prefab

    private List<GameObject> policeCars = new List<GameObject>();
    private GameObject policeHelicopter;

    public GameObject playerTarget;
    int speed = 20;
    void Start()
    {
        currentWantedLevel = 0;
        imgLevel1.gameObject.SetActive(false);
        imgLevel2.gameObject.SetActive(false);
        imgLevel3.gameObject.SetActive(false);
    }

    void Update()
    {
        //UpdatePoliceUnits();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "npc")
        {
            currentWantedLevel++;
        }
        Debug.Log(currentWantedLevel);

        if(currentWantedLevel == 1)
        {
            imgLevel1.gameObject.SetActive(true);
            UpdatePoliceUnits();
        }

        if (currentWantedLevel == 2)
        {
            imgLevel2.gameObject.SetActive(true);
            UpdatePoliceUnits();
            UpdatePoliceUnits();
        }

        if (currentWantedLevel == 3)
        {
            imgLevel3.gameObject.SetActive(true);
            UpdatePoliceUnits();
            UpdatePoliceUnits();
            UpdatePoliceUnits();
        }

        
    }

    void UpdatePoliceUnits()
    {
        // Clear previous police units
        foreach (var car in policeCars)
        {
            Destroy(car);
        }
        policeCars.Clear();
        SpawnPoliceCar();
    }

        void SpawnPoliceCar()
    {
        GameObject newPoliceCar = Instantiate(policeCarPrefab, playerTarget.transform.position + Vector3.back * 10f, Quaternion.identity);
        policeCars.Add(newPoliceCar);

        //newPoliceCar.transform.position = Vector3.MoveTowards(newPoliceCar.transform.position, playerTarget.transform.position + Vector3.back * 10f, speed);
    }
}
