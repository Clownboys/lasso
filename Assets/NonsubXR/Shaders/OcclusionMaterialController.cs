using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcXR
{
    public class OcclusionMaterialController : MonoBehaviour
    {
        [SerializeField] Material occlusionMaterial;

        private int pulseId;

        public float timeSpeed = 1;
        private float radius = 0;
        private Vector3 position;

        [SerializeField] private bool pulseOnStart = false;

		private void Start()
		{
            pulseId = Shader.PropertyToID("_PulsePositionAndTime");

            if(pulseOnStart)
            {
                StartPulse();
            }
		}

        public void StartPulse(Transform startFrom)
        {
            this.position = startFrom.position;
            radius = 0;
        }

		public void StartPulse()
		{
            StartPulse(transform);
		}

		private void LateUpdate()
        {
            if (radius > 50)
                return;

            radius += Time.deltaTime * timeSpeed;
            occlusionMaterial.SetVector(pulseId, new Vector4(position.x, position.y, position.z, radius));
		}
    }
}
