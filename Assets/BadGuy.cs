using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BadGuy : EnemyInstance
{
    public float shootInterval;
    public Rigidbody cannonball;
    public Transform cannonOrigin;
    public VisualEffect shootAnim;
    float lastShot;

    protected override void StartBehaviour()
    {
        lastShot = Time.time;
    }
    protected override void UpdateBehaviour()
    {
        if(Time.time > lastShot + shootInterval)
        {
            cannonball.gameObject.SetActive(true);
            cannonball.isKinematic = false;
            cannonball.transform.position = cannonOrigin.position;
            Vector3 force = Camera.main.transform.position - cannonball.transform.position + Vector3.up*0.1f;
            cannonball.AddForce(force * 2, ForceMode.Impulse);
            lastShot = Time.time;
            shootAnim.Play();
        }
    }
}
