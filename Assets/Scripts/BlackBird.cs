using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBird : Bird
{
    [SerializeField]
    public float _explodeEffect;
    public float _explodeForce;
    public bool _hasExplode = false;

    public void Explode()
    {
        if (!_hasExplode)
        {
            _hasExplode = true;
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, _explodeEffect);

            foreach(Collider2D obj in objects)
            {
                Rigidbody2D _objectRigidBody = obj.GetComponent<Rigidbody2D>();
                if(_objectRigidBody != null && (obj.tag == "Bird" || obj.tag == "Enemy" || obj.tag == "Obstacle"))
                {
                    Vector2 distanceVector = obj.transform.position - transform.position;
                    if(distanceVector.magnitude > 0)
                    {
                        float explosionForce = _explodeForce / distanceVector.magnitude;
                        _objectRigidBody.AddForce(distanceVector.normalized * explosionForce);
                    }
                }
            }
        }
    }

    public override void OnTap()
    {
        Explode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explodeEffect);
    }
}
