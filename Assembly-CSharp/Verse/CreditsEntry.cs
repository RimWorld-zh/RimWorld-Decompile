using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED7 RID: 3799
	public abstract class CreditsEntry
	{
		// Token: 0x06005A04 RID: 23044
		public abstract float DrawHeight(float width);

		// Token: 0x06005A05 RID: 23045
		public abstract void Draw(Rect rect);
	}
}
