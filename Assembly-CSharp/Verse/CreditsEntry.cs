using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED9 RID: 3801
	public abstract class CreditsEntry
	{
		// Token: 0x06005A07 RID: 23047
		public abstract float DrawHeight(float width);

		// Token: 0x06005A08 RID: 23048
		public abstract void Draw(Rect rect);
	}
}
