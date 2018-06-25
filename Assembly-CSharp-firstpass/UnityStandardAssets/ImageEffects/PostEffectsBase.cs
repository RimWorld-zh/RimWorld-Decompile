using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class PostEffectsBase : MonoBehaviour
	{
		protected bool supportHDRTextures = true;

		protected bool supportDX11 = false;

		protected bool isSupported = true;

		public PostEffectsBase()
		{
		}

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

		private void OnEnable()
		{
			this.isSupported = true;
		}

		protected bool CheckSupport()
		{
			return this.CheckSupport(false);
		}

		public virtual bool CheckResources()
		{
			Debug.LogWarning("CheckResources () for " + this.ToString() + " should be overwritten.");
			return this.isSupported;
		}

		protected void Start()
		{
			this.CheckResources();
		}

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

		public bool Dx11Support()
		{
			return this.supportDX11;
		}

		protected void ReportAutoDisable()
		{
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
		}

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

		protected void NotSupported()
		{
			base.enabled = false;
			this.isSupported = false;
		}

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
	}
}
