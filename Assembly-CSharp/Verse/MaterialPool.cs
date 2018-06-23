using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D70 RID: 3440
	public static class MaterialPool
	{
		// Token: 0x04003376 RID: 13174
		private static Dictionary<MaterialRequest, Material> matDictionary = new Dictionary<MaterialRequest, Material>();

		// Token: 0x06004D24 RID: 19748 RVA: 0x002833DC File Offset: 0x002817DC
		public static Material MatFrom(string texPath, bool reportFailure)
		{
			Material result;
			if (texPath == null || texPath == "null")
			{
				result = null;
			}
			else
			{
				MaterialRequest req = new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, reportFailure));
				result = MaterialPool.MatFrom(req);
			}
			return result;
		}

		// Token: 0x06004D25 RID: 19749 RVA: 0x00283424 File Offset: 0x00281824
		public static Material MatFrom(string texPath)
		{
			Material result;
			if (texPath == null || texPath == "null")
			{
				result = null;
			}
			else
			{
				MaterialRequest req = new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true));
				result = MaterialPool.MatFrom(req);
			}
			return result;
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x0028346C File Offset: 0x0028186C
		public static Material MatFrom(Texture2D srcTex)
		{
			MaterialRequest req = new MaterialRequest(srcTex);
			return MaterialPool.MatFrom(req);
		}

		// Token: 0x06004D27 RID: 19751 RVA: 0x00283490 File Offset: 0x00281890
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color)
		{
			MaterialRequest req = new MaterialRequest(srcTex, shader, color);
			return MaterialPool.MatFrom(req);
		}

		// Token: 0x06004D28 RID: 19752 RVA: 0x002834B8 File Offset: 0x002818B8
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06004D29 RID: 19753 RVA: 0x002834E8 File Offset: 0x002818E8
		public static Material MatFrom(string texPath, Shader shader)
		{
			MaterialRequest req = new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader);
			return MaterialPool.MatFrom(req);
		}

		// Token: 0x06004D2A RID: 19754 RVA: 0x00283514 File Offset: 0x00281914
		public static Material MatFrom(string texPath, Shader shader, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06004D2B RID: 19755 RVA: 0x00283548 File Offset: 0x00281948
		public static Material MatFrom(string texPath, Shader shader, Color color)
		{
			MaterialRequest req = new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color);
			return MaterialPool.MatFrom(req);
		}

		// Token: 0x06004D2C RID: 19756 RVA: 0x00283574 File Offset: 0x00281974
		public static Material MatFrom(string texPath, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06004D2D RID: 19757 RVA: 0x002835A8 File Offset: 0x002819A8
		public static Material MatFrom(MaterialRequest req)
		{
			Material result;
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get a material from a different thread.", false);
				result = null;
			}
			else if (req.mainTex == null)
			{
				Log.Error("MatFrom with null sourceTex.", false);
				result = BaseContent.BadMat;
			}
			else if (req.shader == null)
			{
				Log.Warning("Matfrom with null shader.", false);
				result = BaseContent.BadMat;
			}
			else
			{
				if (req.maskTex != null && !req.shader.SupportsMaskTex())
				{
					Log.Error("MaterialRequest has maskTex but shader does not support it. req=" + req.ToString(), false);
					req.maskTex = null;
				}
				Material material;
				if (!MaterialPool.matDictionary.TryGetValue(req, out material))
				{
					material = MaterialAllocator.Create(req.shader);
					material.name = req.shader.name + "_" + req.mainTex.name;
					material.mainTexture = req.mainTex;
					material.color = req.color;
					if (req.maskTex != null)
					{
						material.SetTexture(ShaderPropertyIDs.MaskTex, req.maskTex);
						material.SetColor(ShaderPropertyIDs.ColorTwo, req.colorTwo);
					}
					if (req.renderQueue != 0)
					{
						material.renderQueue = req.renderQueue;
					}
					if (!req.shaderParameters.NullOrEmpty<ShaderParameter>())
					{
						for (int i = 0; i < req.shaderParameters.Count; i++)
						{
							req.shaderParameters[i].Apply(material);
						}
					}
					MaterialPool.matDictionary.Add(req, material);
					if (!MaterialPool.matDictionary.ContainsKey(req))
					{
						Log.Error("MaterialRequest is not present in the dictionary even though we've just added it there. The equality operators are most likely defined incorrectly.", false);
					}
					if (req.shader == ShaderDatabase.CutoutPlant || req.shader == ShaderDatabase.TransparentPlant)
					{
						WindManager.Notify_PlantMaterialCreated(material);
					}
				}
				result = material;
			}
			return result;
		}
	}
}
