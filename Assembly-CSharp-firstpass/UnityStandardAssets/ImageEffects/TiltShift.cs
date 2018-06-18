using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020001AC RID: 428
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Tilt Shift (Lens Blur)")]
	internal class TiltShift : PostEffectsBase
	{
		// Token: 0x06000969 RID: 2409 RVA: 0x00018FA8 File Offset: 0x000171A8
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.tiltShiftMaterial = base.CheckShaderAndCreateMaterial(this.tiltShiftShader, this.tiltShiftMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00018FF4 File Offset: 0x000171F4
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				this.tiltShiftMaterial.SetFloat("_BlurSize", (this.maxBlurSize >= 0f) ? this.maxBlurSize : 0f);
				this.tiltShiftMaterial.SetFloat("_BlurArea", this.blurArea);
				source.filterMode = FilterMode.Bilinear;
				RenderTexture renderTexture = destination;
				if ((float)this.downsample > 0f)
				{
					renderTexture = RenderTexture.GetTemporary(source.width >> this.downsample, source.height >> this.downsample, 0, source.format);
					renderTexture.filterMode = FilterMode.Bilinear;
				}
				int num = (int)this.quality;
				num *= 2;
				Graphics.Blit(source, renderTexture, this.tiltShiftMaterial, (this.mode != TiltShift.TiltShiftMode.TiltShiftMode) ? (num + 1) : num);
				if (this.downsample > 0)
				{
					this.tiltShiftMaterial.SetTexture("_Blurred", renderTexture);
					Graphics.Blit(source, destination, this.tiltShiftMaterial, 6);
				}
				if (renderTexture != destination)
				{
					RenderTexture.ReleaseTemporary(renderTexture);
				}
			}
		}

		// Token: 0x0400086B RID: 2155
		public TiltShift.TiltShiftMode mode = TiltShift.TiltShiftMode.TiltShiftMode;

		// Token: 0x0400086C RID: 2156
		public TiltShift.TiltShiftQuality quality = TiltShift.TiltShiftQuality.Normal;

		// Token: 0x0400086D RID: 2157
		[Range(0f, 15f)]
		public float blurArea = 1f;

		// Token: 0x0400086E RID: 2158
		[Range(0f, 25f)]
		public float maxBlurSize = 5f;

		// Token: 0x0400086F RID: 2159
		[Range(0f, 1f)]
		public int downsample = 0;

		// Token: 0x04000870 RID: 2160
		public Shader tiltShiftShader = null;

		// Token: 0x04000871 RID: 2161
		private Material tiltShiftMaterial = null;

		// Token: 0x020001AD RID: 429
		public enum TiltShiftMode
		{
			// Token: 0x04000873 RID: 2163
			TiltShiftMode,
			// Token: 0x04000874 RID: 2164
			IrisMode
		}

		// Token: 0x020001AE RID: 430
		public enum TiltShiftQuality
		{
			// Token: 0x04000876 RID: 2166
			Preview,
			// Token: 0x04000877 RID: 2167
			Normal,
			// Token: 0x04000878 RID: 2168
			High
		}
	}
}
