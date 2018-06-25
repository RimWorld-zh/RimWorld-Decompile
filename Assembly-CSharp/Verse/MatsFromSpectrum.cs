using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F69 RID: 3945
	public static class MatsFromSpectrum
	{
		// Token: 0x06005F60 RID: 24416 RVA: 0x0030A32C File Offset: 0x0030872C
		public static Material Get(Color[] spectrum, float val)
		{
			return MatsFromSpectrum.Get(spectrum, val, ShaderDatabase.MetaOverlay);
		}

		// Token: 0x06005F61 RID: 24417 RVA: 0x0030A350 File Offset: 0x00308750
		public static Material Get(Color[] spectrum, float val, Shader shader)
		{
			Color col = ColorsFromSpectrum.Get(spectrum, val);
			return SolidColorMaterials.NewSolidColorMaterial(col, shader);
		}
	}
}
