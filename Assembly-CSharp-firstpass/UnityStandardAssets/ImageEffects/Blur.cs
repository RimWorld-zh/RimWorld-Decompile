using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000182 RID: 386
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Blur/Blur")]
	public class Blur : MonoBehaviour
	{
		// Token: 0x0400071E RID: 1822
		[Range(0f, 10f)]
		public int iterations = 3;

		// Token: 0x0400071F RID: 1823
		[Range(0f, 1f)]
		public float blurSpread = 0.6f;

		// Token: 0x04000720 RID: 1824
		public Shader blurShader = null;

		// Token: 0x04000721 RID: 1825
		private static Material m_Material = null;

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x00011978 File Offset: 0x0000FB78
		protected Material material
		{
			get
			{
				if (Blur.m_Material == null)
				{
					Blur.m_Material = new Material(this.blurShader);
					Blur.m_Material.hideFlags = HideFlags.DontSave;
				}
				return Blur.m_Material;
			}
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x000119C0 File Offset: 0x0000FBC0
		protected void OnDisable()
		{
			if (Blur.m_Material)
			{
				UnityEngine.Object.DestroyImmediate(Blur.m_Material);
			}
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x000119E0 File Offset: 0x0000FBE0
		protected void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
			}
			else if (!this.blurShader || !this.material.shader.isSupported)
			{
				base.enabled = false;
			}
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00011A38 File Offset: 0x0000FC38
		public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
		{
			float num = 0.5f + (float)iteration * this.blurSpread;
			Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
			{
				new Vector2(-num, -num),
				new Vector2(-num, num),
				new Vector2(num, num),
				new Vector2(num, -num)
			});
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00011ABC File Offset: 0x0000FCBC
		private void DownSample4x(RenderTexture source, RenderTexture dest)
		{
			float num = 1f;
			Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
			{
				new Vector2(-num, -num),
				new Vector2(-num, num),
				new Vector2(num, num),
				new Vector2(num, -num)
			});
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00011B34 File Offset: 0x0000FD34
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			int width = source.width / 4;
			int height = source.height / 4;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
			this.DownSample4x(source, renderTexture);
			for (int i = 0; i < this.iterations; i++)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
				this.FourTapCone(renderTexture, temporary, i);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			Graphics.Blit(renderTexture, destination);
			RenderTexture.ReleaseTemporary(renderTexture);
		}
	}
}
