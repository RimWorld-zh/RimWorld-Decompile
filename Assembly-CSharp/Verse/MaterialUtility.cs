using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F65 RID: 3941
	public static class MaterialUtility
	{
		// Token: 0x06005F56 RID: 24406 RVA: 0x00309E88 File Offset: 0x00308288
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

		// Token: 0x06005F57 RID: 24407 RVA: 0x00309EC4 File Offset: 0x003082C4
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
