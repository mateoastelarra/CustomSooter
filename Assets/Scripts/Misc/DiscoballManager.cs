using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DiscoballManager : MonoBehaviour
{
    public static Action OnStartParty;
    public static Action OnFinishParty;

    [SerializeField] float _partyTime = 5f;
    [SerializeField] Light2D _globalLight;
    [SerializeField] float _partyLightIntensity = 0.5f;

    private Coroutine _discoPartyRoutine;
    private float _defaultLightIntensity;

    private void Start()
    {
        _defaultLightIntensity = _globalLight.intensity;
    }

    private void OnEnable()
    {
        OnStartParty += TurnDownGlobalLighting;
        OnFinishParty += TurnUpGlobalLighting;
    }

    private void OnDisable()
    {
        OnStartParty -= TurnDownGlobalLighting;
        OnFinishParty -= TurnUpGlobalLighting;
    }

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

    private void TurnDownGlobalLighting()
    {
        _globalLight.intensity = _partyLightIntensity;
    }

    private void TurnUpGlobalLighting()
    {
        _globalLight.intensity = _defaultLightIntensity;
    }
}
