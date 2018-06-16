using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001AB RID: 427
	public class Trigger_FractionColonyDamageTaken : Trigger
	{
		// Token: 0x060008D5 RID: 2261 RVA: 0x000536C0 File Offset: 0x00051AC0
		public Trigger_FractionColonyDamageTaken(float desiredColonyDamageFraction, float minDamage = 3.40282347E+38f)
		{
			this.data = new TriggerData_FractionColonyDamageTaken();
			this.desiredColonyDamageFraction = desiredColonyDamageFraction;
			this.minDamage = minDamage;
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x000536E4 File Offset: 0x00051AE4
		private TriggerData_FractionColonyDamageTaken Data
		{
			get
			{
				return (TriggerData_FractionColonyDamageTaken)this.data;
			}
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00053704 File Offset: 0x00051B04
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.startColonyDamage = transition.Map.damageWatcher.DamageTakenEver;
			}
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00053734 File Offset: 0x00051B34
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.Tick)
			{
				if (this.data == null || !(this.data is TriggerData_FractionColonyDamageTaken))
				{
					BackCompatibility.TriggerDataFractionColonyDamageTakenNull(this, lord.Map);
				}
				float num = Mathf.Max((float)lord.initialColonyHealthTotal * this.desiredColonyDamageFraction, this.minDamage);
				result = (lord.Map.damageWatcher.DamageTakenEver > this.Data.startColonyDamage + num);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x040003BA RID: 954
		private float desiredColonyDamageFraction;

		// Token: 0x040003BB RID: 955
		private float minDamage;
	}
}
