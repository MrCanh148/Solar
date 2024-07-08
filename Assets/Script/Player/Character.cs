﻿using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Meteoroid = 0,
    Asteroid = 1,
    Planet = 2,
    LifePlanet = 3,
    GasGiant = 4,
    Star = 5,
    NeutronStar = 6,
    BlackHole = 7,
    BigBang = 8,
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
    public CircleCollider2D circleCollider2D;
    public Vector2 velocity;
    public Vector2 externalVelocity;
    public Vector2 mainVelocity;
    public bool isPlayer;
    public bool isDead;
    public bool canControl;
    public bool isBasicReSpawn;
    public SpriteRenderer spriteRenderer;
    public List<Character> satellites1;
    public List<Character> satellites2;

    [Header("========= Orbit =========")]
    public Character host;
    public float radius; // Bán kính quỹ đạo
    public float spinSpeed; // Tốc độ quay
    public float angle;
    public bool isCapture;

    [Header("======= Other =======")]
    public int Kill;
    public bool EvolutionDone = false;
    public Character myFamily;
    public bool isSetup;
    private float x, y;
    private Vector2 direction, tmp, dirVeloc;
    private Vector3 contactPoint;
    private int SpeedRotate;

    protected virtual void Start()
    {
        SpeedRotate = GenerateRandomValue();
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
            x = Mathf.Cos(angle) * radius;
            y = Mathf.Sin(angle) * radius;

            // Cập nhật vị trí của đối tượng
            tf.position = host.tf.position + new Vector3(x, y, 0f);
            angle += spinSpeed * Time.deltaTime;
        }

        if (host != null && tf != null)
            myFamily = host.myFamily;

    }

    protected virtual void FixedUpdate()
    {
        velocity.x -= velocity.x * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
        velocity.y -= velocity.y * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
        velocity = new Vector2(velocity.x, velocity.y);
        mainVelocity = velocity + externalVelocity;
        tf.Rotate(Vector3.forward, SpeedRotate * Time.deltaTime);

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

        if (characterType != CharacterType.LifePlanet || !gameObject.activeSelf)
        {
            Kill = 0;
            EvolutionDone = false;
        }
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

        //========================================================= logic asteroid
        if (generalityType == GeneralityType.Asteroid && character.generalityType == GeneralityType.Asteroid)
        {
            if (characterType >= character.characterType)
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
            
            if (characterType < character.characterType)
            {
                if (this.isPlayer)
                    ReSpawnPlayer.Instance.ResPlayer();
                else
                {
                    MergeCharacter(character, this);
                    Vector2 velocityS = (character.rb.mass * character.velocity + rb.mass * velocity) / (rb.mass + character.rb.mass);
                    velocity = new Vector2(velocityS.x, velocityS.y);
                }
                return;
            }
        }

        //========================================================== logic BlackHole
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

        //================================================================== other logic
        if (characterType != character.characterType || characterType == character.characterType)
        {
            if (generalityType == GeneralityType.Asteroid)
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
        if (character.satellites1.Count <= 0)
        {
            DOTween.To(() => character.radius, x => character.radius = x, 2.5f * host.circleCollider2D.radius * SpawnPlanets.instance.GetScalePlanet(host.characterType), .7f)
           .OnComplete(() =>
           {
               SpawnPlanets.instance.DeActiveCharacter(character);
               host.satellites1.Remove(character);
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
        if (satellites1.Count > 0)
        {
            character = satellites1[0];

            foreach (var c in satellites1)
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
        if (satellites1.Count > 0)
        {
            character = satellites1[0];

            foreach (var c in satellites1)
            {
                if (c.satellites1.Count > character.satellites1.Count)
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

    public void AllWhenDie()
    {
        foreach (Character t in satellites1)
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
            host.satellites1.Remove(this);
            host = null;
            isCapture = false;

        }

        satellites1.Clear();
    }

    public int GenerateRandomValue()
    {
        int randomValue = Random.Range(100, 201);
        int sign = Random.Range(0, 2) * 2 - 1;
        return randomValue * sign;
    }

    public void SoundAndVfxDie()
    {
        AudioManager.instance.PlaySFX("Planet-destroy");
        VfxManager.instance.PlanetDestroyVfx(transform.position, transform.rotation);
    }
}