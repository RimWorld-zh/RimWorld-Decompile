using System;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000A12 RID: 2578
	public class Trigger_TicksPassed : Trigger
	{
		// Token: 0x0600399C RID: 14748 RVA: 0x001E7E54 File Offset: 0x001E6254
		public Trigger_TicksPassed(int tickLimit)
		{
			this.data = new TriggerData_TicksPassed();
			this.duration = tickLimit;
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x0600399D RID: 14749 RVA: 0x001E7E78 File Offset: 0x001E6278
		protected TriggerData_TicksPassed Data
		{
			get
			{
				return (TriggerData_TicksPassed)this.data;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x0600399E RID: 14750 RVA: 0x001E7E98 File Offset: 0x001E6298
		public int TicksLeft
		{
			get
			{
				return Mathf.Max(this.duration - this.Data.ticksPassed, 0);
			}
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x001E7EC8 File Offset: 0x001E62C8
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

		// Token: 0x060039A0 RID: 14752 RVA: 0x001E7F36 File Offset: 0x001E6336
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
