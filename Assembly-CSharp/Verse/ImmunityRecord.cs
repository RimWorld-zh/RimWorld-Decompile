using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class ImmunityRecord : IExposable
	{
		public HediffDef hediffDef = null;

		public HediffDef source = null;

		public float immunity = 0f;

		public ImmunityRecord()
		{
		}

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

		public void ImmunityTick(Pawn pawn, bool sick, Hediff diseaseInstance)
		{
			this.immunity += this.ImmunityChangePerTick(pawn, sick, diseaseInstance);
			this.immunity = Mathf.Clamp01(this.immunity);
		}

		public void ExposeData()
		{
			Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
			Scribe_Defs.Look<HediffDef>(ref this.source, "source");
			Scribe_Values.Look<float>(ref this.immunity, "immunity", 0f, false);
		}
	}
}
