using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F6C RID: 3948
	public static class ShaderUtility
	{
		// Token: 0x06005F66 RID: 24422 RVA: 0x0030A654 File Offset: 0x00308A54
		public static bool SupportsMaskTex(this Shader shader)
		{
			return shader == ShaderDatabase.CutoutComplex;
		}
	}
}
