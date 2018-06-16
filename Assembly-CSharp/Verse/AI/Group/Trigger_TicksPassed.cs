using System;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000A12 RID: 2578
	public class Trigger_TicksPassed : Trigger
	{
		// Token: 0x0600399A RID: 14746 RVA: 0x001E7D80 File Offset: 0x001E6180
		public Trigger_TicksPassed(int tickLimit)
		{
			this.data = new TriggerData_TicksPassed();
			this.duration = tickLimit;
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x0600399B RID: 14747 RVA: 0x001E7DA4 File Offset: 0x001E61A4
		protected TriggerData_TicksPassed Data
		{
			get
			{
				return (TriggerData_TicksPassed)this.data;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x0600399C RID: 14748 RVA: 0x001E7DC4 File Offset: 0x001E61C4
		public int TicksLeft
		{
			get
			{
				return Mathf.Max(this.duration - this.Data.ticksPassed, 0);
			}
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x001E7DF4 File Offset: 0x001E61F4
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

		// Token: 0x0600399E RID: 14750 RVA: 0x001E7E62 File Offset: 0x001E6262
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.ticksPassed = 0;
			}
		}

		// Token: 0x040024A7 RID: 9383
		private int duration = 100;
	}
}
