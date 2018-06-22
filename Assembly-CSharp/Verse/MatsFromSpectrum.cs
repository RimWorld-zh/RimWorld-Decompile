using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F64 RID: 3940
	public static class MatsFromSpectrum
	{
		// Token: 0x06005F56 RID: 24406 RVA: 0x00309A68 File Offset: 0x00307E68
		public static Material Get(Color[] spectrum, float val)
		{
			return MatsFromSpectrum.Get(spectrum, val, ShaderDatabase.MetaOverlay);
		}

		// Token: 0x06005F57 RID: 24407 RVA: 0x00309A8C File Offset: 0x00307E8C
		public static Material Get(Color[] spectrum, float val, Shader shader)
		{
			Color col = ColorsFromSpectrum.Get(spectrum, val);
			return SolidColorMaterials.NewSolidColorMaterial(col, shader);
		}
	}
}
