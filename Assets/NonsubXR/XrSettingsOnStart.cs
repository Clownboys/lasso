using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace EcXR
{
	public class XrSettingsOnStart : MonoBehaviour
	{
		[SerializeField] private bool passthroughOn = false;
		[SerializeField] private bool tryLoadRoomMesh = false;
		[SerializeField] private float renderScale = 1.0f;
		[SerializeField] private int targetFramerate = 72;

		[Header("Should only be 0, 2, 4, or 8!")]
		[SerializeField] private ushort antialiasingMsaa = 4;

		void Start()
		{
			PassthroughManager.SetPassthrough(passthroughOn);

#if USE_OCULUS_XR_PACKAGE
			OVRPlugin.systemDisplayFrequency = targetFramerate;
#endif

			if (tryLoadRoomMesh)
				RoomMeshManager.LoadRoomMesh();

#if USE_RENDER_PIPELINE_URP
			UniversalRenderPipelineAsset urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.defaultRenderPipeline;

			urpAsset.renderScale = renderScale;
			urpAsset.msaaSampleCount = antialiasingMsaa;
#else
			XRSettings.eyeTextureResolutionScale = renderScale;
			QualitySettings.antiAliasing = antialiasingMsaa;
#endif

		}
	}
}
