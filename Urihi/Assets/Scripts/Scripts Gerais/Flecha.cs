using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour
{
    [SerializeField] float speedFlecha;
    [SerializeField] int attackDamage;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speedFlecha;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemys>()) 
        {
            collision.gameObject.GetComponent<Enemys>().TakeDamage(attackDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.GetComponent<Mercenario>()) 
        {
            collision.gameObject.GetComponent<Mercenario>().TakeDamage(attackDamage);
            Destroy(gameObject);
        }
    }
}
