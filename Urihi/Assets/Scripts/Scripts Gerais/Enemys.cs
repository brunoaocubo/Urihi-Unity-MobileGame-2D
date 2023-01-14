using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemys : MonoBehaviour
{
    #region Variáveis
    [Header("Controller")]
    [SerializeField] Transform Target;
    [SerializeField] float speedEnemy, StoppingDistance, forceImpulse;

    [Header("Configurações do Minerador")]
    [SerializeField] Slider barLife;
    [SerializeField] int lifeEnemy;
    [SerializeField] float recoveryTime; //Tempo para se recuperar de um hit.

    [Header("Combate")]
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform meelePivot;
    [SerializeField] int picaretaDamage; 
    [SerializeField] float picaretaRange, radiusAttack; 

    [Header("Animation")]
    [SerializeField] float picaretaDelay; //Tempo de animação
    private bool isWalking, isHurt, isDead, isAttacking;
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
    private float recoveryCounter, picaretaCooldown;

    //Identificação de Animações.
    const string MINERADOR_IDLE = "Minerador_Idle";
    const string MINERADOR_ATTACK = "Minerador_Attack";
    const string MINERADOR_WALK = "Minerador_Walk";
    const string MINERADOR_HURT = "Minerador_Hurt";
    const string MINERADOR_DEAD = "Minerador_Dead";
    #endregion

    #region Start, Updates.

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
        AnimationsMinerador();
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

    void FixedUpdate()
    {
        if (picaretaCooldown > 0) picaretaCooldown -= Time.deltaTime; if (picaretaCooldown <= 0) picaretaCooldown = 0;
        Picaretada();
    }
    #endregion

    #region Movimentação
    void Follow()
    {
        if (canMove)
        {
            if (Vector2.Distance(transform.position, Target.position) < StoppingDistance)
            {
                if (Vector2.Distance(transform.position, Target.position) > radiusAttack)
                {
                    transform.position = Vector2.MoveTowards(transform.position, Target.position, speedEnemy * Time.deltaTime);
                    isWalking = true;
                }
                else if (Vector2.Distance(transform.position, Target.position) <= radiusAttack)
                {
                    isWalking = false;
                }
            }
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
    void Picaretada()
    {
        if (picaretaCooldown == 0 && !isAttacking && !isDead)
        {
            if (Vector2.Distance(transform.position, Target.position) < radiusAttack) 
            {
                //isWalking = false;
                isAttacking = true; /**/ picaretaCooldown = 2f;
  
                Collider2D[] targets = Physics2D.OverlapCircleAll(meelePivot.position, picaretaRange, playerLayer);
                foreach (Collider2D target in targets)  // Foreach serve aqui para dizer que este codigo dentro será executado para cada inimigo 'target' que foi armazenado no vetor 'targets'.
                {
                    target.GetComponent<Player>().TakeDamage(picaretaDamage);
                }
                //StartCoroutine("StopMove");
            }   
        }
    }
   
    public void TakeDamage(int damage)
    {
        if (recovering == false && isHurt == false)
        {
            isHurt = true; /**/ recovering = true; /**/ lifeEnemy -= damage;
            barLife.value = lifeEnemy;
            Knockback();
            StartCoroutine("HitColor");

            if (lifeEnemy <= 0)
                Die();
        }
        Invoke("HurtComplete", 0.8f);
    }

    void Knockback()
    {
        rb.AddForce(new Vector2(forceImpulse, 0.5f));
        StartCoroutine("StopMove");
    }

    void Die()
    {
        StartBattle.points = StartBattle.points + 10;
        canMove = false;
        isDead = true;
        Destroy(gameObject, 0.75f);
    }
    #endregion

    #region Animações
    void AnimationsMinerador()
    {
        if (!isWalking && !isAttacking  && !isHurt && !isDead)
            ChangeAnimationState(MINERADOR_IDLE);
        else if (!isAttacking && !isHurt && !isDead) 
        {
            ChangeAnimationState(MINERADOR_WALK);
            SoundWalk();
        }
        else if (!isWalking && !isAttacking && !isDead)
        {
           ChangeAnimationState(MINERADOR_HURT);
           SoundHurt();
           Invoke("HurtComplete", 0.8f);
        }
        else if (isDead) 
        {
            ChangeAnimationState(MINERADOR_DEAD);
            SoundDead();
        }
        else if (!isWalking && !isHurt && !isDead)
        {
            ChangeAnimationState(MINERADOR_ATTACK);
            SoundPicareta();
            Invoke("AttackComplete", picaretaDelay);
        }
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        anim.Play(newState); /**/ currentState = newState;
    }
    #endregion

    #region Sounds Fx
    public void SoundWalk()
    {
        if (!soundfx[0].isPlaying) soundfx[0].Play();
    }
    public void SoundPicareta()
    {
        if (!soundfx[1].isPlaying) soundfx[1].Play();
    }
    public void SoundHurt()
    {
        if (!soundfx[2].isPlaying) soundfx[2].Play();
    }
    public void SoundDead()
    {
        if (!soundfx[3].isPlaying) soundfx[3].Play();
    }
    #endregion

    #region IEnumerator
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meelePivot.position, radiusAttack);
    }
    #endregion

    #region BooleanasForInvoke
    void AttackComplete() { isAttacking = default; }
    void HurtComplete() { isHurt = default; }
    #endregion
}
