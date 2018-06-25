using System;

namespace Verse
{
	// Token: 0x02000B43 RID: 2883
	public class PawnCapacityModifier
	{
		// Token: 0x04002995 RID: 10645
		public PawnCapacityDef capacity;

		// Token: 0x04002996 RID: 10646
		public float offset = 0f;

		// Token: 0x04002997 RID: 10647
		public float setMax = 999f;

		// Token: 0x04002998 RID: 10648
		public float postFactor = 1f;

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06003F4A RID: 16202 RVA: 0x0021560C File Offset: 0x00213A0C
		public bool SetMaxDefined
		{
			get
			{
				return this.setMax != 999f;
			}
		}
	}
}
