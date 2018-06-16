using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020001B4 RID: 436
	public static class SocialProperness
	{
		// Token: 0x06000918 RID: 2328 RVA: 0x000554FC File Offset: 0x000538FC
		public static bool IsSociallyProper(this Thing t, Pawn p)
		{
			return t.IsSociallyProper(p, p.IsPrisonerOfColony, false);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00055520 File Offset: 0x00053920
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
