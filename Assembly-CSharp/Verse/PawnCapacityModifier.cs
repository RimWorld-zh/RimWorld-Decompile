using System;

namespace Verse
{
	// Token: 0x02000B41 RID: 2881
	public class PawnCapacityModifier
	{
		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06003F47 RID: 16199 RVA: 0x00215530 File Offset: 0x00213930
		public bool SetMaxDefined
		{
			get
			{
				return this.setMax != 999f;
			}
		}

		// Token: 0x04002995 RID: 10645
		public PawnCapacityDef capacity;

		// Token: 0x04002996 RID: 10646
		public float offset = 0f;

		// Token: 0x04002997 RID: 10647
		public float setMax = 999f;

		// Token: 0x04002998 RID: 10648
		public float postFactor = 1f;
	}
}
