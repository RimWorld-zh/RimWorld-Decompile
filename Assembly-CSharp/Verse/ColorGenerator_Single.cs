using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0B RID: 2827
	public class ColorGenerator_Single : ColorGenerator
	{
		// Token: 0x040027E6 RID: 10214
		public Color color;

		// Token: 0x06003E8E RID: 16014 RVA: 0x0020F8A0 File Offset: 0x0020DCA0
		public override Color NewRandomizedColor()
		{
			return this.color;
		}
	}
}
