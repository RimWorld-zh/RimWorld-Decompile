using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F68 RID: 3944
	public static class MatsFromSpectrum
	{
		// Token: 0x06005F60 RID: 24416 RVA: 0x0030A0E8 File Offset: 0x003084E8
		public static Material Get(Color[] spectrum, float val)
		{
			return MatsFromSpectrum.Get(spectrum, val, ShaderDatabase.MetaOverlay);
		}

		// Token: 0x06005F61 RID: 24417 RVA: 0x0030A10C File Offset: 0x0030850C
		public static Material Get(Color[] spectrum, float val, Shader shader)
		{
			Color col = ColorsFromSpectrum.Get(spectrum, val);
			return SolidColorMaterials.NewSolidColorMaterial(col, shader);
		}
	}
}
