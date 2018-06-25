using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDA RID: 3802
	public abstract class CreditsEntry
	{
		// Token: 0x06005A07 RID: 23047
		public abstract float DrawHeight(float width);

		// Token: 0x06005A08 RID: 23048
		public abstract void Draw(Rect rect);
	}
}
