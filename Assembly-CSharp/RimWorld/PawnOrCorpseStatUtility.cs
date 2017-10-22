using System;
using Verse;

namespace RimWorld
{
	public static class PawnOrCorpseStatUtility
	{
		public static bool TryGetPawnOrCorpseStat(StatRequest req, Func<Pawn, float> pawnStatGetter, Func<ThingDef, float> pawnDefStatGetter, out float stat)
		{
			bool result;
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					stat = pawnStatGetter(pawn);
					result = true;
					goto IL_00cb;
				}
				Corpse corpse = req.Thing as Corpse;
				if (corpse != null)
				{
					stat = pawnStatGetter(corpse.InnerPawn);
					result = true;
					goto IL_00cb;
				}
			}
			else
			{
				ThingDef thingDef = req.Def as ThingDef;
				if (thingDef != null)
				{
					if (thingDef.category == ThingCategory.Pawn)
					{
						stat = pawnDefStatGetter(thingDef);
						result = true;
						goto IL_00cb;
					}
					if (thingDef.IsCorpse)
					{
						stat = pawnDefStatGetter(thingDef.ingestible.sourceDef);
						result = true;
						goto IL_00cb;
					}
				}
			}
			stat = 0f;
			result = false;
			goto IL_00cb;
			IL_00cb:
			return result;
		}
	}
}
