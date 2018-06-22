using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED8 RID: 3800
	public class CreditRecord_Space : CreditsEntry
	{
		// Token: 0x06005A06 RID: 23046 RVA: 0x002E3BA3 File Offset: 0x002E1FA3
		public CreditRecord_Space()
		{
		}

		// Token: 0x06005A07 RID: 23047 RVA: 0x002E3BB7 File Offset: 0x002E1FB7
		public CreditRecord_Space(float height)
		{
			this.height = height;
		}

		// Token: 0x06005A08 RID: 23048 RVA: 0x002E3BD4 File Offset: 0x002E1FD4
		public override float DrawHeight(float width)
		{
			return this.height;
		}

		// Token: 0x06005A09 RID: 23049 RVA: 0x002E3BEF File Offset: 0x002E1FEF
		public override void Draw(Rect rect)
		{
		}

		// Token: 0x04003C6B RID: 15467
		private float height = 10f;
	}
}
