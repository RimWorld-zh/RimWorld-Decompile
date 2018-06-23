using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000196 RID: 406
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Edge Detection/Edge Detection")]
	public class EdgeDetection : PostEffectsBase
	{
		// Token: 0x040007E6 RID: 2022
		public EdgeDetection.EdgeDetectMode mode = EdgeDetection.EdgeDetectMode.SobelDepthThin;

		// Token: 0x040007E7 RID: 2023
		public float sensitivityDepth = 1f;

		// Token: 0x040007E8 RID: 2024
		public float sensitivityNormals = 1f;

		// Token: 0x040007E9 RID: 2025
		public float lumThreshold = 0.2f;

		// Token: 0x040007EA RID: 2026
		public float edgeExp = 1f;

		// Token: 0x040007EB RID: 2027
		public float sampleDist = 1f;

		// Token: 0x040007EC RID: 2028
		public float edgesOnly = 0f;

		// Token: 0x040007ED RID: 2029
		public Color edgesOnlyBgColor = Color.white;

		// Token: 0x040007EE RID: 2030
		public Shader edgeDetectShader;

		// Token: 0x040007EF RID: 2031
		private Material edgeDetectMaterial = null;

		// Token: 0x040007F0 RID: 2032
		private EdgeDetection.EdgeDetectMode oldMode = EdgeDetection.EdgeDetectMode.SobelDepthThin;

		// Token: 0x06000917 RID: 2327 RVA: 0x0001626C File Offset: 0x0001446C
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.edgeDetectMaterial = base.CheckShaderAndCreateMaterial(this.edgeDetectShader, this.edgeDetectMaterial);
			if (this.mode != this.oldMode)
			{
				this.SetCameraFlag();
			}
			this.oldMode = this.mode;
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x000162DB File Offset: 0x000144DB
		private new void Start()
		{
			this.oldMode = this.mode;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x000162EC File Offset: 0x000144EC
		private void SetCameraFlag()
		{
			if (this.mode == EdgeDetection.EdgeDetectMode.SobelDepth || this.mode == EdgeDetection.EdgeDetectMode.SobelDepthThin)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			else if (this.mode == EdgeDetection.EdgeDetectMode.TriangleDepthNormals || this.mode == EdgeDetection.EdgeDetectMode.RobertsCrossDepthNormals)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
			}
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x00016354 File Offset: 0x00014554
		private void OnEnable()
		{
			this.SetCameraFlag();
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00016360 File Offset: 0x00014560
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				Vector2 vector = new Vector2(this.sensitivityDepth, this.sensitivityNormals);
				this.edgeDetectMaterial.SetVector("_Sensitivity", new Vector4(vector.x, vector.y, 1f, vector.y));
				this.edgeDetectMaterial.SetFloat("_BgFade", this.edgesOnly);
				this.edgeDetectMaterial.SetFloat("_SampleDistance", this.sampleDist);
				this.edgeDetectMaterial.SetVector("_BgColor", this.edgesOnlyBgColor);
				this.edgeDetectMaterial.SetFloat("_Exponent", this.edgeExp);
				this.edgeDetectMaterial.SetFloat("_Threshold", this.lumThreshold);
				Graphics.Blit(source, destination, this.edgeDetectMaterial, (int)this.mode);
			}
		}

		// Token: 0x02000197 RID: 407
		public enum EdgeDetectMode
		{
			// Token: 0x040007F2 RID: 2034
			TriangleDepthNormals,
			// Token: 0x040007F3 RID: 2035
			RobertsCrossDepthNormals,
			// Token: 0x040007F4 RID: 2036
			SobelDepth,
			// Token: 0x040007F5 RID: 2037
			SobelDepthThin,
			// Token: 0x040007F6 RID: 2038
			TriangleLuminance
		}
	}
}
