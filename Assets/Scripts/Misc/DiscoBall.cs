using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : MonoBehaviour, IHitable
{
    private Flash _flash;
    private DiscoballManager _discoballManager;

    private void Awake()
    {
        _flash = GetComponent<Flash>();
        _discoballManager = GetComponent<DiscoballManager>();
    }
    public void TakeHit()
    {
        _flash.StartFlash();
        _discoballManager.DiscoParty();
    }

}
