using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0C RID: 2828
	public class ColorGenerator_Single : ColorGenerator
	{
		// Token: 0x06003E8E RID: 16014 RVA: 0x0020F158 File Offset: 0x0020D558
		public override Color NewRandomizedColor()
		{
			return this.color;
		}

		// Token: 0x040027E2 RID: 10210
		public Color color;
	}
}
