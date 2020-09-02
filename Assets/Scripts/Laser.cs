using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    private bool _isEnemyLaser = false;
    void Update()
    {
        if(_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 8f)
        {
            //check if this object has parent 
            //parent - Once triple shot power up completes, some lasers remain in game away from viewport
            //destroy the parent 
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
               Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            if (player!=null)
            {
                player.Damage();
            }
        }
    }
}
