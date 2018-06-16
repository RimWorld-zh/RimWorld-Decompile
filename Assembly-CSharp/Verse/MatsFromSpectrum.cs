using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F65 RID: 3941
	public static class MatsFromSpectrum
	{
		// Token: 0x06005F2F RID: 24367 RVA: 0x003078E8 File Offset: 0x00305CE8
		public static Material Get(Color[] spectrum, float val)
		{
			return MatsFromSpectrum.Get(spectrum, val, ShaderDatabase.MetaOverlay);
		}

		// Token: 0x06005F30 RID: 24368 RVA: 0x0030790C File Offset: 0x00305D0C
		public static Material Get(Color[] spectrum, float val, Shader shader)
		{
			Color col = ColorsFromSpectrum.Get(spectrum, val);
			return SolidColorMaterials.NewSolidColorMaterial(col, shader);
		}
	}
}
