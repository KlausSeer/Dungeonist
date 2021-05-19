using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isDestroyed;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(AirTime());
    }

    private IEnumerator AirTime()
    {
        yield return new WaitForSeconds(2.5f);
        DestroyObject();
    }

    private void DestroyObject()
    {
        if(!isDestroyed)
        {
            isDestroyed = true;
            Destroy(this);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }

    private void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
