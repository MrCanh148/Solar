using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Asteroid = 0,
    SmallPlanet = 1,
    LifePlanet = 2,
    GasGiantPlanet = 3,
    SmallStar = 4,
    MediumStar = 5,
    BigStar = 6,
    NeutronStar = 7,
    BlackHole = 8,
    BigCrunch = 9,
    BigBang = 10,

};

public enum GeneralityType
{
    Asteroid = 0,
    Planet = 1,
    Star = 2,
    BlackHole = 3,
};

public class Character : MonoBehaviour
{
    public CharacterType characterType;
    public GeneralityType generalityType;
    public Rigidbody2D rb;
    public Transform tf;
    public Vector2 velocity;
    public Vector2 externalVelocity;
    public Vector2 mainVelocity;
    public bool isPlayer;
    public bool isDead;
    public bool canControl;
    public bool isBasicReSpawn;
    public SpriteRenderer spriteRenderer;
    [SerializeField] GameObject canvar;
    public List<Character> satellites;
    public CircleCollider2D circleCollider2D;

    [Header("========= Orbit =========")]
    public Character host;
    public float radius; // Bán kính quỹ đạo
    public float spinSpeed; // Tốc độ quay
    public float angle;
    public bool isCapture;
    public LineRenderer lineRenderer;
    public int NunmberOrbit;
    public int MaxOrbit;

    [Header("======= Other =======")]
    public int Kill;
    public bool EvolutionDone = false;
    public Character myFamily;
    public bool isSetup;
    private float x, y;
    private Vector2 direction, tmp, dirVeloc;
    private Vector3 contactPoint;
    [SerializeField] private GameObject BlackHold;

    protected virtual void Start()
    {

    }

    private void OnEnable()
    {
        if (!isSetup)
            OnInit();
    }

    protected virtual void OnInit()
    {
        AllWhenDie();
        myFamily = this;
        isCapture = false;
        host = null;
    }

    private void Update()
    {
        if (isCapture)
        {
            lineRenderer.enabled = true;
            x = Mathf.Cos(angle) * radius;
            y = Mathf.Sin(angle) * radius;

            // Cập nhật vị trí của đối tượng
            tf.position = host.tf.position + new Vector3(x, y, 0f);
            angle += spinSpeed * Time.deltaTime;
        }
        else
            lineRenderer.enabled = false;

        if (host != null && tf != null && lineRenderer.enabled == true)
        {
            lineRenderer.SetPosition(1, tf.position);
            lineRenderer.SetPosition(0, host.tf.position);

            myFamily = host.myFamily;
        }

    }

    protected virtual void FixedUpdate()
    {
        velocity.x -= velocity.x * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
        velocity.y -= velocity.y * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
        velocity = new Vector2(velocity.x, velocity.y);
        mainVelocity = velocity + externalVelocity;

        if (this.host == null)
            rb.velocity = mainVelocity;
        else
        {
            rb.velocity = Vector2.zero;
            direction = host.tf.position - tf.position;

            if (spinSpeed >= 0f)
            {
                if (tf.position.y >= host.tf.position.y)
                    tmp = new Vector2(-spinSpeed, 0);
                else
                    tmp = new Vector2(spinSpeed, 0);
            }
            else
            {
                if (tf.position.y >= host.tf.position.y)
                    tmp = new Vector2(spinSpeed, 0);
                else
                    tmp = new Vector2(-spinSpeed, 0);
            }

            dirVeloc = CalculateProjection(tmp, direction);
            velocity = dirVeloc.normalized * spinSpeed;
        }


        if (characterType == CharacterType.Asteroid)
            tf.Rotate(Vector3.forward, 100 * Time.deltaTime);
        else
            tf.rotation = Quaternion.identity;

        if (canvar != null)
            canvar.transform.rotation = Quaternion.identity;

        if (characterType != CharacterType.LifePlanet || !gameObject.activeSelf)
        {
            Kill = 0;
            EvolutionDone = false;
        }

        if (generalityType == GeneralityType.Star)
        {
            if (characterType == CharacterType.SmallStar)
                MaxOrbit = 3;
            else if (characterType == CharacterType.MediumStar)
                MaxOrbit = 5;
            else if (characterType == CharacterType.BigStar)
                MaxOrbit = 7;
            else if (characterType == CharacterType.NeutronStar)
                MaxOrbit = 9;
        }
        NunmberOrbit = satellites.Count;

        if (characterType == CharacterType.BlackHole)
            BlackHold.SetActive(true);
        else
            BlackHold.SetActive(false);
    }

