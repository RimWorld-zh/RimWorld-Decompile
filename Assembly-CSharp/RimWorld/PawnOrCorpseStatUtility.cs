using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A6 RID: 2470
	public static class PawnOrCorpseStatUtility
	{
		// Token: 0x06003763 RID: 14179 RVA: 0x001D977C File Offset: 0x001D7B7C
		public static bool TryGetPawnOrCorpseStat(StatRequest req, Func<Pawn, float> pawnStatGetter, Func<ThingDef, float> pawnDefStatGetter, out float stat)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					stat = pawnStatGetter(pawn);
					return true;
				}
				Corpse corpse = req.Thing as Corpse;
				if (corpse != null)
				{
					stat = pawnStatGetter(corpse.InnerPawn);
					return true;
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
						return true;
					}
					if (thingDef.IsCorpse)
					{
						stat = pawnDefStatGetter(thingDef.ingestible.sourceDef);
						return true;
					}
				}
			}
			stat = 0f;
			return false;
		}
	}
}
