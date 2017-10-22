using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class IncidentWorker_TravelerGroup : IncidentWorker_NeutralGroup
	{
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool result;
			IntVec3 travelDest = default(IntVec3);
			if (!base.TryResolveParms(parms))
			{
				result = false;
			}
			else if (!RCellFinder.TryFindTravelDestFrom(parms.spawnCenter, map, out travelDest))
			{
				Log.Warning("Failed to do traveler incident from " + parms.spawnCenter + ": couldn't find anywhere for the traveler to go.");
				result = false;
			}
			else
			{
				List<Pawn> list = base.SpawnPawns(parms);
				if (list.Count == 0)
				{
					result = false;
				}
				else
				{
					string text;
					if (list.Count == 1)
					{
						text = "SingleTravelerPassing".Translate(list[0].story.Title.ToLower(), parms.faction.Name, list[0].Name);
						text = text.AdjustedFor(list[0]);
					}
					else
					{
						text = "GroupTravelersPassing".Translate(parms.faction.Name);
					}
					Messages.Message(text, (Thing)list[0], MessageTypeDefOf.NeutralEvent);
					LordJob_TravelAndExit lordJob = new LordJob_TravelAndExit(travelDest);
					LordMaker.MakeNewLord(parms.faction, lordJob, map, list);
					string label = "";
					string text2 = "";
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref label, ref text2, "LetterRelatedPawnsNeutralGroup".Translate(), true, true);
					if (!text2.NullOrEmpty())
					{
						Find.LetterStack.ReceiveLetter(label, text2, LetterDefOf.NeutralEvent, (Thing)list[0], (string)null);
					}
					result = true;
				}
			}
			return result;
		}
	}
}
