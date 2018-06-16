using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F62 RID: 3938
	public static class MaterialUtility
	{
		// Token: 0x06005F25 RID: 24357 RVA: 0x00307688 File Offset: 0x00305A88
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

		// Token: 0x06005F26 RID: 24358 RVA: 0x003076C4 File Offset: 0x00305AC4
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
