using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000319 RID: 793
	public class QueuedIncident : IExposable
	{
		// Token: 0x040008AA RID: 2218
		private FiringIncident firingInc;

		// Token: 0x040008AB RID: 2219
		private int fireTick = -1;

		// Token: 0x06000D70 RID: 3440 RVA: 0x0007386D File Offset: 0x00071C6D
		public QueuedIncident()
		{
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0007387D File Offset: 0x00071C7D
		public QueuedIncident(FiringIncident firingInc, int fireTick)
		{
			this.firingInc = firingInc;
			this.fireTick = fireTick;
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x0007389C File Offset: 0x00071C9C
		public int FireTick
		{
			get
			{
				return this.fireTick;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x000738B8 File Offset: 0x00071CB8
		public FiringIncident FiringIncident
		{
			get
			{
				return this.firingInc;
			}
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x000738D3 File Offset: 0x00071CD3
		public void ExposeData()
		{
			Scribe_Deep.Look<FiringIncident>(ref this.firingInc, "firingInc", new object[0]);
			Scribe_Values.Look<int>(ref this.fireTick, "fireTick", 0, false);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00073900 File Offset: 0x00071D00
		public override string ToString()
		{
			return this.fireTick + "->" + this.firingInc.ToString();
		}
	}
}
