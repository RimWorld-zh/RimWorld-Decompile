using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200031B RID: 795
	public class QueuedIncident : IExposable
	{
		// Token: 0x040008AD RID: 2221
		private FiringIncident firingInc;

		// Token: 0x040008AE RID: 2222
		private int fireTick = -1;

		// Token: 0x06000D73 RID: 3443 RVA: 0x000739C5 File Offset: 0x00071DC5
		public QueuedIncident()
		{
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x000739D5 File Offset: 0x00071DD5
		public QueuedIncident(FiringIncident firingInc, int fireTick)
		{
			this.firingInc = firingInc;
			this.fireTick = fireTick;
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000D75 RID: 3445 RVA: 0x000739F4 File Offset: 0x00071DF4
		public int FireTick
		{
			get
			{
				return this.fireTick;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x00073A10 File Offset: 0x00071E10
		public FiringIncident FiringIncident
		{
			get
			{
				return this.firingInc;
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x00073A2B File Offset: 0x00071E2B
		public void ExposeData()
		{
			Scribe_Deep.Look<FiringIncident>(ref this.firingInc, "firingInc", new object[0]);
			Scribe_Values.Look<int>(ref this.fireTick, "fireTick", 0, false);
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00073A58 File Offset: 0x00071E58
		public override string ToString()
		{
			return this.fireTick + "->" + this.firingInc.ToString();
		}
	}
}
