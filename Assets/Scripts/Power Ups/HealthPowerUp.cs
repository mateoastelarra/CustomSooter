using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour, IPowerUp
{  
    public void Consume(GameObject gameObject)
    {
        Health playerHealth = gameObject.GetComponent<Health>();
        playerHealth.CurrentHealth += 1;

        PlayerUI playerUI = gameObject.GetComponent<PlayerUI>();
        playerUI.UpdateHealthText();
    }
}
