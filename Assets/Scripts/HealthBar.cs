using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider healthSlider;
    public TMP_Text healthBarText;
   Damageable playerDamageable;


    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null )
        {
            Debug.Log("No player found in the scene. Make sure it has tag 'Player'");
        }
        playerDamageable = player.GetComponent<Damageable>();
    }
    void Start()
    {
       
        healthSlider.value = CalculateSliderPercantage(playerDamageable.Health,playerDamageable.MaxHealth);
        healthBarText.text = "HP " + playerDamageable.Health + " / " + playerDamageable.MaxHealth;
    }

    private void OnEnable()
    {
        playerDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }
     
    private void OnDisable()
    {
        playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private float CalculateSliderPercantage(float currenntHealth , float maxHealth)
    {
        return currenntHealth / maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercantage(newHealth, maxHealth);
        healthBarText.text = "HP " + newHealth + " / " + maxHealth;
    }
}
