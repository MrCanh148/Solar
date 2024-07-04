using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : Character
{
    public float moveSpeed;
    public float velocityStart;
    public bool canWASD = false;

    private bool isMovingUp;
    private bool isMovingDown;
    private bool isMovingLeft;
    private bool isMovingRight;
    private float velocityMoveUp;
    private float velocityMoveDown;
    private float velocityMoveLeft;
    private float velocityMoveRight;

    [SerializeField] private Camera miniCam;
    [SerializeField] private Button absorbButton;
    [SerializeField] private Joystick joystick;

    protected override void Start()
    {
        base.Start();
        canControl = true;
        absorbButton.onClick.AddListener(TryAbsorbCharacter);
    }

    protected override void OnInit()
    {
        base.OnInit();
        velocityMoveUp = 0;
        velocityMoveDown = 0;
        velocityMoveLeft = 0;
        velocityMoveRight = 0;
        velocityStart = 5;
        moveSpeed = 20;
    }

    private void Update()
    {
        if (canWASD)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || joystick.Vertical > 0.1f)
            {
                isMovingUp = true;
            }
            else { isMovingUp = false; }

            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || joystick.Vertical < -0.1f)
            {
                isMovingDown = true;
            }
            else { isMovingDown = false; }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || joystick.Horizontal < -0.1f)
            {
                isMovingLeft = true;
            }
            else { isMovingLeft = false; }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || joystick.Horizontal > 0.1f)
            {
                isMovingRight = true;
            }
            else { isMovingRight = false; }

            miniCam.transform.rotation = Quaternion.identity; // Lock miniCamera

            // Nhan SPACE de Observe Orbit
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryAbsorbCharacter();
            }
        }
    }

    protected override void FixedUpdate()
    {
        if (canControl)
        {
            if (isMovingUp)  // Len
            {
                velocityMoveUp += velocityStart * 0.1f * GameManager.instance.status.acceleration * Time.fixedDeltaTime;
                velocityMoveUp = Mathf.Clamp(velocityMoveUp, -moveSpeed, moveSpeed);
            }
            else
            {
                velocityMoveUp -= velocityMoveUp * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
                if (Mathf.Abs(velocityMoveUp) < 0.01f)
                {
                    velocityMoveUp = 0f;
                }
            }

            if (isMovingDown)  //Xuong
            {
                velocityMoveDown += velocityStart * 0.1f * GameManager.instance.status.acceleration * Time.fixedDeltaTime;
                velocityMoveDown = Mathf.Clamp(velocityMoveDown, -moveSpeed, moveSpeed);
            }
            else
            {
                velocityMoveDown -= velocityMoveDown * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
                if (Mathf.Abs(velocityMoveDown) < 0.01f)
                {
                    velocityMoveDown = 0f;
                }
            }

            if (isMovingLeft) //Trai
            {
                velocityMoveLeft += velocityStart * 0.1f * GameManager.instance.status.acceleration * Time.fixedDeltaTime;
                velocityMoveLeft = Mathf.Clamp(velocityMoveLeft, -moveSpeed, moveSpeed);
            }
            else
            {
                velocityMoveLeft -= velocityMoveLeft * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
                if (Mathf.Abs(velocityMoveLeft) < 0.01f)
                {
                    velocityMoveLeft = 0f;
                }
            }

            if (isMovingRight)  // Phai
            {
                velocityMoveRight += velocityStart * 0.1f * GameManager.instance.status.acceleration * Time.fixedDeltaTime;
                velocityMoveRight = Mathf.Clamp(velocityMoveRight, -moveSpeed, moveSpeed);
            }
            else
            {
                velocityMoveRight -= velocityMoveRight * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
                if (Mathf.Abs(velocityMoveRight) < 0.01f)
                {
                    velocityMoveRight = 0f;
                }
            }

        }

        float velocityHorizontal = velocityMoveUp - velocityMoveDown;
        float velocityVertical = velocityMoveRight - velocityMoveLeft;
        externalVelocity = new Vector2(velocityVertical, velocityHorizontal);
        miniCam.transform.rotation = Quaternion.identity;

        base.FixedUpdate();
    }

    protected override void ResetExternalVelocity()
    {
        base.ResetExternalVelocity();
        velocityMoveUp = 0;
        velocityMoveDown = 0;
        velocityMoveLeft = 0;
        velocityMoveRight = 0;
    }

    public void ResetVelocity()
    {
        velocityMoveUp = 0;
        velocityMoveDown = 0;
        velocityMoveLeft = 0;
        velocityMoveRight = 0;
        velocity = Vector2.zero;
        externalVelocity = Vector2.zero;
    }

    private void TryAbsorbCharacter()
    {
        Character character = GetCharacteHaveSatellite();
        if (character != null)
        {
            AudioManager.instance.PlaySFX("Eat");
            AbsorbCharacter(this, character);
        }
    }
}
