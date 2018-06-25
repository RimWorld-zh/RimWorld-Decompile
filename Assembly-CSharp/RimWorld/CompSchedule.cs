using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000423 RID: 1059
	public class CompSchedule : ThingComp
	{
		// Token: 0x04000B32 RID: 2866
		public const string ScheduledOnSignal = "ScheduledOn";

		// Token: 0x04000B33 RID: 2867
		public const string ScheduledOffSignal = "ScheduledOff";

		// Token: 0x04000B34 RID: 2868
		private bool intAllowed = false;

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x0009FA34 File Offset: 0x0009DE34
		public CompProperties_Schedule Props
		{
			get
			{
				return (CompProperties_Schedule)this.props;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x0600126D RID: 4717 RVA: 0x0009FA54 File Offset: 0x0009DE54
		// (set) Token: 0x0600126E RID: 4718 RVA: 0x0009FA6F File Offset: 0x0009DE6F
		public bool Allowed
		{
			get
			{
				return this.intAllowed;
			}
			set
			{
				if (this.intAllowed != value)
				{
					this.intAllowed = value;
					this.parent.BroadcastCompSignal((!this.intAllowed) ? "ScheduledOff" : "ScheduledOn");
				}
			}
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0009FAAF File Offset: 0x0009DEAF
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.RecalculateAllowed();
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0009FABF File Offset: 0x0009DEBF
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecalculateAllowed();
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0009FAD0 File Offset: 0x0009DED0
		public void RecalculateAllowed()
		{
			float num = GenLocalDate.DayPercent(this.parent);
			if (this.Props.startTime <= this.Props.endTime)
			{
				this.Allowed = (num > this.Props.startTime && num < this.Props.endTime);
			}
			else
			{
				this.Allowed = (num < this.Props.endTime || num > this.Props.startTime);
			}
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0009FB60 File Offset: 0x0009DF60
		public override string CompInspectStringExtra()
		{
			string result;
			if (!this.Allowed)
			{
				result = this.Props.offMessage;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
