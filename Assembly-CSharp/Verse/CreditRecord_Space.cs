using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED9 RID: 3801
	public class CreditRecord_Space : CreditsEntry
	{
		// Token: 0x060059E5 RID: 23013 RVA: 0x002E1D8F File Offset: 0x002E018F
		public CreditRecord_Space()
		{
		}

		// Token: 0x060059E6 RID: 23014 RVA: 0x002E1DA3 File Offset: 0x002E01A3
		public CreditRecord_Space(float height)
		{
			this.height = height;
		}

		// Token: 0x060059E7 RID: 23015 RVA: 0x002E1DC0 File Offset: 0x002E01C0
		public override float DrawHeight(float width)
		{
			return this.height;
		}

		// Token: 0x060059E8 RID: 23016 RVA: 0x002E1DDB File Offset: 0x002E01DB
		public override void Draw(Rect rect)
		{
		}

		// Token: 0x04003C5B RID: 15451
		private float height = 10f;
	}
}
