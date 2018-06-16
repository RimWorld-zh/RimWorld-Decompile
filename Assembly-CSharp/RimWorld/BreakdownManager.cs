using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000706 RID: 1798
	public class BreakdownManager : MapComponent
	{
		// Token: 0x06002757 RID: 10071 RVA: 0x0015215A File Offset: 0x0015055A
		public BreakdownManager(Map map) : base(map)
		{
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x0015217A File Offset: 0x0015057A
		public void Register(CompBreakdownable c)
		{
			this.comps.Add(c);
			if (c.BrokenDown)
			{
				this.brokenDownThings.Add(c.parent);
			}
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x001521A6 File Offset: 0x001505A6
		public void Deregister(CompBreakdownable c)
		{
			this.comps.Remove(c);
			this.brokenDownThings.Remove(c.parent);
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x001521C8 File Offset: 0x001505C8
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

		// Token: 0x0600275B RID: 10075 RVA: 0x0015221C File Offset: 0x0015061C
		public void Notify_BrokenDown(Thing thing)
		{
			this.brokenDownThings.Add(thing);
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x0015222C File Offset: 0x0015062C
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
