using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F67 RID: 3943
	public static class ShaderUtility
	{
		// Token: 0x06005F5C RID: 24412 RVA: 0x00309D90 File Offset: 0x00308190
		public static bool SupportsMaskTex(this Shader shader)
		{
			return shader == ShaderDatabase.CutoutComplex;
		}
	}
}
