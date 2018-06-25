using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class IncidentWorker_TravelerGroup : IncidentWorker_NeutralGroup
	{
		private static readonly SimpleCurve PointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(40f, 0f),
				true
			},
			{
				new CurvePoint(50f, 1f),
				true
			},
			{
				new CurvePoint(100f, 1f),
				true
			},
			{
				new CurvePoint(200f, 0.5f),
				true
			},
			{
				new CurvePoint(300f, 0.1f),
				true
			},
			{
				new CurvePoint(500f, 0f),
				true
			}
		};

		public IncidentWorker_TravelerGroup()
		{
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool result;
			IntVec3 travelDest;
			if (!base.TryResolveParms(parms))
			{
				result = false;
			}
			else if (!RCellFinder.TryFindTravelDestFrom(parms.spawnCenter, map, out travelDest))
			{
				Log.Warning("Failed to do traveler incident from " + parms.spawnCenter + ": Couldn't find anywhere for the traveler to go.", false);
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
						text = "SingleTravelerPassing".Translate(new object[]
						{
							list[0].story.Title,
							parms.faction.Name,
							list[0].Name
						});
						text = text.AdjustedFor(list[0], "PAWN");
					}
					else
					{
						text = "GroupTravelersPassing".Translate(new object[]
						{
							parms.faction.Name
						});
					}
					Messages.Message(text, list[0], MessageTypeDefOf.NeutralEvent, true);
					LordJob_TravelAndExit lordJob = new LordJob_TravelAndExit(travelDest);
					LordMaker.MakeNewLord(parms.faction, lordJob, map, list);
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(list, "LetterRelatedPawnsNeutralGroup".Translate(new object[]
					{
						Faction.OfPlayer.def.pawnsPlural
					}), LetterDefOf.NeutralEvent, true, true);
					result = true;
				}
			}
			return result;
		}

		protected override void ResolveParmsPoints(IncidentParms parms)
		{
			if (parms.points < 0f)
			{
				parms.points = Rand.ByCurve(IncidentWorker_TravelerGroup.PointsCurve);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static IncidentWorker_TravelerGroup()
		{
		}
	}
}
