using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F68 RID: 3944
	public static class ShaderUtility
	{
		// Token: 0x06005F35 RID: 24373 RVA: 0x00307C10 File Offset: 0x00306010
		public static bool SupportsMaskTex(this Shader shader)
		{
			return shader == ShaderDatabase.CutoutComplex;
		}
	}
}
