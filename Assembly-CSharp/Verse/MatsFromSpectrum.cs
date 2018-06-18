using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F64 RID: 3940
	public static class MatsFromSpectrum
	{
		// Token: 0x06005F2D RID: 24365 RVA: 0x003079C4 File Offset: 0x00305DC4
		public static Material Get(Color[] spectrum, float val)
		{
			return MatsFromSpectrum.Get(spectrum, val, ShaderDatabase.MetaOverlay);
		}

		// Token: 0x06005F2E RID: 24366 RVA: 0x003079E8 File Offset: 0x00305DE8
		public static Material Get(Color[] spectrum, float val, Shader shader)
		{
			Color col = ColorsFromSpectrum.Get(spectrum, val);
			return SolidColorMaterials.NewSolidColorMaterial(col, shader);
		}
	}
}
