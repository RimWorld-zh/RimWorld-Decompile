using System;

namespace Verse
{
	// Token: 0x02000B44 RID: 2884
	public class PawnCapacityModifier
	{
		// Token: 0x0400299C RID: 10652
		public PawnCapacityDef capacity;

		// Token: 0x0400299D RID: 10653
		public float offset = 0f;

		// Token: 0x0400299E RID: 10654
		public float setMax = 999f;

		// Token: 0x0400299F RID: 10655
		public float postFactor = 1f;

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06003F4A RID: 16202 RVA: 0x002158EC File Offset: 0x00213CEC
		public bool SetMaxDefined
		{
			get
			{
				return this.setMax != 999f;
			}
		}
	}
}
