using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200018C RID: 396
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Contrast Stretch")]
	public class ContrastStretch : MonoBehaviour
	{
		// Token: 0x04000776 RID: 1910
		[Range(0.0001f, 1f)]
		public float adaptationSpeed = 0.02f;

		// Token: 0x04000777 RID: 1911
		[Range(0f, 1f)]
		public float limitMinimum = 0.2f;

		// Token: 0x04000778 RID: 1912
		[Range(0f, 1f)]
		public float limitMaximum = 0.6f;

		// Token: 0x04000779 RID: 1913
		private RenderTexture[] adaptRenderTex = new RenderTexture[2];

		// Token: 0x0400077A RID: 1914
		private int curAdaptIndex = 0;

		// Token: 0x0400077B RID: 1915
		public Shader shaderLum;

		// Token: 0x0400077C RID: 1916
		private Material m_materialLum;

		// Token: 0x0400077D RID: 1917
		public Shader shaderReduce;

		// Token: 0x0400077E RID: 1918
		private Material m_materialReduce;

		// Token: 0x0400077F RID: 1919
		public Shader shaderAdapt;

		// Token: 0x04000780 RID: 1920
		private Material m_materialAdapt;

		// Token: 0x04000781 RID: 1921
		public Shader shaderApply;

		// Token: 0x04000782 RID: 1922
		private Material m_materialApply;

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00013A2C File Offset: 0x00011C2C
		protected Material materialLum
		{
			get
			{
				if (this.m_materialLum == null)
				{
					this.m_materialLum = new Material(this.shaderLum);
					this.m_materialLum.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialLum;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00013A78 File Offset: 0x00011C78
		protected Material materialReduce
		{
			get
			{
				if (this.m_materialReduce == null)
				{
					this.m_materialReduce = new Material(this.shaderReduce);
					this.m_materialReduce.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialReduce;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00013AC4 File Offset: 0x00011CC4
		protected Material materialAdapt
		{
			get
			{
				if (this.m_materialAdapt == null)
				{
					this.m_materialAdapt = new Material(this.shaderAdapt);
					this.m_materialAdapt.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialAdapt;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x00013B10 File Offset: 0x00011D10
		protected Material materialApply
		{
			get
			{
				if (this.m_materialApply == null)
				{
					this.m_materialApply = new Material(this.shaderApply);
					this.m_materialApply.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialApply;
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00013B5C File Offset: 0x00011D5C
		private void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
			}
			else if (!this.shaderAdapt.isSupported || !this.shaderApply.isSupported || !this.shaderLum.isSupported || !this.shaderReduce.isSupported)
			{
				base.enabled = false;
			}
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00013BD0 File Offset: 0x00011DD0
		private void OnEnable()
		{
			for (int i = 0; i < 2; i++)
			{
				if (!this.adaptRenderTex[i])
				{
					this.adaptRenderTex[i] = new RenderTexture(1, 1, 0);
					this.adaptRenderTex[i].hideFlags = HideFlags.HideAndDontSave;
				}
			}
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00013C28 File Offset: 0x00011E28
		private void OnDisable()
		{
			for (int i = 0; i < 2; i++)
			{
				UnityEngine.Object.DestroyImmediate(this.adaptRenderTex[i]);
				this.adaptRenderTex[i] = null;
			}
			if (this.m_materialLum)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialLum);
			}
			if (this.m_materialReduce)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialReduce);
			}
			if (this.m_materialAdapt)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialAdapt);
			}
			if (this.m_materialApply)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialApply);
			}
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00013CCC File Offset: 0x00011ECC
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			RenderTexture renderTexture = RenderTexture.GetTemporary(source.width, source.height);
			Graphics.Blit(source, renderTexture, this.materialLum);
			while (renderTexture.width > 1 || renderTexture.height > 1)
			{
				int num = renderTexture.width / 2;
				if (num < 1)
				{
					num = 1;
				}
				int num2 = renderTexture.height / 2;
				if (num2 < 1)
				{
					num2 = 1;
				}
				RenderTexture temporary = RenderTexture.GetTemporary(num, num2);
				Graphics.Blit(renderTexture, temporary, this.materialReduce);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			this.CalculateAdaptation(renderTexture);
			this.materialApply.SetTexture("_AdaptTex", this.adaptRenderTex[this.curAdaptIndex]);
			Graphics.Blit(source, destination, this.materialApply);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00013D90 File Offset: 0x00011F90
		private void CalculateAdaptation(Texture curTexture)
		{
			int num = this.curAdaptIndex;
			this.curAdaptIndex = (this.curAdaptIndex + 1) % 2;
			float num2 = 1f - Mathf.Pow(1f - this.adaptationSpeed, 30f * Time.deltaTime);
			num2 = Mathf.Clamp(num2, 0.01f, 1f);
			this.materialAdapt.SetTexture("_CurTex", curTexture);
			this.materialAdapt.SetVector("_AdaptParams", new Vector4(num2, this.limitMinimum, this.limitMaximum, 0f));
			Graphics.SetRenderTarget(this.adaptRenderTex[this.curAdaptIndex]);
			GL.Clear(false, true, Color.black);
			Graphics.Blit(this.adaptRenderTex[num], this.adaptRenderTex[this.curAdaptIndex], this.materialAdapt);
		}
	}
}
