using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstance : MonoBehaviour
{
    public enum EnemyState { Acting, Lassoed, Dying, Dead }
    public EnemyState state = EnemyState.Acting;
    public AudioSource sound;
    public AudioClip popupSound;
    public AudioClip spinSound;
    public AudioClip grabbedSound;
    public EnemyType type;
    public GameObject lassoMesh;
    public Transform lassoPoint;
    Animator anim;
    float startTime;
    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    protected void Start()
    {
        anim.Play("EnemyPopup");
    }

    public void InstantiateEnemy(EnemyType type)
    {
        Debug.Log("Instantiating type " + type.name);
        this.type = type;
        startTime = Time.time;
        StartBehaviour();
    }

    protected void Update()
    {
        Debug.Log(state);
        Debug.Log(type.lifetime);
        Debug.Log(startTime);
        if (Time.time > startTime + type.lifetime && state == EnemyState.Acting)
        {
            FallDown();
        }
        UpdateBehaviour();
    }

    protected virtual void StartBehaviour() { }
    protected virtual void UpdateBehaviour() { }

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
        state = EnemyState.Dying;
        anim.Play("EnemyDespawn");
    }

    public void Poof()
    {
        state = EnemyState.Dying;
        Despawn();
    }

    public void Despawn()
    {
        state = EnemyState.Dead;
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

    public void Lasso()
    {
        if(state == EnemyState.Acting)
        {
            lassoMesh.SetActive(true);
            state = EnemyState.Lassoed;
        }
    }
}