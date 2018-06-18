using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006DD RID: 1757
	public class Spark : Projectile
	{
		// Token: 0x06002636 RID: 9782 RVA: 0x001478F0 File Offset: 0x00145CF0
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			FireUtility.TryStartFireIn(base.Position, map, 0.1f);
		}
	}
}
