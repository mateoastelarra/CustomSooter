using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSplatterHandler : MonoBehaviour
{
    private void OnEnable()
    {
        Health.OnDeath += SpawnDeathSplatterPrefab;
        Health.OnDeath += SpawnDeathVFXPrefab;
    }

    private void OnDisable()
    {
        Health.OnDeath -= SpawnDeathSplatterPrefab;
        Health.OnDeath -= SpawnDeathVFXPrefab;
    }

    public void SpawnDeathSplatterPrefab(Health sender)
    {
        GameObject newSplatterPrefab = Instantiate(sender.SplatterPrefab, sender.transform.position, sender.transform.rotation);
        SpriteRenderer splatterSR = newSplatterPrefab.GetComponent<SpriteRenderer>();
        ColorChanger enemyColorChanger = sender.GetComponent<ColorChanger>();
        splatterSR.color = enemyColorChanger.DefaultColor;
        newSplatterPrefab.transform.SetParent(this.transform);
    }

    public void SpawnDeathVFXPrefab(Health sender)
    {
        GameObject newDeathVFXPrefab = Instantiate(sender.DeathVFXPrefab, sender.transform.position, sender.transform.rotation);
        ParticleSystem.MainModule psMainModule = newDeathVFXPrefab.GetComponent<ParticleSystem>().main;
        ColorChanger enemyColorChanger = sender.GetComponent<ColorChanger>();
        psMainModule.startColor = enemyColorChanger.DefaultColor;
        newDeathVFXPrefab.transform.SetParent(this.transform);
    }
}
