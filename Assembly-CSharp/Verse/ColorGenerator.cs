using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B09 RID: 2825
	public abstract class ColorGenerator
	{
		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06003E89 RID: 16009 RVA: 0x0020F848 File Offset: 0x0020DC48
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
