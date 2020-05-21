using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifespan = 5f;
    public int damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(projectileLifespanCo());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != this.tag)
        {
            if(collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyManager>().damageEnemy(damage);
            }

            Destroy(this.gameObject);
        }
    }

    private IEnumerator projectileLifespanCo()
    {
        yield return new WaitForSeconds(.5f);

        damage = damage / 2;

        yield return new WaitForSeconds(lifespan);

        Destroy(this.gameObject);
    }
}
