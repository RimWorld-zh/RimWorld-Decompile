using System;
using Verse;

namespace RimWorld
{
	public static class SocialProperness
	{
		public static bool IsSociallyProper(this Thing t, Pawn p)
		{
			return t.IsSociallyProper(p, p.IsPrisonerOfColony, false);
		}

		public static bool IsSociallyProper(this Thing t, Pawn p, bool forPrisoner, bool animalsCare = false)
		{
			bool result;
			if (!animalsCare && p != null && !p.RaceProps.Humanlike)
			{
				result = true;
			}
			else if (!t.def.socialPropernessMatters)
			{
				result = true;
			}
			else if (!t.Spawned)
			{
				result = true;
			}
			else
			{
				IntVec3 intVec = (!t.def.hasInteractionCell) ? t.Position : t.InteractionCell;
				if (forPrisoner)
				{
					result = (p == null || intVec.GetRoom(t.Map, RegionType.Set_Passable) == p.GetRoom(RegionType.Set_Passable));
				}
				else
				{
					result = !intVec.IsInPrisonCell(t.Map);
				}
			}
			return result;
		}
	}
}
