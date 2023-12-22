using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePowerUp : MonoBehaviour, IPowerUp
{
    public void Consume(GameObject gameObject)
    {
        Gun playerGun = gameObject.GetComponentInChildren<Gun>();
        playerGun.CurrentGrenades += 1;

        PlayerUI playerUI = gameObject.GetComponent<PlayerUI>();
        playerUI.UpdateGrenadesText();
    }
}
