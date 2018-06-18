using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020001A0 RID: 416
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class PostEffectsBase : MonoBehaviour
	{
		// Token: 0x0600093C RID: 2364 RVA: 0x0000F5C0 File Offset: 0x0000D7C0
		protected Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
		{
			Material result;
			if (!s)
			{
				Debug.Log("Missing shader in " + this.ToString());
				base.enabled = false;
				result = null;
			}
			else if (s.isSupported && m2Create && m2Create.shader == s)
			{
				result = m2Create;
			}
			else if (!s.isSupported)
			{
				this.NotSupported();
				Debug.Log(string.Concat(new string[]
				{
					"The shader ",
					s.ToString(),
					" on effect ",
					this.ToString(),
					" is not supported on this platform!"
				}));
				result = null;
			}
			else
			{
				m2Create = new Material(s);
				m2Create.hideFlags = HideFlags.DontSave;
				if (m2Create)
				{
					result = m2Create;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0000F6AC File Offset: 0x0000D8AC
		protected Material CreateMaterial(Shader s, Material m2Create)
		{
			Material result;
			if (!s)
			{
				Debug.Log("Missing shader in " + this.ToString());
				result = null;
			}
			else if (m2Create && m2Create.shader == s && s.isSupported)
			{
				result = m2Create;
			}
			else if (!s.isSupported)
			{
				result = null;
			}
			else
			{
				m2Create = new Material(s);
				m2Create.hideFlags = HideFlags.DontSave;
				if (m2Create)
				{
					result = m2Create;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0000F74E File Offset: 0x0000D94E
		private void OnEnable()
		{
			this.isSupported = true;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0000F758 File Offset: 0x0000D958
		protected bool CheckSupport()
		{
			return this.CheckSupport(false);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0000F774 File Offset: 0x0000D974
		public virtual bool CheckResources()
		{
			Debug.LogWarning("CheckResources () for " + this.ToString() + " should be overwritten.");
			return this.isSupported;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0000F7A9 File Offset: 0x0000D9A9
		protected void Start()
		{
			this.CheckResources();
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0000F7B4 File Offset: 0x0000D9B4
		protected bool CheckSupport(bool needDepth)
		{
			this.isSupported = true;
			this.supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
			this.supportDX11 = (SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders);
			bool result;
			if (!SystemInfo.supportsImageEffects)
			{
				this.NotSupported();
				result = false;
			}
			else if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				this.NotSupported();
				result = false;
			}
			else
			{
				if (needDepth)
				{
					base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0000F848 File Offset: 0x0000DA48
		protected bool CheckSupport(bool needDepth, bool needHdr)
		{
			bool result;
			if (!this.CheckSupport(needDepth))
			{
				result = false;
			}
			else if (needHdr && !this.supportHDRTextures)
			{
				this.NotSupported();
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0000F890 File Offset: 0x0000DA90
		public bool Dx11Support()
		{
			return this.supportDX11;
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0000F8AB File Offset: 0x0000DAAB
		protected void ReportAutoDisable()
		{
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0000F8C8 File Offset: 0x0000DAC8
		private bool CheckShader(Shader s)
		{
			Debug.Log(string.Concat(new string[]
			{
				"The shader ",
				s.ToString(),
				" on effect ",
				this.ToString(),
				" is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package."
			}));
			bool result;
			if (!s.isSupported)
			{
				this.NotSupported();
				result = false;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0000F932 File Offset: 0x0000DB32
		protected void NotSupported()
		{
			base.enabled = false;
			this.isSupported = false;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0000F948 File Offset: 0x0000DB48
		protected void DrawBorder(RenderTexture dest, Material material)
		{
			RenderTexture.active = dest;
			bool flag = true;
			GL.PushMatrix();
			GL.LoadOrtho();
			for (int i = 0; i < material.passCount; i++)
			{
				material.SetPass(i);
				float y;
				float y2;
				if (flag)
				{
					y = 1f;
					y2 = 0f;
				}
				else
				{
					y = 0f;
					y2 = 1f;
				}
				float x = 0f;
				float x2 = 1f / ((float)dest.width * 1f);
				float y3 = 0f;
				float y4 = 1f;
				GL.Begin(7);
				GL.TexCoord2(0f, y);
				GL.Vertex3(x, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x, y4, 0.1f);
				x = 1f - 1f / ((float)dest.width * 1f);
				x2 = 1f;
				y3 = 0f;
				y4 = 1f;
				GL.TexCoord2(0f, y);
				GL.Vertex3(x, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x, y4, 0.1f);
				x = 0f;
				x2 = 1f;
				y3 = 0f;
				y4 = 1f / ((float)dest.height * 1f);
				GL.TexCoord2(0f, y);
				GL.Vertex3(x, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x, y4, 0.1f);
				x = 0f;
				x2 = 1f;
				y3 = 1f - 1f / ((float)dest.height * 1f);
				y4 = 1f;
				GL.TexCoord2(0f, y);
				GL.Vertex3(x, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x, y4, 0.1f);
				GL.End();
			}
			GL.PopMatrix();
		}

		// Token: 0x0400082F RID: 2095
		protected bool supportHDRTextures = true;

		// Token: 0x04000830 RID: 2096
		protected bool supportDX11 = false;

		// Token: 0x04000831 RID: 2097
		protected bool isSupported = true;
	}
}
