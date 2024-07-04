using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicUIPlayer : MonoBehaviour
{
    public static LogicUIPlayer Instance;

    [Header("0:Shield - 1:Exp - 2:Envolution - 3:Planet - 4:Kill")]
    [SerializeField] private GameObject[] UIinfo;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject Bg;

    [SerializeField] private TextMeshProUGUI numberPlanet;
    [SerializeField] private TextMeshProUGUI numberKill;
    [SerializeField] private TextMeshProUGUI MassText;
    [SerializeField] private TextMeshProUGUI NameTxt;
    [SerializeField] private TextMeshProUGUI EvoluTxt;

    [SerializeField] private Slider EvoluSlider;
    [SerializeField] private Slider EvolutionSlider;
    [SerializeField] private Slider ShieldPlayer;
    [SerializeField] private Slider ExpPlayer;

    [SerializeField] private float MaxExp = 36;
    [SerializeField] private float TimeEvolutionGO = 5f;

    private CanvasGroup bgCanvasGroup;
    private Character character;
    private Shield Shield;
    private bool isEvolutionInProgress = false;
    private int currentMass;

    private void Awake()
    {
        bgCanvasGroup = Bg.GetComponent<CanvasGroup>();
        if (bgCanvasGroup == null)
        {
            bgCanvasGroup = Bg.AddComponent<CanvasGroup>();
        }
    }

    private void Start()
    {
        Instance = this;
        character = player.GetComponent<Character>();
        Shield = character.GetComponent<Shield>();
        OffAllUI();
        currentMass = (int)character.rb.mass;
        UpdateInfo();
    }

    private void Update()
    {
        character.NunmberOrbit = character.satellites.Count;

        if (character.characterType == CharacterType.Asteroid
            || character.characterType == CharacterType.GasGiantPlanet
            || character.characterType == CharacterType.BlackHole
            || character.characterType == CharacterType.SmallPlanet)
        {
            OffAllUI();
        }

        if (character.characterType == CharacterType.LifePlanet && !isEvolutionInProgress)
        {
            OffAllUI();
            StartCoroutine(EvolveOverTime(TimeEvolutionGO));
        }

        if (character.characterType != CharacterType.LifePlanet)
            isEvolutionInProgress = false;

        if (character.generalityType == GeneralityType.Star)
        {
            OffAllUI();
            UIinfo[3].SetActive(true);
            numberPlanet.text = character.NunmberOrbit.ToString() + " / " + character.MaxOrbit.ToString();
        }
        numberKill.text = character.Kill.ToString();

        ExpPlayer.value = character.Kill / MaxExp;
        ShieldPlayer.value = Shield.ShieldPlanet / Shield.MaxShield;
    }

    private IEnumerator EvolveOverTime(float duration)
    {
        UIinfo[2].SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (character.characterType != CharacterType.LifePlanet)
            {
                ResetEvolutionUI();
                yield break;
            }

            elapsedTime += Time.deltaTime;
            EvolutionSlider.value = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        EvolutionSlider.value = 1f;
        UIinfo[0].SetActive(true);
        UIinfo[1].SetActive(true);
        UIinfo[2].SetActive(false);
        UIinfo[4].SetActive(true);

        isEvolutionInProgress = true;
    }

    private void ResetEvolutionUI()
    {
        UIinfo[2].SetActive(false);
        EvolutionSlider.value = 0f;
        isEvolutionInProgress = false;
    }

    private void OffAllUI()
    {
        foreach (GameObject go in UIinfo)
        {
            go.SetActive(false);
        }
        EvolutionSlider.value = 0f;
    }

    public void SetMassTxt(int mass)
    {
        float duration = 0;
        if (mass - currentMass == 1)
            duration = 0;
        else
            duration = 1;


        DOTween.To(() => currentMass, x => currentMass = x, mass, duration)
            .OnUpdate(() =>
            {
                MassText.text = currentMass.ToString();
            });
    }

    public void UpdateInfo()
    {
        SetNameTxt(SpawnPlanets.instance.GetNamePlanet(character.characterType));
        SetMassTxt((int)character.rb.mass);
        SetEvoluTxt((character.characterType + 1).ToString());
        SetEvoluSlider((long)character.rb.mass - SpawnPlanets.instance.GetRequiredMass(character.characterType),
            SpawnPlanets.instance.GetRequiredMass(character.characterType + 1) - SpawnPlanets.instance.GetRequiredMass(character.characterType));
    }

    public void SetEvoluTxt(string CharacterType)
    {
        EvoluTxt.text = "To " + CharacterType;
    }

    public void SetEvoluSlider(long currentMass, long massNeedeVolution)
    {
        EvoluSlider.value = (float)currentMass / massNeedeVolution;
    }

    public void SetNameTxt(string CharacterType)
    {
        NameTxt.text = CharacterType;
    }

    public void BgFadeIn(float time)
    {
        bgCanvasGroup.alpha = 0f;
        Bg.SetActive(true);
        bgCanvasGroup.DOFade(1, time);
    }

    public void BgFadeOut(float time)
    {
        bgCanvasGroup.DOFade(0, time).OnComplete(() => Bg.SetActive(false));
    }
}
