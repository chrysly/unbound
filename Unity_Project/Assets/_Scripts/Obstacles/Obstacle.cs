using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    public Sprite[] states;
    public int _health;
    private SpriteRenderer _spriteRenderer;
    public int points = 1;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _health = states.Length;
    }

    public void Damage(int amount) {
        _health -= amount;

        if (_health > 0) {
            _spriteRenderer.sprite = states[states.Length - _health];
        }
        else {
            Destroy(gameObject);
            GameManager.Instance.IncreaseScore(points);
        }
    }

    public void Heal() {
        _health = states.Length;
        _spriteRenderer.sprite = states[0];
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectile")) {
            Damage(1);
        }
    }
}
