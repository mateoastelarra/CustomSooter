
using UnityEngine;

public class PowerUpRunner : MonoBehaviour
{
    IPowerUp _curentPowerUp;

    public void UsePowerUp()
    {
        _curentPowerUp.Consume(gameObject);
        IPowerUp.OnPowerUpPickUp?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPowerUp powerUp = collision.GetComponent<IPowerUp>();
        if (powerUp != null)
        {
            Debug.Log("PowerUp");
            _curentPowerUp = powerUp;
            UsePowerUp();
            Destroy(collision.gameObject);
        }
    }
}
