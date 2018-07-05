using System;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanBedUtility
	{
		public static bool InCaravanBed(this Pawn p)
		{
			return p.CurrentCaravanBed() != null;
		}

		public static Building_Bed CurrentCaravanBed(this Pawn p)
		{
			Caravan caravan = p.GetCaravan();
			Building_Bed result;
			if (caravan == null)
			{
				result = null;
			}
			else
			{
				result = caravan.beds.GetBedUsedBy(p);
			}
			return result;
		}

		public static bool WouldBenefitFromRestingInBed(Pawn p)
		{
			return !p.Dead && p.health.hediffSet.HasImmunizableNotImmuneHediff();
		}

		public static string AppendUsingBedsLabel(string str, int bedCount)
		{
			string str2 = (bedCount != 1) ? "UsingBedrolls".Translate(new object[]
			{
				bedCount
			}) : "UsingBedroll".Translate();
			return str + " (" + str2 + ")";
		}
	}
}
