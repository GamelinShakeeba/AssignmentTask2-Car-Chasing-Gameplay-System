using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceMonitor : MonoBehaviour
{
    public GameObject[] countDownItems;
    private CheckpointManager[] carsCPM;
    public static bool racing = false;
    public static int totalLaps = 1;
    public GameObject gameOverPanel;
    public GameObject HUD;

    void Start()
    {
        // Disable countdown items initially
        foreach (GameObject g in countDownItems)
            g.SetActive(false);

        // Start countdown coroutine
        StartCoroutine(PlayCountDown());

        // Hide game over panel initially
        gameOverPanel.SetActive(false);

        // Retrieve both player and NPC cars
        GameObject[] playerCars = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] npcCars = GameObject.FindGameObjectsWithTag("car");

        // Combine both arrays
        List<GameObject> allCars = new List<GameObject>();
        allCars.AddRange(playerCars);
        allCars.AddRange(npcCars);

        // Initialize CheckpointManager array
        carsCPM = new CheckpointManager[allCars.Count];

        for (int i = 0; i < allCars.Count; i++)
        {
            // Ensure each car has a CheckpointManager component
            carsCPM[i] = allCars[i].GetComponent<CheckpointManager>();
            if (carsCPM[i] == null)
            {
                Debug.LogWarning($"No CheckpointManager found on car: {allCars[i].name}");
            }
        }
    }

    IEnumerator PlayCountDown()
    {
        yield return new WaitForSeconds(2);

        foreach (GameObject g in countDownItems)
        {
            g.SetActive(true);
            yield return new WaitForSeconds(1);
            g.SetActive(false);
        }

        racing = true;
    }

    public void RestartLevel()
    {
        racing = false;
        SceneManager.LoadScene(3);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    void LateUpdate()
    {
        int finishedCount = 0;

        // Count cars that have finished the race
        foreach (CheckpointManager cpm in carsCPM)
        {
            if (cpm.lap > totalLaps) // Adjusted to account for completed laps
                finishedCount++;
        }

        if (finishedCount == carsCPM.Length)
        {
            HUD.SetActive(false);
            gameOverPanel.SetActive(true);
        }
    }
}
