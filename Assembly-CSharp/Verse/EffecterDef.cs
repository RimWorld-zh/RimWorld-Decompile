using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B38 RID: 2872
	public class EffecterDef : Def
	{
		// Token: 0x0400294A RID: 10570
		public List<SubEffecterDef> children = null;

		// Token: 0x0400294B RID: 10571
		public float positionRadius;

		// Token: 0x0400294C RID: 10572
		public FloatRange offsetTowardsTarget;

		// Token: 0x06003F2A RID: 16170 RVA: 0x00214358 File Offset: 0x00212758
		public Effecter Spawn()
		{
			return new Effecter(this);
		}
	}
}
