using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Spine.Unity;
using FMODUnity;
using FMOD;
using FMODUnityResonance;

public static class PlayerProperty
{
    public static int MaxHealth = 5;
    public static int MaxAbility = 5;
    public static float JumpPowerMult = 1.0f;

    //pray
    public static bool PrayThrow = true;
    public static bool PrayRotate = true;
    //Player
    public static bool Dash = true;
    public static bool DoubleJump = true;
}

public class PlatformerPlayerBehavior : SerializedMonoBehaviour
{

    [Title("Debug"), SerializeField] private bool Debug_Infinite_Ability = false;

    [Title("Input"),SerializeField] private KeyCode key_rightMove;
    [SerializeField] private KeyCode key_leftMove;
    [SerializeField] private KeyCode key_jump;
    [SerializeField] private KeyCode key_clap;
    [SerializeField] private KeyCode key_up;
    [SerializeField] private KeyCode key_dash;
    [SerializeField] private KeyCode key_save;

    [Space(20f)]
    [Title("Property"), SerializeField] private float horzSpeed;
    [SerializeField] private float fallingTerminalSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float dashVelocity = 1.0f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float clapCooldown = 2f;
    [SerializeField] private float abilityFillTime = 2.5f;
    [SerializeField] private float StepSoundInterval = 0.4f;
    [SerializeField] private float DamagerCooldown = 1.0f;
    [SerializeField] private float KnockBackPower = 5.0f;

    [Space(20f)]
    [Title("References"), SerializeField] private Transform RCO_FootL;
    [SerializeField] private Transform RCO_FootR;
    [SerializeField] private Transform RCO_Forward;
    [SerializeField] private MeshRenderer meshRend;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private LayerMask savepointLayer;
    [Title("Sound References")]
    [SerializeField] private StudioEventEmitter stepEvent;
    [SerializeField] private StudioEventEmitter hitEvent;
    [SerializeField] private StudioEventEmitter ambEvent;
    [SerializeField] private StudioEventEmitter bgmEvent;

    [Space(20f)]
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject landingEffectObject;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private GameObject clapEffect;
    [SerializeField] private bool isControlActive;
    public bool IsControlActive { get { return isControlActive; } }

    //static PlayerProperty playerProperty;

