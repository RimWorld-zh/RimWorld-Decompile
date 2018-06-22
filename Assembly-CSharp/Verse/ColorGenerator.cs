using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B06 RID: 2822
	public abstract class ColorGenerator
	{
		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06003E85 RID: 16005 RVA: 0x0020F43C File Offset: 0x0020D83C
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

		// Token: 0x06003E86 RID: 16006
		public abstract Color NewRandomizedColor();
	}
}
