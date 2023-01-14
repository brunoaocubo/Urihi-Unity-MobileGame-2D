using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mercenario : MonoBehaviour
{
    #region Variáveis
    [Header("Controller")]
    [SerializeField] Transform Target;
    [SerializeField] float speedEnemy, StoppingDistance, forceImpulse;

    [Header("Configurações do Mercenário")]
    [SerializeField] Slider barLife;
    [SerializeField] int lifeEnemy;
    [SerializeField] float recoveryTime; //Tempo para se recuperar de um hit.
    
    [Header("Combate")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawn;

    [Header("Animation")]
    [SerializeField] float fireDelay;
    private bool isWalking, isHurt, isAttacking;
    public bool isDead;
    private string currentState;

    [Header("Efeitos Sonoros")]
    [SerializeField] AudioSource[] soundfx;

    //Outras Variáveis Privadas
    Animator anim;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Vector3 facingright, facingleft;
    private bool canMove = true;
    private bool recovering;
    private float recoveryCounter, nextFire;

    //Identificação de Animações.
    const string MERCENÁRIO_IDLE = "Mercenário_Idle";
    const string MERCENÁRIO_FIRE = "Mercenário_Fire";
    const string MERCENÁRIO_WALK = "Mercenário_Walk";
    const string MERCENÁRIO_DEAD = "Mercenário_Dead";

    #endregion

    #region Start, Updates
    void Start()
    {    
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        barLife.value = lifeEnemy;

        facingright = transform.localScale;
        facingleft = transform.localScale;
        facingleft.x = facingleft.x * -1;
    }

    void Update()
    {
        AnimationsMercenário();
        
        if (nextFire >= 0) nextFire -= Time.deltaTime; if (nextFire <= 0) nextFire = 0;
        
        if (canMove)
        {
            Follow();
        }
        if (recovering) //Se recuperando depois de tomar um hit.
        {
            recoveryCounter += Time.deltaTime;
            if (recoveryCounter > recoveryTime)
            {
                recovering = false;
                recoveryCounter = 0;
            }
        }
    }
    #endregion

    #region Movimentação Follow
    void Follow()
    {
        if (Vector2.Distance(transform.position, Target.position) > StoppingDistance)
        {
            //if (Vector2.Distance(transform.position, Target.position) > radiusAttack)
            if (canMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, Target.position, speedEnemy * Time.deltaTime);
                isWalking = true;
            }
        }
       
        if (Vector2.Distance(transform.position, Target.position) <= StoppingDistance)
        {
            isWalking = false;
            Fire();
        }

        if (canMove)
        {
            if (transform.position.x > Target.position.x)
            {
                transform.localScale = facingleft;
            }
            else
                transform.localScale = facingright;
        }
    }
    #endregion

    #region Combate
    void Fire() 
    {  
        if (nextFire <= 0 && isDead == false) 
        {
            isAttacking = true;
            Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
            nextFire = 1f;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!recovering)
        {
            isHurt = true; /**/ recovering = true; /**/ lifeEnemy -= damage;
            barLife.value = lifeEnemy;
            Knockback();
            StartCoroutine("HitColor");

            if (lifeEnemy <= 0)
                Die();
        }
    }

    void Knockback()
    {
        rb.AddForce(new Vector2(forceImpulse, 0.5f));
        StartCoroutine("StopMove");
    }

    public void Die()
    {
        StartBattle.points = StartBattle.points + 10;
        canMove = false;
        isDead = true;
        Destroy(gameObject, 0.75f); //O tempo é baseado no tempo da animação.
    }
    #endregion

    #region Animações
    void AnimationsMercenário()
    {
        if (!isWalking && !isAttacking && !isHurt && !isDead)
            ChangeAnimationState(MERCENÁRIO_IDLE);
        else if (!isAttacking && !isHurt && !isDead) 
        {
            ChangeAnimationState(MERCENÁRIO_WALK);
            SoundWalk();
        }
        else if (isDead) 
        {
            ChangeAnimationState(MERCENÁRIO_DEAD);
            SoundDead();
        }
        else if (!isWalking && !isHurt && !isDead)
        {
            ChangeAnimationState(MERCENÁRIO_FIRE);
            SoundFire();
            Invoke("AttackComplete", nextFire);
        }
    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        anim.Play(newState); /**/ currentState = newState;
    }
    #endregion

    #region IEnumerator/Other
    IEnumerator StopMove()
    {
        rb.velocity = Vector2.zero;
        canMove = false;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
    }

    IEnumerator HitColor()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sprite.color = Color.white;
    }
    #endregion

    #region Sounds Fx
    public void SoundWalk()
    {
        if (!soundfx[0].isPlaying) soundfx[0].Play();
    }
    public void SoundFire()
    {
        if (!soundfx[1].isPlaying) soundfx[1].Play();
    }
    public void SoundDead()
    {
        if (!soundfx[2].isPlaying) soundfx[2].Play();
    }
    #endregion

    #region BooleanasForInvoke
    void AttackComplete() { isAttacking = default; }
    void HurtComplete() { isHurt = default; }
    #endregion
}