    private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }
    private int currentAbility;
    public int CurrentAbility { get { return currentAbility; } }
    private Vector2 rBodyVelocityOffset = Vector2.zero;


    Rigidbody2D rbody;
    Collider2D col;
    FMODSoundsTrigger fmodSound;

    private bool IsEnteredSavepoint;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        fmodSound = GetComponent<FMODSoundsTrigger>();
    }

    private void Start()
    {
        currentHealth = PlayerProperty.MaxHealth;
        currentAbility = PlayerProperty.MaxAbility;

        if (GamePlayUI.Instance != null)
        {
            GamePlayUI.Instance.InitializeHealthSkill(PlayerProperty.MaxHealth, PlayerProperty.MaxAbility, currentHealth, currentAbility);
        }

        if(currentHealth >= 3)
        {
            hitEvent.EventInstance.setParameterByNameWithLabel("SFX_Character_PC_HIt", "HP 3이상 피격");
        }
        else
        {
            hitEvent.EventInstance.setParameterByNameWithLabel("SFX_Character_PC_HIt", "HP 2 피격");
        }
    }

    bool DashFlag = true;

    public void TermianteAllMovements()
    {
        StopAllCoroutines();
    }

    float StepSoundT = 0f;

    private void FixedUpdate()
    {
        StepSoundT += Time.fixedDeltaTime;     
        bool isGrounded = IsGrounded();

        if (isControlActive)
        {
            Vector2 v = rbody.velocity;
            if (Input.GetKey(key_rightMove))
            {
                rbody.velocity = new Vector2(Mathf.Lerp(v.x, horzSpeed, 0.5f), v.y);
                transform.right = Vector2.right;
                anim.SetBool("HorControl", true);
                
                if (StepSoundT > StepSoundInterval && isGrounded)
                {
                    fmodSound.PlaySound("Step");
                    StepSoundT = 0f;
                }
            }
            else if (Input.GetKey(key_leftMove))
            {
                rbody.velocity = new Vector2(Mathf.Lerp(v.x, -horzSpeed, 0.5f), v.y);
                transform.right = Vector2.left;
                anim.SetBool("HorControl", true);

                if (StepSoundT > StepSoundInterval && isGrounded)
                {
                    fmodSound.PlaySound("Step");
                    StepSoundT = 0f;
                }
            }

            if (v.y <= -fallingTerminalSpeed) rbody.velocity = new Vector2(v.x, -fallingTerminalSpeed);
            anim.SetFloat("VertSpeed", v.y);
        }

        rbody.velocity = new Vector2(Mathf.Lerp(rbody.velocity.x, 0f, 0.2f), rbody.velocity.y);

        if (!(Input.GetKey(key_leftMove)|| Input.GetKey(key_rightMove)))
        {
            anim.SetBool("HorControl", false);
        }
    }

    float clapCoolT = 0f;
    float AbilityChargerT = 0f;
    float SaveHoldT = 0f;
    float damageCoolT = 0f;
    bool JumpFlag = false;
    bool DoubleJumpFlag = true;
    bool SaveKeyFlag = false;

    private void Update()
    {
       bool grounded = IsGrounded();

        damageCoolT -= Time.deltaTime;
        if (clapCoolT < clapCooldown) clapCoolT += Time.deltaTime;

        if (currentAbility < PlayerProperty.MaxAbility)
        {
            if (AbilityChargerT < abilityFillTime)
            {
                AbilityChargerT += Time.deltaTime;
            }
            else
            {
                AbilityChargerT = 0;
                currentAbility++;
                if (GamePlayUI.Instance != null)
                {
                    GamePlayUI.Instance.SetSkill(currentAbility);
                    GamePlayUI.Instance.FlashChargerHalo();
                }
            }

            if (GamePlayUI.Instance != null)
                GamePlayUI.Instance.SetSkillChargerRadial(AbilityChargerT / abilityFillTime);
        }


        if (isControlActive)
        {


            if (Input.GetKeyDown(key_clap) && clapCoolT > clapCooldown)
            {
                if (UseAbility(1))
                {
                    Invoke("NewClap", 0.75f);
                    anim.SetTrigger("Clap");
                    clapCoolT = 0f;
                }
            }

            if (Input.GetKeyDown(key_jump) && JumpFlag && grounded)
            {
                JumpFlag = false;
                fmodSound.PlaySound("JumpUp");
                anim.SetTrigger("Jump");
                Jump();
            }

            if (Input.GetKeyDown(key_jump) && DoubleJumpFlag && !grounded)
            {
                DoubleJumpFlag = false;
                fmodSound.PlaySound("DoubleJump");
                anim.SetTrigger("Jump");
                Jump();
            }

            if (Input.GetKeyDown(key_dash) && DashFlag && !grounded)
            {
                if (Input.GetKey(key_up))
                {
                    if (Input.GetKey(key_leftMove))
                    {
                        fmodSound.PlaySound("Dash");
                        StopCoroutine("Cor_Dash");
                        StartCoroutine("Cor_Dash", Vector2.left + Vector2.up);
                        DashFlag = false;
                    }
                    else if (Input.GetKey(key_rightMove))
                    {
                        fmodSound.PlaySound("Dash");
                        StopCoroutine("Cor_Dash");
                        StartCoroutine("Cor_Dash", Vector2.right + Vector2.up);
                        DashFlag = false;
                    }
                    else
                    {
                        fmodSound.PlaySound("Dash");
                        StopCoroutine("Cor_Dash");
                        StartCoroutine("Cor_Dash", Vector2.up);
                        DashFlag = false;
                    }
                }
                else
                {
                    if (Input.GetKey(key_leftMove))
                    {
                        fmodSound.PlaySound("Dash");
                        StopCoroutine("Cor_Dash");
                        StartCoroutine("Cor_Dash", Vector2.left);
                        DashFlag = false;
                    }
                    else if (Input.GetKey(key_rightMove))
                    {
                        fmodSound.PlaySound("Dash");
                        StopCoroutine("Cor_Dash");
                        StartCoroutine("Cor_Dash", Vector2.right);
                        DashFlag = false;
                    }
                }
            }

        }

        if (grounded)
        {
            anim.SetBool("Grounded", true);

            if (rbody.velocity.y < 0.001f)
            {
                if (JumpFlag == false)
                    fmodSound.PlaySound("JumpDown");

                DashFlag = true;
                JumpFlag = true;
                DoubleJumpFlag = true;
            }
        }
        else
        {
            anim.SetBool("Grounded", false);

            if (Physics2D.Raycast(RCO_FootL.position, Vector2.down, 2.0f, collisionLayer) || Physics2D.Raycast(RCO_FootR.position, Vector2.down, 2.0f, collisionLayer))
            {
                if (rbody.velocity.y < -fallingTerminalSpeed)
                {
                    rbody.velocity = new Vector2(rbody.velocity.x, 2.0f);
                    Instantiate(landingEffectObject, transform.position + Vector3.down * 2f, Quaternion.identity);
                }
            }
        }

        if (isControlActive)
        {
            if (Input.GetKey(key_save) && SaveKeyFlag == false)
            {
                SaveHoldT += Time.deltaTime;

                if (SaveHoldT > 3.0f)
                {

                    //save
                    SaveKeyFlag = true;
                }
            }
            else
            {
                if (SaveKeyFlag) SaveHoldT = 0f;
                SaveHoldT -= Time.deltaTime;
                SaveKeyFlag = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damageCoolT < 0)
        {
            if (damageLayer == (damageLayer | (1 << collision.gameObject.layer)))
            {
                DamageAffection((transform.position - collision.transform.position));
                UseHelath(1);
            }
            damageCoolT = DamagerCooldown;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer & (1 << savepointLayer.value)) != 0)
        {
            IsEnteredSavepoint = true;
        }

        if (damageCoolT < 0)
        {
            if (damageLayer == (damageLayer | (1 << collision.gameObject.layer)))
            {
                DamageAffection(transform.position - collision.transform.position);
                UseHelath(1);
            }
            damageCoolT = DamagerCooldown;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.layer & (1 << savepointLayer.value)) != 0)
        {
            IsEnteredSavepoint = false;
        }
    }

    IEnumerator Cor_Dash(Vector2 direction)
    {
        Instantiate(dashEffect, transform);

        for (float i = 0; i < dashDuration; i += Time.fixedDeltaTime)
        {
            //Instantiate(obj_SpriteTrail,transform.position,Quaternion.identity).GetComponent<SpriteTrailBehavior>().OnSpawned(spriteRenderer.sprite,spriteRenderer.transform);
            rbody.MovePosition(rbody.position + direction * dashVelocity);
            yield return new WaitForFixedUpdate();
        }
        rbody.velocity = new Vector2((direction * horzSpeed).x, 0f);
    }

    IEnumerator Cor_KnockBackColor()
    {
        yield return null;
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        string colorPorperty = "_Color";

        for(float t = damageCoolT; t > 0; t -= Time.fixedDeltaTime)
        {
            if(t % 0.2f > 0.1f)
                block.SetColor(colorPorperty, Color.red);
            else if(t % 0.2f < 0.1f)
                block.SetColor(colorPorperty, Color.white);

            meshRend.SetPropertyBlock(block);
            yield return new WaitForFixedUpdate();
        }
        block.SetColor(colorPorperty, Color.white);
        meshRend.SetPropertyBlock(block);
    }

    public bool IsGrounded()
    {
        var rHit = Physics2D.Raycast(RCO_FootL.position, Vector2.down, 0.1f, collisionLayer);
        if ( rHit  || Physics2D.Raycast(RCO_FootR.position, Vector2.down, 0.1f, collisionLayer))
        {
            int type = SoundMaterialManager.Instance.FindMatIndex(gameObject.tag);
            stepEvent.EventInstance.setParameterByName("SFX_Character_PC_Move", type);
            return true;
        }
        else
            return false;
    }

    private void NewClap()
    {
        fmodSound.PlaySound("Clap");
        Instantiate(clapEffect, transform);
        ClapsManager.Instance.AddNewClap(transform.position);
    }

    private void Jump()
    {
        rbody.velocity = new Vector2(rbody.velocity.x, jumpPower);
    }

    public void DamageAffection(Vector2 knockBack)
    {
        rBodyVelocityOffset = Vector2.zero;
        rbody.velocity += (knockBack.normalized-Vector2.left*knockBack.normalized*5f ) * KnockBackPower;

        StopCoroutine("Cor_KnockBackColor");
        StartCoroutine("Cor_KnockBackColor");
    }

    public void UseHelath(int var)
    {
        if (currentHealth <= var)
        {
            StopBackgroundAudios();
            isControlActive = false;
            damageCoolT = float.PositiveInfinity;
            GamePlayUI.Instance.SetHealth(0);
            GeneralStageManager.Instance.OnPlayerDied();
        }
        else
        {
            currentHealth -= var;
            GamePlayUI.Instance.SetHealth(currentHealth);
            hitEvent.EventInstance.start();
        }
    }

    public bool TryNewSave(Transform savePoint)
    {
        bool saveSuccessful;

        if (IsEnteredSavepoint)
        {
            saveSuccessful = true;
        }
        else
        {
            saveSuccessful = UseAbility(1);
        }

        if (saveSuccessful)
        {
            GeneralStageManager.Instance.SetNewSavePoint(savePoint.position);
        }

        return saveSuccessful;
    }

    public bool UseAbility(int var)
    {
        if (Debug_Infinite_Ability) return true;

        if (currentAbility < var) return false;
        else
        {
            currentAbility -= var;
            if (GamePlayUI.Instance != null)
                GamePlayUI.Instance.SetSkill(currentAbility);
            return true;
        }
    }

    public void StopBackgroundAudios()
    {
        ambEvent.EventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        bgmEvent.EventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        
    }
}
