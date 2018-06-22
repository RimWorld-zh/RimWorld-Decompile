using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F61 RID: 3937
	public static class MaterialUtility
	{
		// Token: 0x06005F4C RID: 24396 RVA: 0x00309808 File Offset: 0x00307C08
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

		// Token: 0x06005F4D RID: 24397 RVA: 0x00309844 File Offset: 0x00307C44
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
