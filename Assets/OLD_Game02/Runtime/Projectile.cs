using System;
using UnityEngine;

namespace Game_02.Scripts
{
    public class Projectile : MonoBehaviour
    {
        private Vector3 _speed;
        
        public void Init(Vector3 speed)
        {
            _speed = speed;
        }
        
        private void Update()
        {
            transform.Translate(_speed * Time.deltaTime, Space.World);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("Projectile collision!");
            Destroy(gameObject);
        }
    }
}