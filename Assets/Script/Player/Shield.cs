using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private GameObject ShieldObject;
    [SerializeField] private GameObject ShieldMiniCam;
    [SerializeField] private float TimeToFullRecharge = 10f;
    [SerializeField] private float TimePlanetNotAllowTakeDam = 5f;
    public float ShieldPlanet;
    public float MaxShield = 100f;
    public bool TakeDamage;

    private float lastDamageTime, rechargeRate;
    private bool isRecharging;
    private Coroutine rechargeCoroutine;
    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (character.characterType != CharacterType.LifePlanet)
        {
            ShieldPlanet = 0;
            TakeDamage = false;
            lastDamageTime = 0f;
            isRecharging = false;

            if (rechargeCoroutine != null)
            {
                StopCoroutine(rechargeCoroutine);
            }
        }

        if (ShieldPlanet > 0 && character.characterType == CharacterType.LifePlanet && character.EvolutionDone)
        {
            if (ShieldObject != null)
                ShieldObject.SetActive(true);
            if (ShieldMiniCam != null)
                ShieldMiniCam.SetActive(true);
        }
        else if (ShieldPlanet <= 0 || character.characterType != CharacterType.LifePlanet)
        {
            if (ShieldObject != null)
                ShieldObject.SetActive(false);
            if (ShieldMiniCam != null)
                ShieldMiniCam.SetActive(false);
        }


        if (TakeDamage)
        {
            lastDamageTime = Time.time;
            TakeDamage = false;

            if (isRecharging)
            {
                StopCoroutine(rechargeCoroutine);
                isRecharging = false;
                ShieldPlanet = 0;
            }
        }

        HandleShieldRecharge();

    }

    private void HandleShieldRecharge()
    {
        if (character.EvolutionDone && ShieldPlanet <= 0)
        {
            if (!isRecharging && Time.time - lastDamageTime >= TimePlanetNotAllowTakeDam)
            {
                rechargeCoroutine = StartCoroutine(WaitAndRecharge());
            }
        }
    }

    private IEnumerator WaitAndRecharge()
    {
        isRecharging = true;

        while (Time.time - lastDamageTime < TimePlanetNotAllowTakeDam)
        {
            yield return null;
        }

        rechargeRate = MaxShield / TimeToFullRecharge;
        while (ShieldPlanet < MaxShield)
        {
            ShieldPlanet += rechargeRate * Time.deltaTime;
            if (ShieldPlanet > MaxShield)
            {
                ShieldPlanet = MaxShield;
            }
            yield return null;
        }

        isRecharging = false;
    }
}
