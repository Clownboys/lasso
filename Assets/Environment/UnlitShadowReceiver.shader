Shader "CoreGame/UnlitWithShadows"
{
	Properties
	{
		_BaseMap("Texture", 2D) = "white" {}
		_BaseColor("Color", Color) = (1, 1, 1, 1)
		_Cutoff("AlphaCutout", Range(0.0, 1.0)) = 0.5
		_ShadowColor ("Shadow Color", Color) = (0.35,0.4,0.45,1.0)
 
		// BlendMode
		[HideInInspector] _Surface("__surface", Float) = 0.0
		[HideInInspector] _Blend("__blend", Float) = 0.0
		[HideInInspector] _AlphaClip("__clip", Float) = 0.0
		[HideInInspector] _SrcBlend("Src", Float) = 1.0
		[HideInInspector] _DstBlend("Dst", Float) = 0.0
		[HideInInspector] _ZWrite("ZWrite", Float) = 1.0
		[HideInInspector] _Cull("__cull", Float) = 2.0
 
		// Editmode props
		[HideInInspector] _QueueOffset("Queue offset", Float) = 0.0
 
		// ObsoleteProperties
		[HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
		[HideInInspector] _Color("Base Color", Color) = (0.5, 0.5, 0.5, 1)
		[HideInInspector] _SampleGI("SampleGI", float) = 0.0 // needed from bakedlit
	}
 
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"IgnoreProjector" = "True"
			"RenderPipeline" = "UniversalPipeline"
		}
		LOD 100
 
		Blend [_SrcBlend][_DstBlend]
		ZWrite [_ZWrite]
		Cull [_Cull]
 
		  //  Blend DstColor Zero, Zero One
		 //   Cull Back
			ZTest LEqual
			//ZWrite Off
 
		Pass
		{
			Name "Unlit"
			HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
 
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
   
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHAPREMULTIPLY_ON
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
 
			// -------------------------------------
			// Unity defined keywords
			#pragma multi_compile_instancing
 
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


 
			CBUFFER_START(UnityPerMaterial)
			float4 _ShadowColor;
			CBUFFER_END
 
			struct Attributes
			{
				float4 positionOS       : POSITION;
				float2 uv               : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
 
			struct Varyings
			{
				float4 positionCS               : SV_POSITION;
				float3 positionWS               : TEXCOORD2;
				float2 uv                       : TEXCOORD0;
 
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			texture2D _BaseMap;
			SamplerState sampler_BaseMap;
			float4 _BaseMap_ST;
 
			Varyings vert(Attributes input)
			{
				Varyings output = (Varyings)0;
 
				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_TRANSFER_INSTANCE_ID(input, output);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
 
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
				output.positionCS = vertexInput.positionCS;
				output.positionWS = vertexInput.positionWS;
				output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
 
				return output;
			}
 
			half4 frag(Varyings input) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
 
				half2 uv = input.uv;
				half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
				half4 color = texColor;
			   // half3 color = texColor.rgb * _BaseColor.rgb;
 
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = input.positionWS;
				float4 shadowCoord = GetShadowCoord(vertexInput);
				half shadowAttenutation = MainLightRealtimeShadow(shadowCoord);
				color = lerp(color, _ShadowColor, (1.0 - shadowAttenutation) * _ShadowColor.a);
 
			   // color = MixFog(color, input.fogCoord);
 
				return color;
			}
			ENDHLSL
 
		}
 
		// This pass it not used during regular rendering, only for lightmap baking.
		Pass
		{
			Name "Meta"
			Tags{"LightMode" = "Meta"}
 
			Cull Off
 
			HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma vertex UniversalVertexMeta
			#pragma fragment UniversalFragmentMetaUnlit
 
			#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitMetaPass.hlsl"
 
			ENDHLSL
		}
	}
	FallBack "Hidden/Universal Render Pipeline/FallbackError"
	//CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.UnlitShader"
}