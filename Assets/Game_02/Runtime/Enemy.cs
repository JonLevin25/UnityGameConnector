using UnityEngine;

namespace Game_02.Scripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Vector2 speed;
        [SerializeField] private Rigidbody2D rb;

        public void Init(Vector2 speed)
        {
            this.speed = speed;
        }
        
        private void Update()
        {
            rb.velocity = speed;
        }
    }
}