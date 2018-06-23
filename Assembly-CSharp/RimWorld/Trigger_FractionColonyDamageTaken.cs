using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001AB RID: 427
	public class Trigger_FractionColonyDamageTaken : Trigger
	{
		// Token: 0x040003B8 RID: 952
		private float desiredColonyDamageFraction;

		// Token: 0x040003B9 RID: 953
		private float minDamage;

		// Token: 0x060008D3 RID: 2259 RVA: 0x000536D4 File Offset: 0x00051AD4
		public Trigger_FractionColonyDamageTaken(float desiredColonyDamageFraction, float minDamage = 3.40282347E+38f)
		{
			this.data = new TriggerData_FractionColonyDamageTaken();
			this.desiredColonyDamageFraction = desiredColonyDamageFraction;
			this.minDamage = minDamage;
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x000536F8 File Offset: 0x00051AF8
		private TriggerData_FractionColonyDamageTaken Data
		{
			get
			{
				return (TriggerData_FractionColonyDamageTaken)this.data;
			}
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x00053718 File Offset: 0x00051B18
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.startColonyDamage = transition.Map.damageWatcher.DamageTakenEver;
			}
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00053748 File Offset: 0x00051B48
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
	}
}
