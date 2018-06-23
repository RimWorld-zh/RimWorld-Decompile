using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000199 RID: 409
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Global Fog")]
	internal class GlobalFog : PostEffectsBase
	{
		// Token: 0x040007FB RID: 2043
		[Tooltip("Apply distance-based fog?")]
		public bool distanceFog = true;

		// Token: 0x040007FC RID: 2044
		[Tooltip("Exclude far plane pixels from distance-based fog? (Skybox or clear color)")]
		public bool excludeFarPixels = true;

		// Token: 0x040007FD RID: 2045
		[Tooltip("Distance fog is based on radial distance from camera when checked")]
		public bool useRadialDistance = false;

		// Token: 0x040007FE RID: 2046
		[Tooltip("Apply height-based fog?")]
		public bool heightFog = true;

		// Token: 0x040007FF RID: 2047
		[Tooltip("Fog top Y coordinate")]
		public float height = 1f;

		// Token: 0x04000800 RID: 2048
		[Range(0.001f, 10f)]
		public float heightDensity = 2f;

		// Token: 0x04000801 RID: 2049
		[Tooltip("Push fog away from the camera by this amount")]
		public float startDistance = 0f;

		// Token: 0x04000802 RID: 2050
		public Shader fogShader = null;

		// Token: 0x04000803 RID: 2051
		private Material fogMaterial = null;

		// Token: 0x06000920 RID: 2336 RVA: 0x000165B8 File Offset: 0x000147B8
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.fogMaterial = base.CheckShaderAndCreateMaterial(this.fogShader, this.fogMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00016604 File Offset: 0x00014804
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources() || (!this.distanceFog && !this.heightFog))
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				Camera component = base.GetComponent<Camera>();
				Transform transform = component.transform;
				float nearClipPlane = component.nearClipPlane;
				float farClipPlane = component.farClipPlane;
				float fieldOfView = component.fieldOfView;
				float aspect = component.aspect;
				Matrix4x4 identity = Matrix4x4.identity;
				float num = fieldOfView * 0.5f;
				Vector3 b = transform.right * nearClipPlane * Mathf.Tan(num * 0.0174532924f) * aspect;
				Vector3 b2 = transform.up * nearClipPlane * Mathf.Tan(num * 0.0174532924f);
				Vector3 vector = transform.forward * nearClipPlane - b + b2;
				float d = vector.magnitude * farClipPlane / nearClipPlane;
				vector.Normalize();
				vector *= d;
				Vector3 vector2 = transform.forward * nearClipPlane + b + b2;
				vector2.Normalize();
				vector2 *= d;
				Vector3 vector3 = transform.forward * nearClipPlane + b - b2;
				vector3.Normalize();
				vector3 *= d;
				Vector3 vector4 = transform.forward * nearClipPlane - b - b2;
				vector4.Normalize();
				vector4 *= d;
				identity.SetRow(0, vector);
				identity.SetRow(1, vector2);
				identity.SetRow(2, vector3);
				identity.SetRow(3, vector4);
				Vector3 position = transform.position;
				float num2 = position.y - this.height;
				float z = (num2 > 0f) ? 0f : 1f;
				float y = (!this.excludeFarPixels) ? 2f : 1f;
				this.fogMaterial.SetMatrix("_FrustumCornersWS", identity);
				this.fogMaterial.SetVector("_CameraWS", position);
				this.fogMaterial.SetVector("_HeightParams", new Vector4(this.height, num2, z, this.heightDensity * 0.5f));
				this.fogMaterial.SetVector("_DistanceParams", new Vector4(-Mathf.Max(this.startDistance, 0f), y, 0f, 0f));
				FogMode fogMode = RenderSettings.fogMode;
				float fogDensity = RenderSettings.fogDensity;
				float fogStartDistance = RenderSettings.fogStartDistance;
				float fogEndDistance = RenderSettings.fogEndDistance;
				bool flag = fogMode == FogMode.Linear;
				float num3 = (!flag) ? 0f : (fogEndDistance - fogStartDistance);
				float num4 = (Mathf.Abs(num3) <= 0.0001f) ? 0f : (1f / num3);
				Vector4 value;
				value.x = fogDensity * 1.2011224f;
				value.y = fogDensity * 1.442695f;
				value.z = ((!flag) ? 0f : (-num4));
				value.w = ((!flag) ? 0f : (fogEndDistance * num4));
				this.fogMaterial.SetVector("_SceneFogParams", value);
				this.fogMaterial.SetVector("_SceneFogMode", new Vector4((float)fogMode, (float)((!this.useRadialDistance) ? 0 : 1), 0f, 0f));
				int passNr;
				if (this.distanceFog && this.heightFog)
				{
					passNr = 0;
				}
				else if (this.distanceFog)
				{
					passNr = 1;
				}
				else
				{
					passNr = 2;
				}
				GlobalFog.CustomGraphicsBlit(source, destination, this.fogMaterial, passNr);
			}
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x000169E0 File Offset: 0x00014BE0
		private static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
		{
			RenderTexture.active = dest;
			fxMaterial.SetTexture("_MainTex", source);
			GL.PushMatrix();
			GL.LoadOrtho();
			fxMaterial.SetPass(passNr);
			GL.Begin(7);
			GL.MultiTexCoord2(0, 0f, 0f);
			GL.Vertex3(0f, 0f, 3f);
			GL.MultiTexCoord2(0, 1f, 0f);
			GL.Vertex3(1f, 0f, 2f);
			GL.MultiTexCoord2(0, 1f, 1f);
			GL.Vertex3(1f, 1f, 1f);
			GL.MultiTexCoord2(0, 0f, 1f);
			GL.Vertex3(0f, 1f, 0f);
			GL.End();
			GL.PopMatrix();
		}
	}
}
