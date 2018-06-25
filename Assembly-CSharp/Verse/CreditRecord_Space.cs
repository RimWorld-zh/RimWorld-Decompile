using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDB RID: 3803
	public class CreditRecord_Space : CreditsEntry
	{
		// Token: 0x04003C73 RID: 15475
		private float height = 10f;

		// Token: 0x06005A09 RID: 23049 RVA: 0x002E3EE3 File Offset: 0x002E22E3
		public CreditRecord_Space()
		{
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x002E3EF7 File Offset: 0x002E22F7
		public CreditRecord_Space(float height)
		{
			this.height = height;
		}

		// Token: 0x06005A0B RID: 23051 RVA: 0x002E3F14 File Offset: 0x002E2314
		public override float DrawHeight(float width)
		{
			return this.height;
		}

		// Token: 0x06005A0C RID: 23052 RVA: 0x002E3F2F File Offset: 0x002E232F
		public override void Draw(Rect rect)
		{
		}
	}
}
