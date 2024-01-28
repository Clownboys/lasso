using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardToCenter : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        FaceCenter();
    }

    public void FaceCenter()
    {
        Vector3 lookDirection = GameWrangler.Instance.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }
}
