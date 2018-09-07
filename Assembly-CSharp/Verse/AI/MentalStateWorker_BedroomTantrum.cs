using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	public class MentalStateWorker_BedroomTantrum : MentalStateWorker
	{
		private static List<Thing> tmpThings = new List<Thing>();

		public MentalStateWorker_BedroomTantrum()
		{
		}

		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			if (ownedBed == null || ownedBed.GetRoom(RegionType.Set_Passable) == null || ownedBed.GetRoom(RegionType.Set_Passable).PsychologicallyOutdoors)
			{
				return false;
			}
			MentalStateWorker_BedroomTantrum.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsIn(ownedBed.GetRoom(RegionType.Set_Passable), pawn, MentalStateWorker_BedroomTantrum.tmpThings, null, 0);
			bool result = MentalStateWorker_BedroomTantrum.tmpThings.Any<Thing>();
			MentalStateWorker_BedroomTantrum.tmpThings.Clear();
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static MentalStateWorker_BedroomTantrum()
		{
		}
	}
}
