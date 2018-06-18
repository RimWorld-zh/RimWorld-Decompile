using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000191 RID: 401
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Depth of Field (deprecated)")]
	public class DepthOfFieldDeprecated : PostEffectsBase
	{
		// Token: 0x06000906 RID: 2310 RVA: 0x00015174 File Offset: 0x00013374
		private void CreateMaterials()
		{
			this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
			this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
			this.bokehSupport = this.bokehShader.isSupported;
			if (this.bokeh && this.bokehSupport && this.bokehShader)
			{
				this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
			}
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00015204 File Offset: 0x00013404
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
			this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
			this.bokehSupport = this.bokehShader.isSupported;
			if (this.bokeh && this.bokehSupport && this.bokehShader)
			{
				this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x000152B7 File Offset: 0x000134B7
		private void OnDisable()
		{
			Quads.Cleanup();
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x000152BF File Offset: 0x000134BF
		private void OnEnable()
		{
			this._camera = base.GetComponent<Camera>();
			this._camera.depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x000152E4 File Offset: 0x000134E4
		private float FocalDistance01(float worldDist)
		{
			return this._camera.WorldToViewportPoint((worldDist - this._camera.nearClipPlane) * this._camera.transform.forward + this._camera.transform.position).z / (this._camera.farClipPlane - this._camera.nearClipPlane);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0001535C File Offset: 0x0001355C
		private int GetDividerBasedOnQuality()
		{
			int result = 1;
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.Medium)
			{
				result = 2;
			}
			else if (this.resolution == DepthOfFieldDeprecated.DofResolution.Low)
			{
				result = 2;
			}
			return result;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00015398 File Offset: 0x00013598
		private int GetLowResolutionDividerBasedOnQuality(int baseDivider)
		{
			int num = baseDivider;
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.High)
			{
				num *= 2;
			}
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.Low)
			{
				num *= 2;
			}
			return num;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x000153D0 File Offset: 0x000135D0
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				if (this.smoothness < 0.1f)
				{
					this.smoothness = 0.1f;
				}
				this.bokeh = (this.bokeh && this.bokehSupport);
				float num = (!this.bokeh) ? 1f : DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR;
				bool flag = this.quality > DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground;
				float num2 = this.focalSize / (this._camera.farClipPlane - this._camera.nearClipPlane);
				if (this.simpleTweakMode)
				{
					this.focalDistance01 = ((!this.objectFocus) ? this.FocalDistance01(this.focalPoint) : (this._camera.WorldToViewportPoint(this.objectFocus.position).z / this._camera.farClipPlane));
					this.focalStartCurve = this.focalDistance01 * this.smoothness;
					this.focalEndCurve = this.focalStartCurve;
					flag = (flag && this.focalPoint > this._camera.nearClipPlane + Mathf.Epsilon);
				}
				else
				{
					if (this.objectFocus)
					{
						Vector3 vector = this._camera.WorldToViewportPoint(this.objectFocus.position);
						vector.z /= this._camera.farClipPlane;
						this.focalDistance01 = vector.z;
					}
					else
					{
						this.focalDistance01 = this.FocalDistance01(this.focalZDistance);
					}
					this.focalStartCurve = this.focalZStartCurve;
					this.focalEndCurve = this.focalZEndCurve;
					flag = (flag && this.focalPoint > this._camera.nearClipPlane + Mathf.Epsilon);
				}
				this.widthOverHeight = 1f * (float)source.width / (1f * (float)source.height);
				this.oneOverBaseSize = 0.001953125f;
				this.dofMaterial.SetFloat("_ForegroundBlurExtrude", this.foregroundBlurExtrude);
				this.dofMaterial.SetVector("_CurveParams", new Vector4((!this.simpleTweakMode) ? this.focalStartCurve : (1f / this.focalStartCurve), (!this.simpleTweakMode) ? this.focalEndCurve : (1f / this.focalEndCurve), num2 * 0.5f, this.focalDistance01));
				this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * (float)source.width), 1f / (1f * (float)source.height), 0f, 0f));
				int dividerBasedOnQuality = this.GetDividerBasedOnQuality();
				int lowResolutionDividerBasedOnQuality = this.GetLowResolutionDividerBasedOnQuality(dividerBasedOnQuality);
				this.AllocateTextures(flag, source, dividerBasedOnQuality, lowResolutionDividerBasedOnQuality);
				Graphics.Blit(source, source, this.dofMaterial, 3);
				this.Downsample(source, this.mediumRezWorkTexture);
				this.Blur(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 4, this.maxBlurSpread);
				if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
				{
					this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast, this.bokehThresholdLuminance, 0.95f, 0f));
					Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
					Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
					this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread * num);
				}
				else
				{
					this.Downsample(this.mediumRezWorkTexture, this.lowRezWorkTexture);
					this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread);
				}
				this.dofBlurMaterial.SetTexture("_TapLow", this.lowRezWorkTexture);
				this.dofBlurMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
				Graphics.Blit(null, this.finalDefocus, this.dofBlurMaterial, 3);
				if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
				{
					this.AddBokeh(this.bokehSource2, this.bokehSource, this.finalDefocus);
				}
				this.dofMaterial.SetTexture("_TapLowBackground", this.finalDefocus);
				this.dofMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
				Graphics.Blit(source, (!flag) ? destination : this.foregroundTexture, this.dofMaterial, (!this.visualize) ? 0 : 2);
				if (flag)
				{
					Graphics.Blit(this.foregroundTexture, source, this.dofMaterial, 5);
					this.Downsample(source, this.mediumRezWorkTexture);
					this.BlurFg(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 2, this.maxBlurSpread);
					if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
					{
						this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast * 0.5f, this.bokehThresholdLuminance, 0f, 0f));
						Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
						Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
						this.BlurFg(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread * num);
					}
					else
					{
						this.BlurFg(this.mediumRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread);
					}
					Graphics.Blit(this.lowRezWorkTexture, this.finalDefocus);
					this.dofMaterial.SetTexture("_TapLowForeground", this.finalDefocus);
					Graphics.Blit(source, destination, this.dofMaterial, (!this.visualize) ? 4 : 1);
					if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
					{
						this.AddBokeh(this.bokehSource2, this.bokehSource, destination);
					}
				}
				this.ReleaseTextures();
			}
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00015A04 File Offset: 0x00013C04
		private void Blur(RenderTexture from, RenderTexture to, DepthOfFieldDeprecated.DofBlurriness iterations, int blurPass, float spread)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
			if (iterations > DepthOfFieldDeprecated.DofBlurriness.Low)
			{
				this.BlurHex(from, to, blurPass, spread, temporary);
				if (iterations > DepthOfFieldDeprecated.DofBlurriness.High)
				{
					this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
					Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
					this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
					Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
				}
			}
			else
			{
				this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
				Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
				this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
				Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00015B50 File Offset: 0x00013D50
		private void BlurFg(RenderTexture from, RenderTexture to, DepthOfFieldDeprecated.DofBlurriness iterations, int blurPass, float spread)
		{
			this.dofBlurMaterial.SetTexture("_TapHigh", from);
			RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
			if (iterations > DepthOfFieldDeprecated.DofBlurriness.Low)
			{
				this.BlurHex(from, to, blurPass, spread, temporary);
				if (iterations > DepthOfFieldDeprecated.DofBlurriness.High)
				{
					this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
					Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
					this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
					Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
				}
			}
			else
			{
				this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
				Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
				this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
				Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00015CAC File Offset: 0x00013EAC
		private void BlurHex(RenderTexture from, RenderTexture to, int blurPass, float spread, RenderTexture tmp)
		{
			this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(from, tmp, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
			Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(to, tmp, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, -spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00015DC8 File Offset: 0x00013FC8
		private void Downsample(RenderTexture from, RenderTexture to)
		{
			this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * (float)to.width), 1f / (1f * (float)to.height), 0f, 0f));
			Graphics.Blit(from, to, this.dofMaterial, DepthOfFieldDeprecated.SMOOTH_DOWNSAMPLE_PASS);
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00015E30 File Offset: 0x00014030
		private void AddBokeh(RenderTexture bokehInfo, RenderTexture tempTex, RenderTexture finalTarget)
		{
			if (this.bokehMaterial)
			{
				Mesh[] meshes = Quads.GetMeshes(tempTex.width, tempTex.height);
				RenderTexture.active = tempTex;
				GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
				GL.PushMatrix();
				GL.LoadIdentity();
				bokehInfo.filterMode = FilterMode.Point;
				float num = (float)bokehInfo.width * 1f / ((float)bokehInfo.height * 1f);
				float num2 = 2f / (1f * (float)bokehInfo.width);
				num2 += this.bokehScale * this.maxBlurSpread * DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR * this.oneOverBaseSize;
				this.bokehMaterial.SetTexture("_Source", bokehInfo);
				this.bokehMaterial.SetTexture("_MainTex", this.bokehTexture);
				this.bokehMaterial.SetVector("_ArScale", new Vector4(num2, num2 * num, 0.5f, 0.5f * num));
				this.bokehMaterial.SetFloat("_Intensity", this.bokehIntensity);
				this.bokehMaterial.SetPass(0);
				foreach (Mesh mesh in meshes)
				{
					if (mesh)
					{
						Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
					}
				}
				GL.PopMatrix();
				Graphics.Blit(tempTex, finalTarget, this.dofMaterial, 8);
				bokehInfo.filterMode = FilterMode.Bilinear;
			}
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00015FA8 File Offset: 0x000141A8
		private void ReleaseTextures()
		{
			if (this.foregroundTexture)
			{
				RenderTexture.ReleaseTemporary(this.foregroundTexture);
			}
			if (this.finalDefocus)
			{
				RenderTexture.ReleaseTemporary(this.finalDefocus);
			}
			if (this.mediumRezWorkTexture)
			{
				RenderTexture.ReleaseTemporary(this.mediumRezWorkTexture);
			}
			if (this.lowRezWorkTexture)
			{
				RenderTexture.ReleaseTemporary(this.lowRezWorkTexture);
			}
			if (this.bokehSource)
			{
				RenderTexture.ReleaseTemporary(this.bokehSource);
			}
			if (this.bokehSource2)
			{
				RenderTexture.ReleaseTemporary(this.bokehSource2);
			}
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00016058 File Offset: 0x00014258
		private void AllocateTextures(bool blurForeground, RenderTexture source, int divider, int lowTexDivider)
		{
			this.foregroundTexture = null;
			if (blurForeground)
			{
				this.foregroundTexture = RenderTexture.GetTemporary(source.width, source.height, 0);
			}
			this.mediumRezWorkTexture = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
			this.finalDefocus = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
			this.lowRezWorkTexture = RenderTexture.GetTemporary(source.width / lowTexDivider, source.height / lowTexDivider, 0);
			this.bokehSource = null;
			this.bokehSource2 = null;
			if (this.bokeh)
			{
				this.bokehSource = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
				this.bokehSource2 = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
				this.bokehSource.filterMode = FilterMode.Bilinear;
				this.bokehSource2.filterMode = FilterMode.Bilinear;
				RenderTexture.active = this.bokehSource2;
				GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
			}
			source.filterMode = FilterMode.Bilinear;
			this.finalDefocus.filterMode = FilterMode.Bilinear;
			this.mediumRezWorkTexture.filterMode = FilterMode.Bilinear;
			this.lowRezWorkTexture.filterMode = FilterMode.Bilinear;
			if (this.foregroundTexture)
			{
				this.foregroundTexture.filterMode = FilterMode.Bilinear;
			}
		}

		// Token: 0x040007AC RID: 1964
		private static int SMOOTH_DOWNSAMPLE_PASS = 6;

		// Token: 0x040007AD RID: 1965
		private static float BOKEH_EXTRA_BLUR = 2f;

		// Token: 0x040007AE RID: 1966
		public DepthOfFieldDeprecated.Dof34QualitySetting quality = DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground;

		// Token: 0x040007AF RID: 1967
		public DepthOfFieldDeprecated.DofResolution resolution = DepthOfFieldDeprecated.DofResolution.Low;

		// Token: 0x040007B0 RID: 1968
		public bool simpleTweakMode = true;

		// Token: 0x040007B1 RID: 1969
		public float focalPoint = 1f;

		// Token: 0x040007B2 RID: 1970
		public float smoothness = 0.5f;

		// Token: 0x040007B3 RID: 1971
		public float focalZDistance = 0f;

		// Token: 0x040007B4 RID: 1972
		public float focalZStartCurve = 1f;

		// Token: 0x040007B5 RID: 1973
		public float focalZEndCurve = 1f;

		// Token: 0x040007B6 RID: 1974
		private float focalStartCurve = 2f;

		// Token: 0x040007B7 RID: 1975
		private float focalEndCurve = 2f;

		// Token: 0x040007B8 RID: 1976
		private float focalDistance01 = 0.1f;

		// Token: 0x040007B9 RID: 1977
		public Transform objectFocus = null;

		// Token: 0x040007BA RID: 1978
		public float focalSize = 0f;

		// Token: 0x040007BB RID: 1979
		public DepthOfFieldDeprecated.DofBlurriness bluriness = DepthOfFieldDeprecated.DofBlurriness.High;

		// Token: 0x040007BC RID: 1980
		public float maxBlurSpread = 1.75f;

		// Token: 0x040007BD RID: 1981
		public float foregroundBlurExtrude = 1.15f;

		// Token: 0x040007BE RID: 1982
		public Shader dofBlurShader;

		// Token: 0x040007BF RID: 1983
		private Material dofBlurMaterial = null;

		// Token: 0x040007C0 RID: 1984
		public Shader dofShader;

		// Token: 0x040007C1 RID: 1985
		private Material dofMaterial = null;

		// Token: 0x040007C2 RID: 1986
		public bool visualize = false;

		// Token: 0x040007C3 RID: 1987
		public DepthOfFieldDeprecated.BokehDestination bokehDestination = DepthOfFieldDeprecated.BokehDestination.Background;

		// Token: 0x040007C4 RID: 1988
		private float widthOverHeight = 1.25f;

		// Token: 0x040007C5 RID: 1989
		private float oneOverBaseSize = 0.001953125f;

		// Token: 0x040007C6 RID: 1990
		public bool bokeh = false;

		// Token: 0x040007C7 RID: 1991
		public bool bokehSupport = true;

		// Token: 0x040007C8 RID: 1992
		public Shader bokehShader;

		// Token: 0x040007C9 RID: 1993
		public Texture2D bokehTexture;

		// Token: 0x040007CA RID: 1994
		public float bokehScale = 2.4f;

		// Token: 0x040007CB RID: 1995
		public float bokehIntensity = 0.15f;

		// Token: 0x040007CC RID: 1996
		public float bokehThresholdContrast = 0.1f;

		// Token: 0x040007CD RID: 1997
		public float bokehThresholdLuminance = 0.55f;

		// Token: 0x040007CE RID: 1998
		public int bokehDownsample = 1;

		// Token: 0x040007CF RID: 1999
		private Material bokehMaterial;

		// Token: 0x040007D0 RID: 2000
		private Camera _camera;

		// Token: 0x040007D1 RID: 2001
		private RenderTexture foregroundTexture = null;

		// Token: 0x040007D2 RID: 2002
		private RenderTexture mediumRezWorkTexture = null;

		// Token: 0x040007D3 RID: 2003
		private RenderTexture finalDefocus = null;

		// Token: 0x040007D4 RID: 2004
		private RenderTexture lowRezWorkTexture = null;

		// Token: 0x040007D5 RID: 2005
		private RenderTexture bokehSource = null;

		// Token: 0x040007D6 RID: 2006
		private RenderTexture bokehSource2 = null;

		// Token: 0x02000192 RID: 402
		public enum Dof34QualitySetting
		{
			// Token: 0x040007D8 RID: 2008
			OnlyBackground = 1,
			// Token: 0x040007D9 RID: 2009
			BackgroundAndForeground
		}

		// Token: 0x02000193 RID: 403
		public enum DofResolution
		{
			// Token: 0x040007DB RID: 2011
			High = 2,
			// Token: 0x040007DC RID: 2012
			Medium,
			// Token: 0x040007DD RID: 2013
			Low
		}

		// Token: 0x02000194 RID: 404
		public enum DofBlurriness
		{
			// Token: 0x040007DF RID: 2015
			Low = 1,
			// Token: 0x040007E0 RID: 2016
			High,
			// Token: 0x040007E1 RID: 2017
			VeryHigh = 4
		}

		// Token: 0x02000195 RID: 405
		public enum BokehDestination
		{
			// Token: 0x040007E3 RID: 2019
			Background = 1,
			// Token: 0x040007E4 RID: 2020
			Foreground,
			// Token: 0x040007E5 RID: 2021
			BackgroundAndForeground
		}
	}
}
