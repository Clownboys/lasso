using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcXR
{
    public class ShadowLight : MonoBehaviour
    {
        void OnValidate()
        {
            Light light = GetComponent<Light>();

            light.color = new Color(-1, -1, -1);
            light.cullingMask = LayerMask.NameToLayer("Room");
        }
    }
}
