using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceEnds : MonoBehaviour
{
    public List<Collider> playerCars = new List<Collider>();
    public List<Collider> npcCars = new List<Collider>();
    private bool raceEnded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            if (raceEnded) return; // Prevent multiple triggers

            raceEnded = true; // Stop the race
        }
    }

    public void RegisterCar(Collider car)
    {
        if (car.CompareTag("Player"))
        {
            if (!playerCars.Contains(car))
                playerCars.Add(car);
        }
        else if (car.CompareTag("car"))
        {
            if (!npcCars.Contains(car))
                npcCars.Add(car);
        }
    }
}
