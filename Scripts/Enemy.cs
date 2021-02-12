using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject DeathFx;
    [SerializeField] Transform parent;
    ScoreBoard scoreBoard;
    [SerializeField] int scorePerHit = 10;
    [SerializeField] int Hits = 3;
    // Start is called before the first frame update
    void Start()
    {
        AttachBoxCollider();
        scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    private void AttachBoxCollider()
    {
        Collider collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = false;
    }

    private void OnParticleCollision(GameObject other)
    { 
        Hits = Hits - 1;
        if (Hits <= 1)
        {
            print("Killed enemy");
            KilledEnemy();
        }
    }   

    private void KilledEnemy()
    {
        scoreBoard.ScoreHit(scorePerHit);
        GameObject fx = Instantiate(DeathFx, transform.position, Quaternion.identity);//quaternion.identity = no rotation
        fx.transform.parent = parent;
        Destroy(gameObject);
    }
}
