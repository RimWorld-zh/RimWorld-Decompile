using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000319 RID: 793
	public class QueuedIncident : IExposable
	{
		// Token: 0x06000D70 RID: 3440 RVA: 0x000737B9 File Offset: 0x00071BB9
		public QueuedIncident()
		{
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x000737C9 File Offset: 0x00071BC9
		public QueuedIncident(FiringIncident firingInc, int fireTick)
		{
			this.firingInc = firingInc;
			this.fireTick = fireTick;
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x000737E8 File Offset: 0x00071BE8
		public int FireTick
		{
			get
			{
				return this.fireTick;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x00073804 File Offset: 0x00071C04
		public FiringIncident FiringIncident
		{
			get
			{
				return this.firingInc;
			}
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0007381F File Offset: 0x00071C1F
		public void ExposeData()
		{
			Scribe_Deep.Look<FiringIncident>(ref this.firingInc, "firingInc", new object[0]);
			Scribe_Values.Look<int>(ref this.fireTick, "fireTick", 0, false);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0007384C File Offset: 0x00071C4C
		public override string ToString()
		{
			return this.fireTick + "->" + this.firingInc.ToString();
		}

		// Token: 0x040008A8 RID: 2216
		private FiringIncident firingInc;

		// Token: 0x040008A9 RID: 2217
		private int fireTick = -1;
	}
}
