using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D9 RID: 1753
	public class Spark : Projectile
	{
		// Token: 0x0600262E RID: 9774 RVA: 0x00147A94 File Offset: 0x00145E94
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			FireUtility.TryStartFireIn(base.Position, map, 0.1f);
		}
	}
}
