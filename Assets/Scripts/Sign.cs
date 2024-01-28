using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Signpost : MonoBehaviour
{
    public UnityEvent hitEvent;

    // Start is called before the first frame update
    void Start()
    {
        hitEvent.Invoke();
    }
}
