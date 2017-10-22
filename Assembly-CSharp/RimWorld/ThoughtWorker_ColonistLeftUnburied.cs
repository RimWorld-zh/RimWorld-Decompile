using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_ColonistLeftUnburied : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.Faction != Faction.OfPlayer)
			{
				result = false;
			}
			else
			{
				List<Thing> list = p.Map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.Corpse));
				for (int i = 0; i < list.Count; i++)
				{
					Corpse corpse = (Corpse)list[i];
					if ((float)corpse.Age > 90000.0 && corpse.InnerPawn.Faction == Faction.OfPlayer && corpse.InnerPawn.def.race.Humanlike && !corpse.IsInAnyStorage())
						goto IL_0094;
				}
				result = false;
			}
			goto IL_00be;
			IL_00be:
			return result;
			IL_0094:
			result = true;
			goto IL_00be;
		}
	}
}
