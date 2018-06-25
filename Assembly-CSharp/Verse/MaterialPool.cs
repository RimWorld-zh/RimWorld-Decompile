using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class MaterialPool
	{
		private static Dictionary<MaterialRequest, Material> matDictionary = new Dictionary<MaterialRequest, Material>();

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

		public static Material MatFrom(Texture2D srcTex)
		{
			MaterialRequest req = new MaterialRequest(srcTex);
			return MaterialPool.MatFrom(req);
		}

		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color)
		{
			MaterialRequest req = new MaterialRequest(srcTex, shader, color);
			return MaterialPool.MatFrom(req);
		}

		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color)
			{
				renderQueue = renderQueue
			});
		}

		public static Material MatFrom(string texPath, Shader shader)
		{
			MaterialRequest req = new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader);
			return MaterialPool.MatFrom(req);
		}

		public static Material MatFrom(string texPath, Shader shader, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader)
			{
				renderQueue = renderQueue
			});
		}

		public static Material MatFrom(string texPath, Shader shader, Color color)
		{
			MaterialRequest req = new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color);
			return MaterialPool.MatFrom(req);
		}

		public static Material MatFrom(string texPath, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color)
			{
				renderQueue = renderQueue
			});
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static MaterialPool()
		{
		}
	}
}
