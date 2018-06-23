using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000173 RID: 371
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Antialiasing")]
	public class Antialiasing : PostEffectsBase
	{
		// Token: 0x0400069B RID: 1691
		public AAMode mode = AAMode.FXAA3Console;

		// Token: 0x0400069C RID: 1692
		public bool showGeneratedNormals = false;

		// Token: 0x0400069D RID: 1693
		public float offsetScale = 0.2f;

		// Token: 0x0400069E RID: 1694
		public float blurRadius = 18f;

		// Token: 0x0400069F RID: 1695
		public float edgeThresholdMin = 0.05f;

		// Token: 0x040006A0 RID: 1696
		public float edgeThreshold = 0.2f;

		// Token: 0x040006A1 RID: 1697
		public float edgeSharpness = 4f;

		// Token: 0x040006A2 RID: 1698
		public bool dlaaSharp = false;

		// Token: 0x040006A3 RID: 1699
		public Shader ssaaShader;

		// Token: 0x040006A4 RID: 1700
		private Material ssaa;

		// Token: 0x040006A5 RID: 1701
		public Shader dlaaShader;

		// Token: 0x040006A6 RID: 1702
		private Material dlaa;

		// Token: 0x040006A7 RID: 1703
		public Shader nfaaShader;

		// Token: 0x040006A8 RID: 1704
		private Material nfaa;

		// Token: 0x040006A9 RID: 1705
		public Shader shaderFXAAPreset2;

		// Token: 0x040006AA RID: 1706
		private Material materialFXAAPreset2;

		// Token: 0x040006AB RID: 1707
		public Shader shaderFXAAPreset3;

		// Token: 0x040006AC RID: 1708
		private Material materialFXAAPreset3;

		// Token: 0x040006AD RID: 1709
		public Shader shaderFXAAII;

		// Token: 0x040006AE RID: 1710
		private Material materialFXAAII;

		// Token: 0x040006AF RID: 1711
		public Shader shaderFXAAIII;

		// Token: 0x040006B0 RID: 1712
		private Material materialFXAAIII;

		// Token: 0x060008AD RID: 2221 RVA: 0x0000FC50 File Offset: 0x0000DE50
		public Material CurrentAAMaterial()
		{
			Material result;
			switch (this.mode)
			{
			case AAMode.FXAA2:
				result = this.materialFXAAII;
				break;
			case AAMode.FXAA3Console:
				result = this.materialFXAAIII;
				break;
			case AAMode.FXAA1PresetA:
				result = this.materialFXAAPreset2;
				break;
			case AAMode.FXAA1PresetB:
				result = this.materialFXAAPreset3;
				break;
			case AAMode.NFAA:
				result = this.nfaa;
				break;
			case AAMode.SSAA:
				result = this.ssaa;
				break;
			case AAMode.DLAA:
				result = this.dlaa;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0000FCF4 File Offset: 0x0000DEF4
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.materialFXAAPreset2 = base.CreateMaterial(this.shaderFXAAPreset2, this.materialFXAAPreset2);
			this.materialFXAAPreset3 = base.CreateMaterial(this.shaderFXAAPreset3, this.materialFXAAPreset3);
			this.materialFXAAII = base.CreateMaterial(this.shaderFXAAII, this.materialFXAAII);
			this.materialFXAAIII = base.CreateMaterial(this.shaderFXAAIII, this.materialFXAAIII);
			this.nfaa = base.CreateMaterial(this.nfaaShader, this.nfaa);
			this.ssaa = base.CreateMaterial(this.ssaaShader, this.ssaa);
			this.dlaa = base.CreateMaterial(this.dlaaShader, this.dlaa);
			if (!this.ssaaShader.isSupported)
			{
				base.NotSupported();
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0000FDE0 File Offset: 0x0000DFE0
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else if (this.mode == AAMode.FXAA3Console && this.materialFXAAIII != null)
			{
				this.materialFXAAIII.SetFloat("_EdgeThresholdMin", this.edgeThresholdMin);
				this.materialFXAAIII.SetFloat("_EdgeThreshold", this.edgeThreshold);
				this.materialFXAAIII.SetFloat("_EdgeSharpness", this.edgeSharpness);
				Graphics.Blit(source, destination, this.materialFXAAIII);
			}
			else if (this.mode == AAMode.FXAA1PresetB && this.materialFXAAPreset3 != null)
			{
				Graphics.Blit(source, destination, this.materialFXAAPreset3);
			}
			else if (this.mode == AAMode.FXAA1PresetA && this.materialFXAAPreset2 != null)
			{
				source.anisoLevel = 4;
				Graphics.Blit(source, destination, this.materialFXAAPreset2);
				source.anisoLevel = 0;
			}
			else if (this.mode == AAMode.FXAA2 && this.materialFXAAII != null)
			{
				Graphics.Blit(source, destination, this.materialFXAAII);
			}
			else if (this.mode == AAMode.SSAA && this.ssaa != null)
			{
				Graphics.Blit(source, destination, this.ssaa);
			}
			else if (this.mode == AAMode.DLAA && this.dlaa != null)
			{
				source.anisoLevel = 0;
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
				Graphics.Blit(source, temporary, this.dlaa, 0);
				Graphics.Blit(temporary, destination, this.dlaa, (!this.dlaaSharp) ? 1 : 2);
				RenderTexture.ReleaseTemporary(temporary);
			}
			else if (this.mode == AAMode.NFAA && this.nfaa != null)
			{
				source.anisoLevel = 0;
				this.nfaa.SetFloat("_OffsetScale", this.offsetScale);
				this.nfaa.SetFloat("_BlurRadius", this.blurRadius);
				Graphics.Blit(source, destination, this.nfaa, (!this.showGeneratedNormals) ? 0 : 1);
			}
			else
			{
				Graphics.Blit(source, destination);
			}
		}
	}
}
