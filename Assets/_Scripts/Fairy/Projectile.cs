using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour {
    private new Rigidbody2D _rigidbody2D;
    private new Collider2D _collider2D;
    private Transform parent;
    private SpriteRenderer _spriteRenderer;

    public float speed = 30f;

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        
        _collider2D = GetComponent<Collider2D>();
        _collider2D.enabled = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        
        parent = transform.parent;
    }

    private void Update() {
        if (_rigidbody2D.isKinematic && Input.GetButton("Fire1")) {
            transform.SetParent(null);
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _collider2D.enabled = true;
            _spriteRenderer.enabled = true;
        }
    }

    private void FixedUpdate() {
        if (!_rigidbody2D.isKinematic) {
            Vector2 position = _rigidbody2D.position;
            position += Vector2.up * speed * Time.fixedDeltaTime;
            _rigidbody2D.MovePosition(position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0f, 1.5f, 0f);
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        _collider2D.enabled = false;
    }
}
