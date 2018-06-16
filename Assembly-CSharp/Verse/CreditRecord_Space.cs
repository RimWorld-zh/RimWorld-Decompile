using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDA RID: 3802
	public class CreditRecord_Space : CreditsEntry
	{
		// Token: 0x060059E7 RID: 23015 RVA: 0x002E1CB7 File Offset: 0x002E00B7
		public CreditRecord_Space()
		{
		}

		// Token: 0x060059E8 RID: 23016 RVA: 0x002E1CCB File Offset: 0x002E00CB
		public CreditRecord_Space(float height)
		{
			this.height = height;
		}

		// Token: 0x060059E9 RID: 23017 RVA: 0x002E1CE8 File Offset: 0x002E00E8
		public override float DrawHeight(float width)
		{
			return this.height;
		}

		// Token: 0x060059EA RID: 23018 RVA: 0x002E1D03 File Offset: 0x002E0103
		public override void Draw(Rect rect)
		{
		}

		// Token: 0x04003C5C RID: 15452
		private float height = 10f;
	}
}
