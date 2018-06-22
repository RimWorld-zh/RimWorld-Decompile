using System;

namespace Verse
{
	// Token: 0x02000BD2 RID: 3026
	public class TimeSlower
	{
		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06004208 RID: 16904 RVA: 0x0022D144 File Offset: 0x0022B544
		public bool ForcedNormalSpeed
		{
			get
			{
				return Find.TickManager.TicksGame < this.forceNormalSpeedUntil;
			}
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x0022D16B File Offset: 0x0022B56B
		public void SignalForceNormalSpeed()
		{
			this.forceNormalSpeedUntil = Find.TickManager.TicksGame + 790;
		}

		// Token: 0x0600420A RID: 16906 RVA: 0x0022D184 File Offset: 0x0022B584
		public void SignalForceNormalSpeedShort()
		{
			this.forceNormalSpeedUntil = Find.TickManager.TicksGame + 250;
		}

		// Token: 0x04002D27 RID: 11559
		private int forceNormalSpeedUntil;

		// Token: 0x04002D28 RID: 11560
		private const int ForceTicksStandard = 790;

		// Token: 0x04002D29 RID: 11561
		private const int ForceTicksShort = 250;
	}
}
