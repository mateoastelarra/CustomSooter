using System.Collections;
using UnityEngine;

public class GunPowerUp : MonoBehaviour, IPowerUp
{
    [SerializeField] private float _powerUpDuration = 15f;
    public void Consume(GameObject gameObject)
    {
        Gun playerGun = gameObject.GetComponentInChildren<Gun>();
        playerGun.ActivateSpecialBullets(_powerUpDuration);
    }

    

}
