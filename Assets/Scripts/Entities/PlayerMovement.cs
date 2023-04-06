using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Component that handles player movement
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed;

    private Vector2 movement;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        movement = new Vector2(x, y);
    }
    private void FixedUpdate() {
        if(movement != Vector2.zero){
            Move();
        }
    }
    private void Move(){
        Vector2 force = new Vector2(
            movement.x * speed * Time.deltaTime * 1000f,
            movement.y * speed * Time.deltaTime * 1000f
        );

        rb.AddForce(force);
    }
}
