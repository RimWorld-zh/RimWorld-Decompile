using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020001A6 RID: 422
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Occlusion")]
	public class ScreenSpaceAmbientOcclusion : MonoBehaviour
	{
		// Token: 0x04000847 RID: 2119
		[Range(0.05f, 1f)]
		public float m_Radius = 0.4f;

		// Token: 0x04000848 RID: 2120
		public ScreenSpaceAmbientOcclusion.SSAOSamples m_SampleCount = ScreenSpaceAmbientOcclusion.SSAOSamples.Medium;

		// Token: 0x04000849 RID: 2121
		[Range(0.5f, 4f)]
		public float m_OcclusionIntensity = 1.5f;

		// Token: 0x0400084A RID: 2122
		[Range(0f, 4f)]
		public int m_Blur = 2;

		// Token: 0x0400084B RID: 2123
		[Range(1f, 6f)]
		public int m_Downsampling = 2;

		// Token: 0x0400084C RID: 2124
		[Range(0.2f, 2f)]
		public float m_OcclusionAttenuation = 1f;

		// Token: 0x0400084D RID: 2125
		[Range(1E-05f, 0.5f)]
		public float m_MinZ = 0.01f;

		// Token: 0x0400084E RID: 2126
		public Shader m_SSAOShader;

		// Token: 0x0400084F RID: 2127
		private Material m_SSAOMaterial;

		// Token: 0x04000850 RID: 2128
		public Texture2D m_RandomTexture;

		// Token: 0x04000851 RID: 2129
		private bool m_Supported;

		// Token: 0x0600095C RID: 2396 RVA: 0x000185F4 File Offset: 0x000167F4
		private static Material CreateMaterial(Shader shader)
		{
			Material result;
			if (!shader)
			{
				result = null;
			}
			else
			{
				result = new Material(shader)
				{
					hideFlags = HideFlags.HideAndDontSave
				};
			}
			return result;
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0001862B File Offset: 0x0001682B
		private static void DestroyMaterial(Material mat)
		{
			if (mat)
			{
				UnityEngine.Object.DestroyImmediate(mat);
				mat = null;
			}
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00018644 File Offset: 0x00016844
		private void OnDisable()
		{
			ScreenSpaceAmbientOcclusion.DestroyMaterial(this.m_SSAOMaterial);
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00018654 File Offset: 0x00016854
		private void Start()
		{
			if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				this.m_Supported = false;
				base.enabled = false;
			}
			else
			{
				this.CreateMaterials();
				if (!this.m_SSAOMaterial || this.m_SSAOMaterial.passCount != 5)
				{
					this.m_Supported = false;
					base.enabled = false;
				}
				else
				{
					this.m_Supported = true;
				}
			}
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x000186CD File Offset: 0x000168CD
		private void OnEnable()
		{
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x000186E4 File Offset: 0x000168E4
		private void CreateMaterials()
		{
			if (!this.m_SSAOMaterial && this.m_SSAOShader.isSupported)
			{
				this.m_SSAOMaterial = ScreenSpaceAmbientOcclusion.CreateMaterial(this.m_SSAOShader);
				this.m_SSAOMaterial.SetTexture("_RandomTexture", this.m_RandomTexture);
			}
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0001873C File Offset: 0x0001693C
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.m_Supported || !this.m_SSAOShader.isSupported)
			{
				base.enabled = false;
			}
			else
			{
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
		}

		// Token: 0x020001A7 RID: 423
		public enum SSAOSamples
		{
			// Token: 0x04000853 RID: 2131
			Low,
			// Token: 0x04000854 RID: 2132
			Medium,
			// Token: 0x04000855 RID: 2133
			High
		}
	}
}
