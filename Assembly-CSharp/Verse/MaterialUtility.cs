using UnityEngine;

namespace Verse
{
	public static class MaterialUtility
	{
		public static Texture2D GetMaskTexture(this Material mat)
		{
			return mat.HasProperty(ShaderPropertyIDs.MaskTex) ? ((Texture2D)mat.GetTexture(ShaderPropertyIDs.MaskTex)) : null;
		}

		public static Color GetColorTwo(this Material mat)
		{
			return mat.HasProperty(ShaderPropertyIDs.ColorTwo) ? mat.GetColor(ShaderPropertyIDs.ColorTwo) : Color.white;
		}
	}
}
