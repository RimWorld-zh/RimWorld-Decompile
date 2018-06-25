using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006DB RID: 1755
	public class Spark : Projectile
	{
		// Token: 0x06002631 RID: 9777 RVA: 0x00147E44 File Offset: 0x00146244
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			FireUtility.TryStartFireIn(base.Position, map, 0.1f);
		}
	}
}
