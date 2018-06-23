using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000198 RID: 408
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Displacement/Fisheye")]
	public class Fisheye : PostEffectsBase
	{
		// Token: 0x040007F7 RID: 2039
		[Range(0f, 1.5f)]
		public float strengthX = 0.05f;

		// Token: 0x040007F8 RID: 2040
		[Range(0f, 1.5f)]
		public float strengthY = 0.05f;

		// Token: 0x040007F9 RID: 2041
		public Shader fishEyeShader = null;

		// Token: 0x040007FA RID: 2042
		private Material fisheyeMaterial = null;

		// Token: 0x0600091D RID: 2333 RVA: 0x0001647C File Offset: 0x0001467C
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.fisheyeMaterial = base.CheckShaderAndCreateMaterial(this.fishEyeShader, this.fisheyeMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x000164C8 File Offset: 0x000146C8
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				float num = 0.15625f;
				float num2 = (float)source.width * 1f / ((float)source.height * 1f);
				this.fisheyeMaterial.SetVector("intensity", new Vector4(this.strengthX * num2 * num, this.strengthY * num, this.strengthX * num2 * num, this.strengthY * num));
				Graphics.Blit(source, destination, this.fisheyeMaterial);
			}
		}
	}
}
