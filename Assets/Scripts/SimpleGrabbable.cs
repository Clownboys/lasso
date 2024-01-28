using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SimpleGrabbable : MonoBehaviour
{
    public Transform grabPoint, offHandGrabPoint;
    public ObiParticleAttachment grabAttachment, offHandGrabAttachment;
    public LassoHand leftHand, rightHand;
    Transform follow = null, followOffHand = null;
    public void OnSelectEntered(SelectEnterEventArgs args) {
        follow = args.interactorObject.transform;
        LassoHand followLasso = follow.GetComponent<LassoHand>();
        followOffHand = followLasso.left ? rightHand.transform : leftHand.transform;
    }

    public void OnSelectExit(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform == follow)
        {
            follow = null;
            followOffHand = null;
        }
    }

    private void Update()
    {
        if (follow != null)
        {
            transform.position = follow.position;
            offHandGrabPoint.position = followOffHand.position;
            grabAttachment.enabled = true;
            offHandGrabAttachment.enabled = true;
        }
        else
        {
            transform.position = grabPoint.position;
            grabAttachment.enabled = false;
            offHandGrabAttachment.enabled = false;
        }
    }

    public bool isGrabbed()
    {
        return follow != null;
    }
}
