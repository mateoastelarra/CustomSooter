using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerUp
{
    public static Action OnPowerUpPickUp;

    void Consume(GameObject gameObject);
}
