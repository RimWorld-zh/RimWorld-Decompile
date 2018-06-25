using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F6B RID: 3947
	public static class ShaderUtility
	{
		// Token: 0x06005F66 RID: 24422 RVA: 0x0030A410 File Offset: 0x00308810
		public static bool SupportsMaskTex(this Shader shader)
		{
			return shader == ShaderDatabase.CutoutComplex;
		}
	}
}
