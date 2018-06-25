using System;
using UnityEngine;

namespace Verse
{
	public static class MaterialUtility
	{
		public static Texture2D GetMaskTexture(this Material mat)
		{
			Texture2D result;
			if (!mat.HasProperty(ShaderPropertyIDs.MaskTex))
			{
				result = null;
			}
			else
			{
				result = (Texture2D)mat.GetTexture(ShaderPropertyIDs.MaskTex);
			}
			return result;
		}

		public static Color GetColorTwo(this Material mat)
		{
			Color result;
			if (!mat.HasProperty(ShaderPropertyIDs.ColorTwo))
			{
				result = Color.white;
			}
			else
			{
				result = mat.GetColor(ShaderPropertyIDs.ColorTwo);
			}
			return result;
		}
	}
}
