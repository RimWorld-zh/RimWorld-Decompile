using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D74 RID: 3444
	public static class MaterialPool
	{
		// Token: 0x06004D11 RID: 19729 RVA: 0x00281E4C File Offset: 0x0028024C
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

		// Token: 0x06004D12 RID: 19730 RVA: 0x00281E94 File Offset: 0x00280294
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

		// Token: 0x06004D13 RID: 19731 RVA: 0x00281EDC File Offset: 0x002802DC
		public static Material MatFrom(Texture2D srcTex)
		{
			MaterialRequest req = new MaterialRequest(srcTex);
			return MaterialPool.MatFrom(req);
		}

		// Token: 0x06004D14 RID: 19732 RVA: 0x00281F00 File Offset: 0x00280300
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color)
		{
			MaterialRequest req = new MaterialRequest(srcTex, shader, color);
			return MaterialPool.MatFrom(req);
		}

		// Token: 0x06004D15 RID: 19733 RVA: 0x00281F28 File Offset: 0x00280328
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06004D16 RID: 19734 RVA: 0x00281F58 File Offset: 0x00280358
		public static Material MatFrom(string texPath, Shader shader)
		{
			MaterialRequest req = new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader);
			return MaterialPool.MatFrom(req);
		}

		// Token: 0x06004D17 RID: 19735 RVA: 0x00281F84 File Offset: 0x00280384
		public static Material MatFrom(string texPath, Shader shader, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06004D18 RID: 19736 RVA: 0x00281FB8 File Offset: 0x002803B8
		public static Material MatFrom(string texPath, Shader shader, Color color)
		{
			MaterialRequest req = new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color);
			return MaterialPool.MatFrom(req);
		}

		// Token: 0x06004D19 RID: 19737 RVA: 0x00281FE4 File Offset: 0x002803E4
		public static Material MatFrom(string texPath, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06004D1A RID: 19738 RVA: 0x00282018 File Offset: 0x00280418
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

		// Token: 0x0400336D RID: 13165
		private static Dictionary<MaterialRequest, Material> matDictionary = new Dictionary<MaterialRequest, Material>();
	}
}
