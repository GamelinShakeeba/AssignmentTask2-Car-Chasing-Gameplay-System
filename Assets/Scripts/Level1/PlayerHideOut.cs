using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHideOut : MonoBehaviour
{
    
    PoliceChase2 pc;
    public GameObject evasionPanel;
    // Start is called before the first frame update
    void Start()
    {
        evasionPanel.gameObject.SetActive(false);
        pc = GetComponent<PoliceChase2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "hideout")
        {
            StartCoroutine(Evade());
        }
    }

    IEnumerator Evade()
    {
        
        yield return new WaitForSeconds(10f);
        
        foreach (GameObject car in pc.policeCars)
        {
            Destroy(car);
        }
        pc.policeCars.Clear();
        evasionPanel.gameObject.SetActive(true);
        
}
}
