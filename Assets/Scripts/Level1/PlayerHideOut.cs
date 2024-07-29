using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHideOut : MonoBehaviour
{
    
    PoliceChase2 pc;
    public GameObject evasionPanel;
    public GameObject evasionTimer;
    float evadeTime = 20f;
    public Image timerImage;

    private float timeRemaining; // Time remaining for the timer
    private bool timerIsRunning = false; // Timer state

    // Start is called before the first frame update
    void Start()
    {
        evasionPanel.gameObject.SetActive(false);
        evasionTimer.gameObject.SetActive(false);
        pc = GetComponent<PoliceChase2>();

        if (timerImage != null)
        {
            timerImage.fillAmount = 1f; // Set full image initially
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerIsRunning = false;
                TimerEnded();
            }
            
            if (timerImage != null)
            {
                timerImage.fillAmount = timeRemaining / evadeTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "hideout")
        {
            StartTimer();
            StartCoroutine(Evade());
        }
    }

    IEnumerator Evade()
    {
        evasionTimer.gameObject.SetActive(true);
        yield return new WaitForSeconds(evadeTime);
        foreach (GameObject car in pc.policeCars)
        {
            Destroy(car);
        }
        pc.policeCars.Clear();
        evasionTimer.gameObject.SetActive(false);
        evasionPanel.gameObject.SetActive(true);
    }

    void StartTimer()
    {
        timeRemaining = evadeTime;
        timerIsRunning = true;
        Debug.Log("Timer has started");
    }

    void TimerEnded()
    {
        // Actions to take when the timer ends
        Debug.Log("Timer ended");
    }
}
