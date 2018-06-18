using System;

namespace Verse
{
	// Token: 0x02000B45 RID: 2885
	public class PawnCapacityModifier
	{
		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06003F48 RID: 16200 RVA: 0x00214F18 File Offset: 0x00213318
		public bool SetMaxDefined
		{
			get
			{
				return this.setMax != 999f;
			}
		}

		// Token: 0x04002998 RID: 10648
		public PawnCapacityDef capacity;

		// Token: 0x04002999 RID: 10649
		public float offset = 0f;

		// Token: 0x0400299A RID: 10650
		public float setMax = 999f;

		// Token: 0x0400299B RID: 10651
		public float postFactor = 1f;
	}
}
