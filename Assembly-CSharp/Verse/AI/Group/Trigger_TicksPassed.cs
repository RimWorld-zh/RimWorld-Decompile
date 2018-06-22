using System;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000A0E RID: 2574
	public class Trigger_TicksPassed : Trigger
	{
		// Token: 0x06003996 RID: 14742 RVA: 0x001E8094 File Offset: 0x001E6494
		public Trigger_TicksPassed(int tickLimit)
		{
			this.data = new TriggerData_TicksPassed();
			this.duration = tickLimit;
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06003997 RID: 14743 RVA: 0x001E80B8 File Offset: 0x001E64B8
		protected TriggerData_TicksPassed Data
		{
			get
			{
				return (TriggerData_TicksPassed)this.data;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06003998 RID: 14744 RVA: 0x001E80D8 File Offset: 0x001E64D8
		public int TicksLeft
		{
			get
			{
				return Mathf.Max(this.duration - this.Data.ticksPassed, 0);
			}
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x001E8108 File Offset: 0x001E6508
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.Tick)
			{
				if (this.data == null || !(this.data is TriggerData_TicksPassed))
				{
					BackCompatibility.TriggerDataTicksPassedNull(this);
				}
				TriggerData_TicksPassed data = this.Data;
				data.ticksPassed++;
				result = (data.ticksPassed > this.duration);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x001E8176 File Offset: 0x001E6576
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.ticksPassed = 0;
			}
		}

		// Token: 0x040024A2 RID: 9378
		private int duration = 100;
	}
}
