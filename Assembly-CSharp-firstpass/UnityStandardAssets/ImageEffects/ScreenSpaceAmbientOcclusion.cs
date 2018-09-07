using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Occlusion")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class ScreenSpaceAmbientOcclusion : MonoBehaviour
	{
		[Range(0.05f, 1f)]
		public float m_Radius = 0.4f;

		public ScreenSpaceAmbientOcclusion.SSAOSamples m_SampleCount = ScreenSpaceAmbientOcclusion.SSAOSamples.Medium;

		[Range(0.5f, 4f)]
		public float m_OcclusionIntensity = 1.5f;

		[Range(0f, 4f)]
		public int m_Blur = 2;

		[Range(1f, 6f)]
		public int m_Downsampling = 2;

		[Range(0.2f, 2f)]
		public float m_OcclusionAttenuation = 1f;

		[Range(1E-05f, 0.5f)]
		public float m_MinZ = 0.01f;

		public Shader m_SSAOShader;

		private Material m_SSAOMaterial;

		public Texture2D m_RandomTexture;

		private bool m_Supported;

		public ScreenSpaceAmbientOcclusion()
		{
		}

		private static Material CreateMaterial(Shader shader)
		{
			if (!shader)
			{
				return null;
			}
			return new Material(shader)
			{
				hideFlags = HideFlags.HideAndDontSave
			};
		}

		private static void DestroyMaterial(Material mat)
		{
			if (mat)
			{
				UnityEngine.Object.DestroyImmediate(mat);
				mat = null;
			}
		}

		private void OnDisable()
		{
			ScreenSpaceAmbientOcclusion.DestroyMaterial(this.m_SSAOMaterial);
		}

		private void Start()
		{
			if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				this.m_Supported = false;
				base.enabled = false;
				return;
			}
			this.CreateMaterials();
			if (!this.m_SSAOMaterial || this.m_SSAOMaterial.passCount != 5)
			{
				this.m_Supported = false;
				base.enabled = false;
				return;
			}
			this.m_Supported = true;
		}

		private void OnEnable()
		{
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
		}

		private void CreateMaterials()
		{
			if (!this.m_SSAOMaterial && this.m_SSAOShader.isSupported)
			{
				this.m_SSAOMaterial = ScreenSpaceAmbientOcclusion.CreateMaterial(this.m_SSAOShader);
				this.m_SSAOMaterial.SetTexture("_RandomTexture", this.m_RandomTexture);
			}
		}

		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.m_Supported || !this.m_SSAOShader.isSupported)
			{
				base.enabled = false;
				return;
			}
			this.CreateMaterials();
			this.m_Downsampling = Mathf.Clamp(this.m_Downsampling, 1, 6);
			this.m_Radius = Mathf.Clamp(this.m_Radius, 0.05f, 1f);
			this.m_MinZ = Mathf.Clamp(this.m_MinZ, 1E-05f, 0.5f);
			this.m_OcclusionIntensity = Mathf.Clamp(this.m_OcclusionIntensity, 0.5f, 4f);
			this.m_OcclusionAttenuation = Mathf.Clamp(this.m_OcclusionAttenuation, 0.2f, 2f);
			this.m_Blur = Mathf.Clamp(this.m_Blur, 0, 4);
			RenderTexture renderTexture = RenderTexture.GetTemporary(source.width / this.m_Downsampling, source.height / this.m_Downsampling, 0);
			float fieldOfView = base.GetComponent<Camera>().fieldOfView;
			float farClipPlane = base.GetComponent<Camera>().farClipPlane;
			float num = Mathf.Tan(fieldOfView * 0.0174532924f * 0.5f) * farClipPlane;
			float x = num * base.GetComponent<Camera>().aspect;
			this.m_SSAOMaterial.SetVector("_FarCorner", new Vector3(x, num, farClipPlane));
			int num2;
			int num3;
			if (this.m_RandomTexture)
			{
				num2 = this.m_RandomTexture.width;
				num3 = this.m_RandomTexture.height;
			}
			else
			{
				num2 = 1;
				num3 = 1;
			}
			this.m_SSAOMaterial.SetVector("_NoiseScale", new Vector3((float)renderTexture.width / (float)num2, (float)renderTexture.height / (float)num3, 0f));
			this.m_SSAOMaterial.SetVector("_Params", new Vector4(this.m_Radius, this.m_MinZ, 1f / this.m_OcclusionAttenuation, this.m_OcclusionIntensity));
			bool flag = this.m_Blur > 0;
			Graphics.Blit((!flag) ? source : null, renderTexture, this.m_SSAOMaterial, (int)this.m_SampleCount);
			if (flag)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
				this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4((float)this.m_Blur / (float)source.width, 0f, 0f, 0f));
				this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
				Graphics.Blit(null, temporary, this.m_SSAOMaterial, 3);
				RenderTexture.ReleaseTemporary(renderTexture);
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
				this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(0f, (float)this.m_Blur / (float)source.height, 0f, 0f));
				this.m_SSAOMaterial.SetTexture("_SSAO", temporary);
				Graphics.Blit(source, temporary2, this.m_SSAOMaterial, 3);
				RenderTexture.ReleaseTemporary(temporary);
				renderTexture = temporary2;
			}
			this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
			Graphics.Blit(source, destination, this.m_SSAOMaterial, 4);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		public enum SSAOSamples
		{
			Low,
			Medium,
			High
		}
	}
}
