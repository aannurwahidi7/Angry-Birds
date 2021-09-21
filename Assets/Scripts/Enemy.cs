using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float Health = 50f;

    public UnityAction<GameObject> OnEnemyDestroyed = delegate { };

    private bool _isHit = false;
    public bool destroyed = false;

    void OnDestroy()
    {
        if (_isHit || destroyed)
        {
            OnEnemyDestroyed(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() == null)
        {
            destroyed = true;
            return;
        }

        if (col.gameObject.tag == "Bird")
        {
            _isHit = true;
            Destroy(gameObject);
            destroyed = true;
        }
        else if (col.gameObject.tag == "Obstacle")
        {
            //Hitung damage yang diperoleh
            float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            Health -= damage;

            if (Health <= 0)
            {
                _isHit = true;
                Destroy(gameObject);
                destroyed = true;
            }
        }
    }
}
