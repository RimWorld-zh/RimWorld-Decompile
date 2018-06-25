using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0A RID: 2826
	public class ColorGenerator_Single : ColorGenerator
	{
		// Token: 0x040027DF RID: 10207
		public Color color;

		// Token: 0x06003E8E RID: 16014 RVA: 0x0020F5C0 File Offset: 0x0020D9C0
		public override Color NewRandomizedColor()
		{
			return this.color;
		}
	}
}
