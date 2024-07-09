using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicUIPlayer : MonoBehaviour
{
    public static LogicUIPlayer Instance;

    [Header("======== Stage1 =========")]
    [SerializeField] private Image ImageStage1;
    [SerializeField] private TextMeshProUGUI NameStage1;

    [Header("======== Stage2 =========")]
    [SerializeField] private Image ImageStage2;
    [SerializeField] private TextMeshProUGUI NameStage2;

    [Header("======== Slide Mass ==========")]
    [SerializeField] private Slider EvolutionSlide;
    [SerializeField] private TextMeshProUGUI massText;

    [Header("======== Other ==========")]
    [SerializeField] private TextMeshProUGUI ringText1;
    [SerializeField] private TextMeshProUGUI ringText2;

    private CanvasGroup bgCanvasGroup;
    private Character character;
    private int currentMass;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject Bg;

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
        currentMass = (int)character.rb.mass;
        UpdateInfo();
    }

    private void Update()
    {
        SetTextRing1(character.satellites1.Count, SpawnPlanets.instance.GetMaxOrbit1(character.characterType));
        SetTextRing2(character.satellites2.Count, SpawnPlanets.instance.GetMaxOrbit2(character.characterType));
    }

    public void UpdateInfo()
    {
        SetState1(SpawnPlanets.instance.GetNamePlanet(character.characterType), SpawnPlanets.instance.GetSpritePlanet(character.characterType));
        SetMassTxt((int)character.rb.mass);
        SetState2((character.characterType + 1).ToString(), SpawnPlanets.instance.GetSpritePlanet(character.characterType + 1));
        SetEvoluSlider((long)character.rb.mass - SpawnPlanets.instance.GetRequiredMass(character.characterType),
            SpawnPlanets.instance.GetRequiredMass(character.characterType + 1) - SpawnPlanets.instance.GetRequiredMass(character.characterType));
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
                massText.text = currentMass.ToString();
            });
    }

    public void SetState2(string CharacterType, Sprite image)
    {
        ImageStage2.sprite = image;
        NameStage2.text = CharacterType;
    }

    public void SetState1(string CharacterType, Sprite image)
    {
        ImageStage1.sprite = image;
        NameStage1.text = CharacterType;
    }

    public void SetEvoluSlider(long currentMass, long massNeedeVolution)
    {
        EvolutionSlide.value = (float)currentMass / massNeedeVolution;
    }

    public void SetTextRing1(int intOrbit, int maxOrbit)
    {
        ringText1.text = "1st ring " + intOrbit + "/" + maxOrbit;
    }

    public void SetTextRing2(int intOrbit, int maxOrbit)
    {
        ringText2.text = "2nd ring " + intOrbit + "/" + maxOrbit;
    }

    // animation
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
