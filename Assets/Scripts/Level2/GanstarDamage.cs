using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GanstarDamage : MonoBehaviour
{
    int maxHealth = 50;
    int currentHealth;
    int damage = 10;
    public Text enemyHealthtxt;
    public GameObject ps;
    public GameObject gameOverPanel;
    void Start()
    {
        currentHealth = maxHealth;
        ps.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            currentHealth -= damage;
        }
        enemyHealthtxt.text = currentHealth.ToString();
        
        Debug.Log("Enemy Current Health = " + currentHealth);
        if(currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die() 
    {
        ps.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
        
        gameOverPanel.gameObject.SetActive(true);
}


}
