using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000704 RID: 1796
	public class BreakdownManager : MapComponent
	{
		// Token: 0x040015C3 RID: 5571
		private List<CompBreakdownable> comps = new List<CompBreakdownable>();

		// Token: 0x040015C4 RID: 5572
		public HashSet<Thing> brokenDownThings = new HashSet<Thing>();

		// Token: 0x040015C5 RID: 5573
		public const int CheckIntervalTicks = 1041;

		// Token: 0x06002754 RID: 10068 RVA: 0x00152726 File Offset: 0x00150B26
		public BreakdownManager(Map map) : base(map)
		{
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x00152746 File Offset: 0x00150B46
		public void Register(CompBreakdownable c)
		{
			this.comps.Add(c);
			if (c.BrokenDown)
			{
				this.brokenDownThings.Add(c.parent);
			}
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x00152772 File Offset: 0x00150B72
		public void Deregister(CompBreakdownable c)
		{
			this.comps.Remove(c);
			this.brokenDownThings.Remove(c.parent);
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x00152794 File Offset: 0x00150B94
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

		// Token: 0x06002758 RID: 10072 RVA: 0x001527E8 File Offset: 0x00150BE8
		public void Notify_BrokenDown(Thing thing)
		{
			this.brokenDownThings.Add(thing);
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x001527F8 File Offset: 0x00150BF8
		public void Notify_Repaired(Thing thing)
		{
			this.brokenDownThings.Remove(thing);
		}
	}
}
