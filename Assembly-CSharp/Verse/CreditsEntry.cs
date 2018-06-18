using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED8 RID: 3800
	public abstract class CreditsEntry
	{
		// Token: 0x060059E3 RID: 23011
		public abstract float DrawHeight(float width);

		// Token: 0x060059E4 RID: 23012
		public abstract void Draw(Rect rect);
	}
}
