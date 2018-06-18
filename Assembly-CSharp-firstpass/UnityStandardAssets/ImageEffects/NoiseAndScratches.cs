using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200019F RID: 415
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Noise/Noise and Scratches")]
	public class NoiseAndScratches : MonoBehaviour
	{
		// Token: 0x06000936 RID: 2358 RVA: 0x000174C0 File Offset: 0x000156C0
		protected void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
			}
			else if (this.shaderRGB == null || this.shaderYUV == null)
			{
				Debug.Log("Noise shaders are not set up! Disabling noise effect.");
				base.enabled = false;
			}
			else if (!this.shaderRGB.isSupported)
			{
				base.enabled = false;
			}
			else if (!this.shaderYUV.isSupported)
			{
				this.rgbFallback = true;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x00017554 File Offset: 0x00015754
		protected Material material
		{
			get
			{
				if (this.m_MaterialRGB == null)
				{
					this.m_MaterialRGB = new Material(this.shaderRGB);
					this.m_MaterialRGB.hideFlags = HideFlags.HideAndDontSave;
				}
				if (this.m_MaterialYUV == null && !this.rgbFallback)
				{
					this.m_MaterialYUV = new Material(this.shaderYUV);
					this.m_MaterialYUV.hideFlags = HideFlags.HideAndDontSave;
				}
				return (this.rgbFallback || this.monochrome) ? this.m_MaterialRGB : this.m_MaterialYUV;
			}
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x000175FD File Offset: 0x000157FD
		protected void OnDisable()
		{
			if (this.m_MaterialRGB)
			{
				UnityEngine.Object.DestroyImmediate(this.m_MaterialRGB);
			}
			if (this.m_MaterialYUV)
			{
				UnityEngine.Object.DestroyImmediate(this.m_MaterialYUV);
			}
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00017638 File Offset: 0x00015838
		private void SanitizeParameters()
		{
			this.grainIntensityMin = Mathf.Clamp(this.grainIntensityMin, 0f, 5f);
			this.grainIntensityMax = Mathf.Clamp(this.grainIntensityMax, 0f, 5f);
			this.scratchIntensityMin = Mathf.Clamp(this.scratchIntensityMin, 0f, 5f);
			this.scratchIntensityMax = Mathf.Clamp(this.scratchIntensityMax, 0f, 5f);
			this.scratchFPS = Mathf.Clamp(this.scratchFPS, 1f, 30f);
			this.scratchJitter = Mathf.Clamp(this.scratchJitter, 0f, 1f);
			this.grainSize = Mathf.Clamp(this.grainSize, 0.1f, 50f);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00017704 File Offset: 0x00015904
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.SanitizeParameters();
			if (this.scratchTimeLeft <= 0f)
			{
				this.scratchTimeLeft = UnityEngine.Random.value * 2f / this.scratchFPS;
				this.scratchX = UnityEngine.Random.value;
				this.scratchY = UnityEngine.Random.value;
			}
			this.scratchTimeLeft -= Time.deltaTime;
			Material material = this.material;
			material.SetTexture("_GrainTex", this.grainTexture);
			material.SetTexture("_ScratchTex", this.scratchTexture);
			float num = 1f / this.grainSize;
			material.SetVector("_GrainOffsetScale", new Vector4(UnityEngine.Random.value, UnityEngine.Random.value, (float)Screen.width / (float)this.grainTexture.width * num, (float)Screen.height / (float)this.grainTexture.height * num));
			material.SetVector("_ScratchOffsetScale", new Vector4(this.scratchX + UnityEngine.Random.value * this.scratchJitter, this.scratchY + UnityEngine.Random.value * this.scratchJitter, (float)Screen.width / (float)this.scratchTexture.width, (float)Screen.height / (float)this.scratchTexture.height));
			material.SetVector("_Intensity", new Vector4(UnityEngine.Random.Range(this.grainIntensityMin, this.grainIntensityMax), UnityEngine.Random.Range(this.scratchIntensityMin, this.scratchIntensityMax), 0f, 0f));
			Graphics.Blit(source, destination, material);
		}

		// Token: 0x0400081D RID: 2077
		public bool monochrome = true;

		// Token: 0x0400081E RID: 2078
		private bool rgbFallback = false;

		// Token: 0x0400081F RID: 2079
		[Range(0f, 5f)]
		public float grainIntensityMin = 0.1f;

		// Token: 0x04000820 RID: 2080
		[Range(0f, 5f)]
		public float grainIntensityMax = 0.2f;

		// Token: 0x04000821 RID: 2081
		[Range(0.1f, 50f)]
		public float grainSize = 2f;

		// Token: 0x04000822 RID: 2082
		[Range(0f, 5f)]
		public float scratchIntensityMin = 0.05f;

		// Token: 0x04000823 RID: 2083
		[Range(0f, 5f)]
		public float scratchIntensityMax = 0.25f;

		// Token: 0x04000824 RID: 2084
		[Range(1f, 30f)]
		public float scratchFPS = 10f;

		// Token: 0x04000825 RID: 2085
		[Range(0f, 1f)]
		public float scratchJitter = 0.01f;

		// Token: 0x04000826 RID: 2086
		public Texture grainTexture;

		// Token: 0x04000827 RID: 2087
		public Texture scratchTexture;

		// Token: 0x04000828 RID: 2088
		public Shader shaderRGB;

		// Token: 0x04000829 RID: 2089
		public Shader shaderYUV;

		// Token: 0x0400082A RID: 2090
		private Material m_MaterialRGB;

		// Token: 0x0400082B RID: 2091
		private Material m_MaterialYUV;

		// Token: 0x0400082C RID: 2092
		private float scratchTimeLeft = 0f;

		// Token: 0x0400082D RID: 2093
		private float scratchX;

		// Token: 0x0400082E RID: 2094
		private float scratchY;
	}
}
