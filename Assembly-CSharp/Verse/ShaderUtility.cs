using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F67 RID: 3943
	public static class ShaderUtility
	{
		// Token: 0x06005F33 RID: 24371 RVA: 0x00307CEC File Offset: 0x003060EC
		public static bool SupportsMaskTex(this Shader shader)
		{
			return shader == ShaderDatabase.CutoutComplex;
		}
	}
}
