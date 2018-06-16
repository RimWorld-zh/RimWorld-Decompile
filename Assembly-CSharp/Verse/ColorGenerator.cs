using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0A RID: 2826
	public abstract class ColorGenerator
	{
		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06003E87 RID: 16007 RVA: 0x0020F02C File Offset: 0x0020D42C
		public virtual Color ExemplaryColor
		{
			get
			{
				Rand.PushState(764543439);
				Color result = this.NewRandomizedColor();
				Rand.PopState();
				return result;
			}
		}

		// Token: 0x06003E88 RID: 16008
		public abstract Color NewRandomizedColor();
	}
}
