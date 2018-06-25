using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200031B RID: 795
	public class QueuedIncident : IExposable
	{
		// Token: 0x040008AA RID: 2218
		private FiringIncident firingInc;

		// Token: 0x040008AB RID: 2219
		private int fireTick = -1;

		// Token: 0x06000D74 RID: 3444 RVA: 0x000739BD File Offset: 0x00071DBD
		public QueuedIncident()
		{
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x000739CD File Offset: 0x00071DCD
		public QueuedIncident(FiringIncident firingInc, int fireTick)
		{
			this.firingInc = firingInc;
			this.fireTick = fireTick;
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x000739EC File Offset: 0x00071DEC
		public int FireTick
		{
			get
			{
				return this.fireTick;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000D77 RID: 3447 RVA: 0x00073A08 File Offset: 0x00071E08
		public FiringIncident FiringIncident
		{
			get
			{
				return this.firingInc;
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00073A23 File Offset: 0x00071E23
		public void ExposeData()
		{
			Scribe_Deep.Look<FiringIncident>(ref this.firingInc, "firingInc", new object[0]);
			Scribe_Values.Look<int>(ref this.fireTick, "fireTick", 0, false);
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00073A50 File Offset: 0x00071E50
		public override string ToString()
		{
			return this.fireTick + "->" + this.firingInc.ToString();
		}
	}
}
