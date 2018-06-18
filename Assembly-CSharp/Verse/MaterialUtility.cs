using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F61 RID: 3937
	public static class MaterialUtility
	{
		// Token: 0x06005F23 RID: 24355 RVA: 0x00307764 File Offset: 0x00305B64
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

		// Token: 0x06005F24 RID: 24356 RVA: 0x003077A0 File Offset: 0x00305BA0
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
