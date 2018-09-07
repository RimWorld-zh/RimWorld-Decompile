using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[AddComponentMenu("Image Effects/Camera/Vignette and Chromatic Aberration")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class VignetteAndChromaticAberration : PostEffectsBase
	{
		public VignetteAndChromaticAberration.AberrationMode mode;

		public float intensity = 0.036f;

		public float chromaticAberration = 0.2f;

		public float axialAberration = 0.5f;

		public float blur;

		public float blurSpread = 0.75f;

		public float luminanceDependency = 0.25f;

		public float blurDistance = 2.5f;

		public Shader vignetteShader;

		public Shader separableBlurShader;

		public Shader chromAberrationShader;

		private Material m_VignetteMaterial;

		private Material m_SeparableBlurMaterial;

		private Material m_ChromAberrationMaterial;

		public VignetteAndChromaticAberration()
		{
		}

		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.m_VignetteMaterial = base.CheckShaderAndCreateMaterial(this.vignetteShader, this.m_VignetteMaterial);
			this.m_SeparableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.m_SeparableBlurMaterial);
			this.m_ChromAberrationMaterial = base.CheckShaderAndCreateMaterial(this.chromAberrationShader, this.m_ChromAberrationMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int width = source.width;
			int height = source.height;
			bool flag = Mathf.Abs(this.blur) > 0f || Mathf.Abs(this.intensity) > 0f;
			float num = 1f * (float)width / (1f * (float)height);
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			if (flag)
			{
				renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
				if (Mathf.Abs(this.blur) > 0f)
				{
					renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
					Graphics.Blit(source, renderTexture2, this.m_ChromAberrationMaterial, 0);
					for (int i = 0; i < 2; i++)
					{
						this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(0f, this.blurSpread * 0.001953125f, 0f, 0f));
						RenderTexture temporary = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
						Graphics.Blit(renderTexture2, temporary, this.m_SeparableBlurMaterial);
						RenderTexture.ReleaseTemporary(renderTexture2);
						this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * 0.001953125f / num, 0f, 0f, 0f));
						renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
						Graphics.Blit(temporary, renderTexture2, this.m_SeparableBlurMaterial);
						RenderTexture.ReleaseTemporary(temporary);
					}
				}
				this.m_VignetteMaterial.SetFloat("_Intensity", 1f / (1f - this.intensity) - 1f);
				this.m_VignetteMaterial.SetFloat("_Blur", 1f / (1f - this.blur) - 1f);
				this.m_VignetteMaterial.SetTexture("_VignetteTex", renderTexture2);
				Graphics.Blit(source, renderTexture, this.m_VignetteMaterial, 0);
			}
			this.m_ChromAberrationMaterial.SetFloat("_ChromaticAberration", this.chromaticAberration);
			this.m_ChromAberrationMaterial.SetFloat("_AxialAberration", this.axialAberration);
			this.m_ChromAberrationMaterial.SetVector("_BlurDistance", new Vector2(-this.blurDistance, this.blurDistance));
			this.m_ChromAberrationMaterial.SetFloat("_Luminance", 1f / Mathf.Max(Mathf.Epsilon, this.luminanceDependency));
			if (flag)
			{
				renderTexture.wrapMode = TextureWrapMode.Clamp;
			}
			else
			{
				source.wrapMode = TextureWrapMode.Clamp;
			}
			Graphics.Blit((!flag) ? source : renderTexture, destination, this.m_ChromAberrationMaterial, (this.mode != VignetteAndChromaticAberration.AberrationMode.Advanced) ? 1 : 2);
			RenderTexture.ReleaseTemporary(renderTexture);
			RenderTexture.ReleaseTemporary(renderTexture2);
		}

		public enum AberrationMode
		{
			Simple,
			Advanced
		}
	}
}
