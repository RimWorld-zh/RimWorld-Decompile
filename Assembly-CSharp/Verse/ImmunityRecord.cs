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

		public void ExposeData()
		{
			Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
			Scribe_Defs.Look<HediffDef>(ref this.source, "source");
			Scribe_Values.Look<float>(ref this.immunity, "immunity", 0f, false);
		}

		public void ImmunityTick(Pawn pawn, bool sick, Hediff diseaseInstance)
		{
			this.immunity += this.ImmunityChangePerTick(pawn, sick, diseaseInstance);
			this.immunity = Mathf.Clamp01(this.immunity);
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
				if (sick)
				{
					float num = hediffCompProperties_Immunizable.immunityPerDaySick;
					num *= pawn.GetStatValue(StatDefOf.ImmunityGainSpeed, true);
					if (diseaseInstance != null)
					{
						Rand.PushState();
						Rand.Seed = Gen.HashCombineInt(diseaseInstance.loadID ^ Find.World.info.randomValue, 156482735);
						num *= Mathf.Lerp(0.8f, 1.2f, Rand.Value);
						Rand.PopState();
					}
					result = num / 60000f;
				}
				else
				{
					result = hediffCompProperties_Immunizable.immunityPerDayNotSick / 60000f;
				}
			}
			return result;
		}
	}
}
