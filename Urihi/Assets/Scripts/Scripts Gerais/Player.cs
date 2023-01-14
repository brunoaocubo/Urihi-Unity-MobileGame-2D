using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Variáveis
    [Header("Configurações do Joystick")]
    [SerializeField] FixedJoystick moveJoystick;

    [Header("Configurações do Player")]
    [SerializeField] ParticleSystem particles;
    [SerializeField] GameObject skill;
    [SerializeField] Transform skillSpawn, spawnPos;
    [SerializeField] Slider barLife;
    [SerializeField] float playerSpeed, recoveryTime, knockbackForce;
    [SerializeField] public int life;

    [Header("Combate")]
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] LayerMask enemy2Layers;
    [SerializeField] int punchDamage;
    [SerializeField] float punchRange, radiusAttack; 
    [SerializeField] Transform meelePivot;

    [Header("Animações")]
    [SerializeField] float punchDelay = 0.375f;
    [SerializeField] float arcoDelay = 0.3f;
    private bool isPunching, isUsingArco, isWalking, isHurt; public bool isDead;

    [Header("Efeitos Sonoros")]
    [SerializeField] AudioSource[] soundfx;

    //Outras Variáveis Privadas
    private Vector2 direction;
    private SpriteRenderer sprite;
    private Animator anim;
    private Rigidbody2D rb;
    private bool canMove = true; /**/ bool isFacingRight = true;
    private bool recoverHIT;
    private float recoveryCounter;
    private float punchCooldown, arcoCooldown; 

    #endregion

    #region Start, Updates
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        barLife.value = life;
    }

    void Update()
    {
        if (punchCooldown >= 0) punchCooldown -= Time.deltaTime; if (punchCooldown <= 0) punchCooldown = 0;
        if (arcoCooldown >= 0) arcoCooldown -= Time.deltaTime; if (arcoCooldown <= 0) arcoCooldown = 0;

        Walking();
        MovePlayer();
        RecoveryHit();
    }
    #endregion

    #region Movimentação
    void MovePlayer()
    {
        //Movimentação do personagem com uso do velocity //Direção para onde o personagem tem que olhar.
        if (canMove)
        {
            direction = Vector2.up * moveJoystick.Vertical + Vector2.right * moveJoystick.Horizontal;
            rb.velocity = direction * playerSpeed;
        }
  
        if (isFacingRight && direction.x < 0f || !isFacingRight && direction.x > 0f) 
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        } 
    }

    void Walking()
    {
        if (rb.velocity.x > 0 || rb.velocity.x < 0 || rb.velocity.y > 0 || rb.velocity.y < 0)
        {
            CreateRunParticles();
            anim.SetBool("Walk", true);
            SoundWalk();
            isWalking = true;
        }
        else
        {
            anim.SetBool("Walk", false);
            isWalking = default;
        }
    }
    #endregion

    #region Combate
    public void IniciaArco()
    {
        if (arcoCooldown <= 0)
        {
            anim.SetTrigger("Arco");
            SoundBow();
            StartCoroutine("Freeze");
            rb.velocity = Vector2.zero;
            arcoCooldown = 1f;
            Instantiate(skill, skillSpawn.position, skillSpawn.rotation);

            if (!isUsingArco)
                isUsingArco = true;
        }
    }

    public void IniciaSoco()
    {
        if (punchCooldown <= 0)
        {
            anim.SetTrigger("Attack");
            SoundPunch();
            isPunching = true; /**/ punchCooldown = 0.5f;
            Punch();
        }
    }

    void Punch()
    {
     //Foreach serve aqui para dizer que este codigo dentro será executado para cada inimigo 'target' que foi armazenado no vetor 'targets'.
     //'targets´ é um vetor utilizado aqui para definir que todo inimigo que o ataque encostar será atribuido tal condição, a de dano usando como auxilio a layer enemyLayers.
        
        Collider2D[] targets = Physics2D.OverlapCircleAll(meelePivot.position, punchRange, enemyLayers);
        foreach (Collider2D target in targets)  
        {
            target.GetComponent<Enemys>().TakeDamage(punchDamage); 
        }
        Collider2D[] targets2 = Physics2D.OverlapCircleAll(meelePivot.position, punchRange, enemy2Layers);
        foreach (Collider2D target2 in targets2)
        {
            target2.GetComponent<Mercenario>().TakeDamage(punchDamage);
        }

        rb.velocity = Vector2.zero;
        StartCoroutine("Freeze");
    }

    public void TakeDamage(int damage)
    {
        if (!recoverHIT)
        {
            anim.SetTrigger("TakeHit");
            StartCoroutine("DmgColor");
            SoundHurt();
            recoverHIT = true; /**/ life -= damage;
            barLife.value--;
            Knockback();

            if (life <= 0)
                Die();
        }
    }

    void RecoveryHit() 
    {
        if (recoverHIT) //Se recuperando depois de tomar um hit.
        {
            recoveryCounter += Time.deltaTime;
            if (recoveryCounter > recoveryTime)
            {
                recoverHIT = false;
                recoveryCounter = 0;
            }
        }
    }

    void Knockback()
    {
        rb.AddForce(new Vector2(knockbackForce, 0.5f));
        StartCoroutine("Freeze");
    }

    void Die()
    {
        isDead = true;
        if (isDead) 
        {
            anim.SetBool("Walk", true);
            isDead = false;
            life = life + 10;
            barLife.value = life;
            transform.position = spawnPos.position;
        }
    }
    #endregion

    #region IEnumerator's / Other
    IEnumerator Freeze()
    {
        canMove = false;
        yield return new WaitForSeconds(0.3f);
        canMove = true;
    }

    IEnumerator DmgColor() 
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        sprite.color = Color.white;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(meelePivot.position, radiusAttack);
    }
    #endregion

    #region Animações
    void CreateRunParticles() 
    {
        particles.Play();
    }
    #endregion

    #region SoundsFx
    public void SoundWalk()
    {
        if (!soundfx[0].isPlaying) soundfx[0].Play();
    }
    public void SoundPunch()
    {
        if (!soundfx[1].isPlaying) soundfx[1].Play();
    }
    public void SoundBow()
    {
        if (!soundfx[2].isPlaying) soundfx[2].Play();
    }
    public void SoundHurt()
    {
        if (!soundfx[3].isPlaying) soundfx[3].Play();
    }
    #endregion
}