using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000706 RID: 1798
	public class BreakdownManager : MapComponent
	{
		// Token: 0x06002759 RID: 10073 RVA: 0x001521D2 File Offset: 0x001505D2
		public BreakdownManager(Map map) : base(map)
		{
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x001521F2 File Offset: 0x001505F2
		public void Register(CompBreakdownable c)
		{
			this.comps.Add(c);
			if (c.BrokenDown)
			{
				this.brokenDownThings.Add(c.parent);
			}
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x0015221E File Offset: 0x0015061E
		public void Deregister(CompBreakdownable c)
		{
			this.comps.Remove(c);
			this.brokenDownThings.Remove(c.parent);
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x00152240 File Offset: 0x00150640
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

		// Token: 0x0600275D RID: 10077 RVA: 0x00152294 File Offset: 0x00150694
		public void Notify_BrokenDown(Thing thing)
		{
			this.brokenDownThings.Add(thing);
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x001522A4 File Offset: 0x001506A4
		public void Notify_Repaired(Thing thing)
		{
			this.brokenDownThings.Remove(thing);
		}

		// Token: 0x040015C1 RID: 5569
		private List<CompBreakdownable> comps = new List<CompBreakdownable>();

		// Token: 0x040015C2 RID: 5570
		public HashSet<Thing> brokenDownThings = new HashSet<Thing>();

		// Token: 0x040015C3 RID: 5571
		public const int CheckIntervalTicks = 1041;
	}
}
