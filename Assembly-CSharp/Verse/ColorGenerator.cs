using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0A RID: 2826
	public abstract class ColorGenerator
	{
		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06003E89 RID: 16009 RVA: 0x0020F100 File Offset: 0x0020D500
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

		// Token: 0x06003E8A RID: 16010
		public abstract Color NewRandomizedColor();
	}
}
