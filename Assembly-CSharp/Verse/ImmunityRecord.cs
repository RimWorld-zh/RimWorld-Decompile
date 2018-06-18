using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D3E RID: 3390
	public class ImmunityRecord : IExposable
	{
		// Token: 0x06004AB7 RID: 19127 RVA: 0x0026F6D8 File Offset: 0x0026DAD8
		public float ImmunityChangePerTick(Pawn pawn, bool sick, Hediff diseaseInstance)
		{
			float result;
			if (!pawn.RaceProps.IsFlesh)
			{
				result = 0f;
			}
			else
			{
				HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.hediffDef.CompProps<HediffCompProperties_Immunizable>();
				float num = (!sick) ? hediffCompProperties_Immunizable.immunityPerDayNotSick : hediffCompProperties_Immunizable.immunityPerDaySick;
				num /= 60000f;
				float num2 = pawn.GetStatValue(StatDefOf.ImmunityGainSpeed, true);
				if (diseaseInstance != null)
				{
					Rand.PushState();
					Rand.Seed = Gen.HashCombineInt(diseaseInstance.loadID ^ Find.World.info.randomValue, 156482735);
					num2 *= Mathf.Lerp(0.8f, 1.2f, Rand.Value);
					Rand.PopState();
				}
				if (num > 0f)
				{
					result = num * num2;
				}
				else
				{
					result = num / num2;
				}
			}
			return result;
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x0026F7A6 File Offset: 0x0026DBA6
		public void ImmunityTick(Pawn pawn, bool sick, Hediff diseaseInstance)
		{
			this.immunity += this.ImmunityChangePerTick(pawn, sick, diseaseInstance);
			this.immunity = Mathf.Clamp01(this.immunity);
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x0026F7D0 File Offset: 0x0026DBD0
		public void ExposeData()
		{
			Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
			Scribe_Defs.Look<HediffDef>(ref this.source, "source");
			Scribe_Values.Look<float>(ref this.immunity, "immunity", 0f, false);
		}

		// Token: 0x04003262 RID: 12898
		public HediffDef hediffDef = null;

		// Token: 0x04003263 RID: 12899
		public HediffDef source = null;

		// Token: 0x04003264 RID: 12900
		public float immunity = 0f;
	}
}
