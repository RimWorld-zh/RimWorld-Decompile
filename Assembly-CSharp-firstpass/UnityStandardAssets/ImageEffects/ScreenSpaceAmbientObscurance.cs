using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020001A5 RID: 421
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Obscurance")]
	internal class ScreenSpaceAmbientObscurance : PostEffectsBase
	{
		// Token: 0x06000958 RID: 2392 RVA: 0x000182BC File Offset: 0x000164BC
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.aoMaterial = base.CheckShaderAndCreateMaterial(this.aoShader, this.aoMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x00018308 File Offset: 0x00016508
		private void OnDisable()
		{
			if (this.aoMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.aoMaterial);
			}
			this.aoMaterial = null;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x00018330 File Offset: 0x00016530
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				Matrix4x4 projectionMatrix = base.GetComponent<Camera>().projectionMatrix;
				Matrix4x4 inverse = projectionMatrix.inverse;
				Vector4 value = new Vector4(-2f / ((float)Screen.width * projectionMatrix[0]), -2f / ((float)Screen.height * projectionMatrix[5]), (1f - projectionMatrix[2]) / projectionMatrix[0], (1f + projectionMatrix[6]) / projectionMatrix[5]);
				this.aoMaterial.SetVector("_ProjInfo", value);
				this.aoMaterial.SetMatrix("_ProjectionInv", inverse);
				this.aoMaterial.SetTexture("_Rand", this.rand);
				this.aoMaterial.SetFloat("_Radius", this.radius);
				this.aoMaterial.SetFloat("_Radius2", this.radius * this.radius);
				this.aoMaterial.SetFloat("_Intensity", this.intensity);
				this.aoMaterial.SetFloat("_BlurFilterDistance", this.blurFilterDistance);
				int width = source.width;
				int height = source.height;
				RenderTexture renderTexture = RenderTexture.GetTemporary(width >> this.downsample, height >> this.downsample);
				Graphics.Blit(source, renderTexture, this.aoMaterial, 0);
				if (this.downsample > 0)
				{
					RenderTexture temporary = RenderTexture.GetTemporary(width, height);
					Graphics.Blit(renderTexture, temporary, this.aoMaterial, 4);
					RenderTexture.ReleaseTemporary(renderTexture);
					renderTexture = temporary;
				}
				for (int i = 0; i < this.blurIterations; i++)
				{
					this.aoMaterial.SetVector("_Axis", new Vector2(1f, 0f));
					RenderTexture temporary = RenderTexture.GetTemporary(width, height);
					Graphics.Blit(renderTexture, temporary, this.aoMaterial, 1);
					RenderTexture.ReleaseTemporary(renderTexture);
					this.aoMaterial.SetVector("_Axis", new Vector2(0f, 1f));
					renderTexture = RenderTexture.GetTemporary(width, height);
					Graphics.Blit(temporary, renderTexture, this.aoMaterial, 1);
					RenderTexture.ReleaseTemporary(temporary);
				}
				this.aoMaterial.SetTexture("_AOTex", renderTexture);
				Graphics.Blit(source, destination, this.aoMaterial, 2);
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}

		// Token: 0x0400083F RID: 2111
		[Range(0f, 3f)]
		public float intensity = 0.5f;

		// Token: 0x04000840 RID: 2112
		[Range(0.1f, 3f)]
		public float radius = 0.2f;

		// Token: 0x04000841 RID: 2113
		[Range(0f, 3f)]
		public int blurIterations = 1;

		// Token: 0x04000842 RID: 2114
		[Range(0f, 5f)]
		public float blurFilterDistance = 1.25f;

		// Token: 0x04000843 RID: 2115
		[Range(0f, 1f)]
		public int downsample = 0;

		// Token: 0x04000844 RID: 2116
		public Texture2D rand = null;

		// Token: 0x04000845 RID: 2117
		public Shader aoShader = null;

		// Token: 0x04000846 RID: 2118
		private Material aoMaterial = null;
	}
}
