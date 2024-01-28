using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstance : MonoBehaviour
{
    public AudioSource sound;
    public AudioClip popupSound;
    public AudioClip spinSound;
    public AudioClip grabbedSound;
    public EnemyType type;
    Animator anim;
    float startTime;
    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        anim.Play("EnemyPopup");
    }

    public void InstantiateEnemy(EnemyType type)
    {
        this.type = type;
        startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > startTime + type.lifetime)
        {
            FallDown();
        }
    }

    public void PopupEvent()
    {
        sound.clip = popupSound;
        sound.Play();
    }

    public void SpinEvent()
    {
        sound.clip = spinSound;
        sound.Play();
    }

    public void Whip()
    {
        anim.Play("EnemySpin");
    }

    public void FallDown()
    {
        anim.Play("EnemyDespawn");
    }

    public void Poof()
    {
        Despawn();
    }

    public void Despawn()
    {
        GameWrangler.Instance.RemoveEnemy(this);
    }

    public void Yank(Vector3 impulse)
    {
        rb.isKinematic = false;
        rb.AddForce(impulse, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            //rb.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Lasso")
        {
            Debug.Log("Trigger lasso");
        }
    }
}

[CreateAssetMenu(fileName = "Enemy", menuName = "Lasso/Create EnemyType", order = 1)]
public class EnemyType : ScriptableObject
{
    public string enemyName;
    public float lifetime;
    public int score;
    public EnemyInstance prefab;
}