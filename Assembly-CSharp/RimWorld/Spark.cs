using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006DD RID: 1757
	public class Spark : Projectile
	{
		// Token: 0x06002634 RID: 9780 RVA: 0x00147878 File Offset: 0x00145C78
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			FireUtility.TryStartFireIn(base.Position, map, 0.1f);
		}
	}
}
