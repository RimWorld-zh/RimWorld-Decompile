using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B3B RID: 2875
	public class EffecterDef : Def
	{
		// Token: 0x04002951 RID: 10577
		public List<SubEffecterDef> children = null;

		// Token: 0x04002952 RID: 10578
		public float positionRadius;

		// Token: 0x04002953 RID: 10579
		public FloatRange offsetTowardsTarget;

		// Token: 0x06003F2D RID: 16173 RVA: 0x00214714 File Offset: 0x00212B14
		public Effecter Spawn()
		{
			return new Effecter(this);
		}
	}
}
