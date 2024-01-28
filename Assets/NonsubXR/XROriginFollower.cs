using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace EcXR
{
    public class XROriginFollower : MonoBehaviour
    {
		private Transform xrFloorOffsetTransform;
		private void Awake()
		{
			xrFloorOffsetTransform = FindObjectOfType<XROrigin>().CameraFloorOffsetObject.transform;
		}

		private void LateUpdate()
		{
			transform.SetPositionAndRotation(xrFloorOffsetTransform.position, xrFloorOffsetTransform.rotation);
		}
	}
}
