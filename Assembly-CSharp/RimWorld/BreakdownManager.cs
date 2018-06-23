using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000702 RID: 1794
	public class BreakdownManager : MapComponent
	{
		// Token: 0x040015BF RID: 5567
		private List<CompBreakdownable> comps = new List<CompBreakdownable>();

		// Token: 0x040015C0 RID: 5568
		public HashSet<Thing> brokenDownThings = new HashSet<Thing>();

		// Token: 0x040015C1 RID: 5569
		public const int CheckIntervalTicks = 1041;

		// Token: 0x06002751 RID: 10065 RVA: 0x00152376 File Offset: 0x00150776
		public BreakdownManager(Map map) : base(map)
		{
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x00152396 File Offset: 0x00150796
		public void Register(CompBreakdownable c)
		{
			this.comps.Add(c);
			if (c.BrokenDown)
			{
				this.brokenDownThings.Add(c.parent);
			}
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x001523C2 File Offset: 0x001507C2
		public void Deregister(CompBreakdownable c)
		{
			this.comps.Remove(c);
			this.brokenDownThings.Remove(c.parent);
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x001523E4 File Offset: 0x001507E4
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

		// Token: 0x06002755 RID: 10069 RVA: 0x00152438 File Offset: 0x00150838
		public void Notify_BrokenDown(Thing thing)
		{
			this.brokenDownThings.Add(thing);
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x00152448 File Offset: 0x00150848
		public void Notify_Repaired(Thing thing)
		{
			this.brokenDownThings.Remove(thing);
		}
	}
}
