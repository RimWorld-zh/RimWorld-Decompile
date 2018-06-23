using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B08 RID: 2824
	public class ColorGenerator_Single : ColorGenerator
	{
		// Token: 0x040027DE RID: 10206
		public Color color;

		// Token: 0x06003E8A RID: 16010 RVA: 0x0020F494 File Offset: 0x0020D894
		public override Color NewRandomizedColor()
		{
			return this.color;
		}
	}
}
