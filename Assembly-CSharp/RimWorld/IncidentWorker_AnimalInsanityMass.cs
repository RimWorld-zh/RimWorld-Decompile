using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class IncidentWorker_AnimalInsanityMass : IncidentWorker
	{
		public static bool AnimalUsable(Pawn p)
		{
			return p.Spawned && !p.Position.Fogged(p.Map) && (!p.InMentalState || !p.MentalStateDef.IsAggro) && !p.Downed && p.Faction == null;
		}

		public static void DriveInsane(Pawn p)
		{
			p.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, (string)null, true, false, null);
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (parms.points <= 0.0)
			{
				Log.Error("AnimalInsanity running without points.");
				parms.points = (float)(int)(map.strengthWatcher.StrengthRating * 50.0);
			}
			float adjustedPoints = parms.points;
			if (adjustedPoints > 250.0)
			{
				adjustedPoints -= 250f;
				adjustedPoints *= 0.5f;
				adjustedPoints += 250f;
			}
			IEnumerable<PawnKindDef> source = from def in DefDatabase<PawnKindDef>.AllDefs
			where def.RaceProps.Animal && def.combatPower <= adjustedPoints && (from p in map.mapPawns.AllPawnsSpawned
			where p.kindDef == def && IncidentWorker_AnimalInsanityMass.AnimalUsable(p)
			select p).Count() >= 3
			select def;
			PawnKindDef animalDef;
			if (!source.TryRandomElement<PawnKindDef>(out animalDef))
			{
				return false;
			}
			List<Pawn> list = (from p in map.mapPawns.AllPawnsSpawned
			where p.kindDef == animalDef && IncidentWorker_AnimalInsanityMass.AnimalUsable(p)
			select p).ToList();
			float combatPower = animalDef.combatPower;
			float num = 0f;
			int num2 = 0;
			Pawn pawn = null;
			list.Shuffle();
			List<Pawn>.Enumerator enumerator = list.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Pawn current = enumerator.Current;
					if (!(num + combatPower > adjustedPoints))
					{
						IncidentWorker_AnimalInsanityMass.DriveInsane(current);
						num += combatPower;
						num2++;
						pawn = current;
						continue;
					}
					break;
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			if (num == 0.0)
			{
				return false;
			}
			string label;
			string text;
			if (num2 == 1)
			{
				label = "LetterLabelAnimalInsanitySingle".Translate() + ": " + pawn.LabelCap;
				text = "AnimalInsanitySingle".Translate(pawn.LabelShort);
			}
			else
			{
				label = "LetterLabelAnimalInsanityMultiple".Translate() + ": " + animalDef.LabelCap;
				text = "AnimalInsanityMultiple".Translate(animalDef.label);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.BadUrgent, (Thing)pawn, (string)null);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(map);
			if (map == Find.VisibleMap)
			{
				Find.CameraDriver.shaker.DoShake(1f);
			}
			return true;
		}
	}
}
