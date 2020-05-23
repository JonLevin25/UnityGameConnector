using System;
using UnityEngine;

namespace Game_02.Scripts
{
    // Nyancat like flying (left edge of screen, limited left/right movement
    internal class G02_CharController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 20.0f;
        [SerializeField] private float projectileSpeed = 20.0f;
        [SerializeField] private Transform projectileSpawn;
        
        private Rigidbody2D body;
        private float horizontal;
        private float vertical;
        private float moveLimiter = 0.7f;
        private Projectile _projectilePrefab;

        public void Init(Projectile projectilePrefab)
        {
            _projectilePrefab = projectilePrefab;
        }
        
        void Start ()
        {
            body = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // Gives a value between -1 and 1
            horizontal = Input.GetAxis("Horizontal"); // -1 is left
            vertical = Input.GetAxis("Vertical"); // -1 is down

            if (Input.GetButtonDown("Submit"))
            {
                FireProjectile();
            }
        }

        private void FireProjectile()
        {
            var projDir = projectileSpawn.right;
            var projectile = Instantiate(_projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
            projectile.Init(projDir * moveSpeed);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log($"OnCollisionEnter2D. other: {other.gameObject.name}");
            var enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                OnEnemyCollision(enemy);
            }
        }

        private void OnEnemyCollision(Enemy enemy)
        {
            Debug.Log("Hit by enemy! ending game!");
            G02MiniGameController.Instance.EndGame();
        }

        void FixedUpdate()
        {
            body.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
        }
    }
}