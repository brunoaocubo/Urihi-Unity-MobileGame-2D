using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    #region Variáveis
    [SerializeField] float moveSpeed;
    [SerializeField] int bulletDamage;

    Rigidbody2D rb;
    GameObject Target;
    Vector2 moveDirection;
    #endregion

    #region Start, Update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Target = GameObject.Find("BodyTarget");
        moveDirection = (Target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(gameObject, 4f);
    }
    #endregion

    #region Colisão
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") 
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
    #endregion
}