    //=================================== VA CHAM DAN HOI ============================================ 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        contactPoint = collision.contacts[0].point;
        Character character = collision.gameObject.GetComponent<Character>();
        if (character == null)
            return;
        HandleCollision2(character);
    }

    public void HandleCollision2(Character character)
    {
        float gravitational = (mainVelocity - character.mainVelocity).magnitude;
        if (generalityType == GeneralityType.Asteroid && character.generalityType == GeneralityType.Asteroid)
        {
            if (gravitational <= GameManager.instance.status.minimumMergeForce)
            {
                Vector2 velocity = (2 * rb.mass * mainVelocity + (character.rb.mass - rb.mass) * character.mainVelocity) / (rb.mass + character.rb.mass);
                character.velocity = new Vector2(velocity.x, velocity.y);
                character.ResetExternalVelocity();
                AudioManager.instance.PlaySFX("Hit");
                VfxManager.instance.PlanetHitVfx(contactPoint, transform.rotation);
            }
            else
            {
                if (this.isPlayer)
                {
                    MergeCharacter(this, character);
                    Vector2 velocityS = (character.rb.mass * character.velocity + rb.mass * velocity) / (rb.mass + character.rb.mass);
                    velocity = new Vector2(velocityS.x, velocityS.y);
                }
                else if (this.GetInstanceID() > character.GetInstanceID())
                {
                    MergeCharacter(this, character);
                    Vector2 velocityS = (character.rb.mass * character.velocity + rb.mass * velocity) / (rb.mass + character.rb.mass);
                    velocity = new Vector2(velocityS.x, velocityS.y);
                }
            }
            return;
        }

        if (generalityType == GeneralityType.BlackHole)
        {
            if (character.generalityType == GeneralityType.BlackHole)
            {
                if (this.rb.mass > character.rb.mass)
                {
                    MergeCharacter(this, character);
                    SpawnPlanets.instance.ActiveCharacter2(character);
                    SpawnPlanets.instance.quantityPlanetActive++;
                }

            }
            else
            {
                MergeCharacter(this, character);
                if (character.characterType > CharacterType.Asteroid)
                {
                    SpawnPlanets.instance.ActiveCharacter2(character);
                    SpawnPlanets.instance.quantityPlanetActive++;
                }
            }
            return;
        }

        if (characterType != character.characterType || characterType == character.characterType)
        {
            if (characterType == CharacterType.Asteroid)
            {
                character.rb.mass -= (int)rb.mass;
                SoundAndVfxDie();
                SpawnPlanets.instance.ActiveCharacter(this, characterType);
                AllWhenDie();
            }

            else
            {
                VfxManager.instance.PlanetHitVfx(contactPoint, transform.rotation);
                AudioManager.instance.PlaySFX("Hit");
                character.rb.mass -= SpawnPlanets.instance.GetRequiredMass(characterType + 1) / 10;
            }

            Vector2 velocity = (2 * rb.mass * mainVelocity + (character.rb.mass - rb.mass) * character.mainVelocity) / (rb.mass + character.rb.mass);

            character.velocity = new Vector2(velocity.x, velocity.y);
            character.ResetExternalVelocity();
        }
    }

    public void MergeCharacter(Character c1, Character c2)
    {
        AudioManager.instance.PlaySFX("Eat");
        c1.rb.mass += c2.rb.mass;
        c2.AllWhenDie();
        SpawnPlanets.instance.DeActiveCharacter(c2);
    }

    protected virtual void ResetExternalVelocity()
    {
        externalVelocity = Vector2.zero;
    }

    public void AbsorbCharacter(Character host, Character character)
    {
        if (character.satellites.Count <= 0)
        {
            DOTween.To(() => character.radius, x => character.radius = x, 2.5f * host.circleCollider2D.radius * SpawnPlanets.instance.GetScalePlanet(host.characterType), 0.3f)
           .OnComplete(() =>
           {
               SpawnPlanets.instance.DeActiveCharacter(character);
               host.satellites.Remove(character);
               character.lineRenderer.enabled = false;
               ResetRadiusSatellite(host);
           })
           .Play();
            host.rb.mass += character.rb.mass;
        }
        else
        {
            Character supCharacter = character.GetCharacterWithMinimumMass();
            if (supCharacter != null)
            {
                AbsorbCharacter(character, supCharacter);
            }
        }
    }

    public Character GetCharacterWithMinimumMass()
    {
        Character character = null;
        if (satellites.Count > 0)
        {
            character = satellites[0];

            foreach (var c in satellites)
            {
                if (c.rb.mass < character.rb.mass)
                {
                    character = c;
                }
            }
        }
        return character;
    }

    public Character GetCharacteHaveSatellite()
    {
        Character character = null;
        if (satellites.Count > 0)
        {
            character = satellites[0];

            foreach (var c in satellites)
            {
                if (c.satellites.Count > character.satellites.Count)
                {
                    character = c;
                }
            }
        }
        return character;

    }

    private Vector2 CalculateProjection(Vector2 v1, Vector2 v2)
    {
        // Tính phép chiếu vector v1 lên v2
        Vector2 projV1OnV2 = Vector2.Dot(v1, v2.normalized) * v2.normalized;

        // Tính vector vuông góc với v2
        Vector2 orthogonalV2 = new Vector2(-v2.y, v2.x);

        // Tính phép chiếu vector v1 lên vector vuông góc với v2
        Vector2 projV1OnOrthogonalV2 = Vector2.Dot(v1, orthogonalV2.normalized) * orthogonalV2.normalized;

        return projV1OnOrthogonalV2;
    }

    public void ResetRadiusSatellite(Character owner)
    {
        for (int i = 0; i < owner.satellites.Count; i++)
        {
            Character character = owner.satellites[i];

            float limitedRadius = 0;
            if (owner.generalityType == GeneralityType.Planet)
            {
                limitedRadius = GameManager.instance.status.coefficientRadiusPlanet * owner.circleCollider2D.radius * SpawnPlanets.instance.GetScalePlanet(owner.characterType);

            }
            else if (owner.generalityType == GeneralityType.Star)
            {
                limitedRadius = GameManager.instance.status.coefficientRadiusStar * owner.circleCollider2D.radius * SpawnPlanets.instance.GetScalePlanet(owner.characterType);

            }
            float tmpRadius = limitedRadius + i * owner.circleCollider2D.radius * SpawnPlanets.instance.GetScalePlanet(owner.characterType) * 2;
            DOTween.To(() => character.radius, x => character.radius = x, tmpRadius, 0.3f)
                .OnStart(() => character.gameObject.GetComponent<CircleCollider2D>().enabled = false)
                .OnComplete(() => character.gameObject.GetComponent<CircleCollider2D>().enabled = true);

        }
    }

    public void AllWhenDie()
    {
        foreach (Character t in satellites)
        {
            if (t != null)
            {
                t.host = null;
                t.isCapture = false;
                t.myFamily = t;
            }

        }
        if (host != null)
        {
            host.satellites.Remove(this);
            host.ResetRadiusSatellite(host);
            host = null;
            isCapture = false;

        }

        satellites.Clear();
    }

    public void SoundAndVfxDie()
    {
        AudioManager.instance.PlaySFX("Planet-destroy");
        VfxManager.instance.PlanetDestroyVfx(transform.position, transform.rotation);
    }
}