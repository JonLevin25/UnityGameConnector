using UnityEngine;

namespace Game_01.Scripts
{
    internal class G01_CharController : MonoBehaviour
    {
        public float moveSpeed = 20.0f;
        
        private Rigidbody2D body;
        private float horizontal;
        private float vertical;
        private float moveLimiter = 0.7f;


        void Start ()
        {
            body = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            // Gives a value between -1 and 1
            horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
            vertical = Input.GetAxisRaw("Vertical"); // -1 is down
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log($"OnCollisionEnter2D. other: {other.gameObject.name}");
            // TODO: do stuff on 
        }

        void FixedUpdate()
        {
            if (horizontal != 0 && vertical != 0) // Check for diagonal movement
            {
                // limit movement speed diagonally, so you move at 70% speed
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            } 

            body.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
        }
    }
}