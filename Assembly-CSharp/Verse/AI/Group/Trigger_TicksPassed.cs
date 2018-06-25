using System;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000A10 RID: 2576
	public class Trigger_TicksPassed : Trigger
	{
		// Token: 0x040024A3 RID: 9379
		private int duration = 100;

		// Token: 0x0600399A RID: 14746 RVA: 0x001E81C0 File Offset: 0x001E65C0
		public Trigger_TicksPassed(int tickLimit)
		{
			this.data = new TriggerData_TicksPassed();
			this.duration = tickLimit;
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x0600399B RID: 14747 RVA: 0x001E81E4 File Offset: 0x001E65E4
		protected TriggerData_TicksPassed Data
		{
			get
			{
				return (TriggerData_TicksPassed)this.data;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x0600399C RID: 14748 RVA: 0x001E8204 File Offset: 0x001E6604
		public int TicksLeft
		{
			get
			{
				return Mathf.Max(this.duration - this.Data.ticksPassed, 0);
			}
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x001E8234 File Offset: 0x001E6634
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

		// Token: 0x0600399E RID: 14750 RVA: 0x001E82A2 File Offset: 0x001E66A2
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.ticksPassed = 0;
			}
		}
	}
}
