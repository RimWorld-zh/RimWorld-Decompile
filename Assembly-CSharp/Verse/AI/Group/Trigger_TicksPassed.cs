using System;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000A11 RID: 2577
	public class Trigger_TicksPassed : Trigger
	{
		// Token: 0x040024B3 RID: 9395
		private int duration = 100;

		// Token: 0x0600399B RID: 14747 RVA: 0x001E84EC File Offset: 0x001E68EC
		public Trigger_TicksPassed(int tickLimit)
		{
			this.data = new TriggerData_TicksPassed();
			this.duration = tickLimit;
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x0600399C RID: 14748 RVA: 0x001E8510 File Offset: 0x001E6910
		protected TriggerData_TicksPassed Data
		{
			get
			{
				return (TriggerData_TicksPassed)this.data;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x0600399D RID: 14749 RVA: 0x001E8530 File Offset: 0x001E6930
		public int TicksLeft
		{
			get
			{
				return Mathf.Max(this.duration - this.Data.ticksPassed, 0);
			}
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x001E8560 File Offset: 0x001E6960
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

		// Token: 0x0600399F RID: 14751 RVA: 0x001E85CE File Offset: 0x001E69CE
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.ticksPassed = 0;
			}
		}
	}
}
