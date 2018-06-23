using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200019D RID: 413
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Blur/Motion Blur (Color Accumulation)")]
	[RequireComponent(typeof(Camera))]
	public class MotionBlur : ImageEffectBase
	{
		// Token: 0x04000808 RID: 2056
		[Range(0f, 0.92f)]
		public float blurAmount = 0.8f;

		// Token: 0x04000809 RID: 2057
		public bool extraBlur = false;

		// Token: 0x0400080A RID: 2058
		private RenderTexture accumTexture;

		// Token: 0x0600092E RID: 2350 RVA: 0x00016BE5 File Offset: 0x00014DE5
		protected override void OnDisable()
		{
			base.OnDisable();
			UnityEngine.Object.DestroyImmediate(this.accumTexture);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00016BFC File Offset: 0x00014DFC
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.accumTexture == null || this.accumTexture.width != source.width || this.accumTexture.height != source.height)
			{
				UnityEngine.Object.DestroyImmediate(this.accumTexture);
				this.accumTexture = new RenderTexture(source.width, source.height, 0);
				this.accumTexture.hideFlags = HideFlags.HideAndDontSave;
				Graphics.Blit(source, this.accumTexture);
			}
			if (this.extraBlur)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
				this.accumTexture.MarkRestoreExpected();
				Graphics.Blit(this.accumTexture, temporary);
				Graphics.Blit(temporary, this.accumTexture);
				RenderTexture.ReleaseTemporary(temporary);
			}
			this.blurAmount = Mathf.Clamp(this.blurAmount, 0f, 0.92f);
			base.material.SetTexture("_MainTex", this.accumTexture);
			base.material.SetFloat("_AccumOrig", 1f - this.blurAmount);
			this.accumTexture.MarkRestoreExpected();
			Graphics.Blit(source, this.accumTexture, base.material);
			Graphics.Blit(this.accumTexture, destination);
		}
	}
}
