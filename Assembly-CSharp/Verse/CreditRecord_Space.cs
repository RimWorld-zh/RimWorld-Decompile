using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDA RID: 3802
	public class CreditRecord_Space : CreditsEntry
	{
		// Token: 0x04003C6B RID: 15467
		private float height = 10f;

		// Token: 0x06005A09 RID: 23049 RVA: 0x002E3CC3 File Offset: 0x002E20C3
		public CreditRecord_Space()
		{
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x002E3CD7 File Offset: 0x002E20D7
		public CreditRecord_Space(float height)
		{
			this.height = height;
		}

		// Token: 0x06005A0B RID: 23051 RVA: 0x002E3CF4 File Offset: 0x002E20F4
		public override float DrawHeight(float width)
		{
			return this.height;
		}

		// Token: 0x06005A0C RID: 23052 RVA: 0x002E3D0F File Offset: 0x002E210F
		public override void Draw(Rect rect)
		{
		}
	}
}
