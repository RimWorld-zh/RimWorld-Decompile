using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000704 RID: 1796
	public class BreakdownManager : MapComponent
	{
		// Token: 0x040015BF RID: 5567
		private List<CompBreakdownable> comps = new List<CompBreakdownable>();

		// Token: 0x040015C0 RID: 5568
		public HashSet<Thing> brokenDownThings = new HashSet<Thing>();

		// Token: 0x040015C1 RID: 5569
		public const int CheckIntervalTicks = 1041;

		// Token: 0x06002755 RID: 10069 RVA: 0x001524C6 File Offset: 0x001508C6
		public BreakdownManager(Map map) : base(map)
		{
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x001524E6 File Offset: 0x001508E6
		public void Register(CompBreakdownable c)
		{
			this.comps.Add(c);
			if (c.BrokenDown)
			{
				this.brokenDownThings.Add(c.parent);
			}
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x00152512 File Offset: 0x00150912
		public void Deregister(CompBreakdownable c)
		{
			this.comps.Remove(c);
			this.brokenDownThings.Remove(c.parent);
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x00152534 File Offset: 0x00150934
		public override void MapComponentTick()
		{
			if (Find.TickManager.TicksGame % 1041 == 0)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CheckForBreakdown();
				}
			}
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x00152588 File Offset: 0x00150988
		public void Notify_BrokenDown(Thing thing)
		{
			this.brokenDownThings.Add(thing);
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x00152598 File Offset: 0x00150998
		public void Notify_Repaired(Thing thing)
		{
			this.brokenDownThings.Remove(thing);
		}
	}
}
