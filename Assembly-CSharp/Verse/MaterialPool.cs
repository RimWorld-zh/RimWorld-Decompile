using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D73 RID: 3443
	public static class MaterialPool
	{
		// Token: 0x0400337D RID: 13181
		private static Dictionary<MaterialRequest, Material> matDictionary = new Dictionary<MaterialRequest, Material>();

		// Token: 0x06004D28 RID: 19752 RVA: 0x002837E8 File Offset: 0x00281BE8
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

		// Token: 0x06004D29 RID: 19753 RVA: 0x00283830 File Offset: 0x00281C30
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

		// Token: 0x06004D2A RID: 19754 RVA: 0x00283878 File Offset: 0x00281C78
		public static Material MatFrom(Texture2D srcTex)
		{
			MaterialRequest req = new MaterialRequest(srcTex);
			return MaterialPool.MatFrom(req);
		}

		// Token: 0x06004D2B RID: 19755 RVA: 0x0028389C File Offset: 0x00281C9C
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color)
		{
			MaterialRequest req = new MaterialRequest(srcTex, shader, color);
			return MaterialPool.MatFrom(req);
		}

		// Token: 0x06004D2C RID: 19756 RVA: 0x002838C4 File Offset: 0x00281CC4
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06004D2D RID: 19757 RVA: 0x002838F4 File Offset: 0x00281CF4
		public static Material MatFrom(string texPath, Shader shader)
		{
			MaterialRequest req = new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader);
			return MaterialPool.MatFrom(req);
		}

		// Token: 0x06004D2E RID: 19758 RVA: 0x00283920 File Offset: 0x00281D20
		public static Material MatFrom(string texPath, Shader shader, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x00283954 File Offset: 0x00281D54
		public static Material MatFrom(string texPath, Shader shader, Color color)
		{
			MaterialRequest req = new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color);
			return MaterialPool.MatFrom(req);
		}

		// Token: 0x06004D30 RID: 19760 RVA: 0x00283980 File Offset: 0x00281D80
		public static Material MatFrom(string texPath, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06004D31 RID: 19761 RVA: 0x002839B4 File Offset: 0x00281DB4
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
