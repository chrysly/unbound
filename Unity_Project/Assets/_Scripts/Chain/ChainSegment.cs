using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSegment : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public Chain chain { get; set; }
    public ChainSegment ahead { get; set; }
    public ChainSegment behind { get; set; }
    public bool isHead => ahead == null;

    private Vector2 direction = Vector2.right + Vector2.down;
    private Vector2 targetPosition;
    
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPosition = transform.position;

        //if (isHead) {
        //    gameObject.GetComponent<Animator>().enabled = true;
        //}
    }

    private void Update() {
        if (isHead && Vector2.Distance(transform.position, targetPosition) < 0.1f) {
            UpdateHeadSegment();
        }
        
        Vector2 currentPosition = transform.position;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, chain.speed * Time.deltaTime);

        Vector2 movementDirection = (targetPosition - currentPosition).normalized;
        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void UpdateHeadSegment() {
        Vector2 gridPosition = GridPosition(transform.position);
        targetPosition = gridPosition;
        targetPosition.x += direction.x;

        if (Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, chain.collisionMask)) {
            direction.x = -direction.x;
            targetPosition.x = gridPosition.x;
            targetPosition.y = gridPosition.y + direction.y;

            Bounds homeBounds = chain.homeBase.bounds;
            if ((direction.y == 1f && targetPosition.y > homeBounds.max.y) ||
                (direction.y == -1f && targetPosition.y < homeBounds.min.y)) {
                direction.y = -direction.y;
                targetPosition.y = gridPosition.y + direction.y;
            }

            Debug.Log("Collided");
        }
        
        if (behind != null) {
            behind.UpdateBodySegment();
        }
    }

    private void UpdateBodySegment() {

        targetPosition = GridPosition(ahead.transform.position);
        direction = ahead.direction;
        
        if (behind != null) {
            behind.UpdateBodySegment();
        }
    }
    
    private Vector2 GridPosition(Vector2 position) {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GameManager.Instance.ResetRound();
            return;
        }
        
        if (collision.collider.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Projectile")) {
            collision.collider.enabled = false;
            chain.Remove(this);
        }
    }
}
