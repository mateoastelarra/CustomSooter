using System;
using System.Collections;
using UnityEngine;

public class DiscoballManager : MonoBehaviour
{
    public static Action OnStartParty;
    public static Action OnFinishParty;

    [SerializeField] float _partyTime = 5f;

    private Coroutine _discoPartyRoutine;

    public void DiscoParty()
    {
        if (_discoPartyRoutine != null) { return; }

        _discoPartyRoutine = StartCoroutine(DiscoPartyRoutine());
    }

    private IEnumerator DiscoPartyRoutine()
    {
        OnStartParty?.Invoke();
        yield return new WaitForSeconds(_partyTime);
        OnFinishParty?.Invoke();

        _discoPartyRoutine = null;
    }
}
