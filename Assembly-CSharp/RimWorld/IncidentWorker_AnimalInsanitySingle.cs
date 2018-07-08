using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_AnimalInsanitySingle : IncidentWorker
	{
		private const int FixedPoints = 30;

		public IncidentWorker_AnimalInsanitySingle()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			if (!base.CanFireNowSub(parms))
			{
				result = false;
			}
			else
			{
				Map map = (Map)parms.target;
				Pawn pawn;
				result = this.TryFindRandomAnimal(map, out pawn);
			}
			return result;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn;
			bool result;
			if (!this.TryFindRandomAnimal(map, out pawn))
			{
				result = false;
			}
			else
			{
				IncidentWorker_AnimalInsanityMass.DriveInsane(pawn);
				string text = "AnimalInsanitySingle".Translate(new object[]
				{
					pawn.Label
				});
				Find.LetterStack.ReceiveLetter("LetterLabelAnimalInsanitySingle".Translate(new object[]
				{
					pawn.Label
				}), text, LetterDefOf.ThreatSmall, pawn, null, null);
				result = true;
			}
			return result;
		}

		private bool TryFindRandomAnimal(Map map, out Pawn animal)
		{
			int maxPoints = 150;
			if (GenDate.DaysPassed < 7)
			{
				maxPoints = 40;
			}
			return (from p in map.mapPawns.AllPawnsSpawned
			where p.RaceProps.Animal && p.kindDef.combatPower <= (float)maxPoints && IncidentWorker_AnimalInsanityMass.AnimalUsable(p)
			select p).TryRandomElement(out animal);
		}

		[CompilerGenerated]
		private sealed class <TryFindRandomAnimal>c__AnonStorey0
		{
			internal int maxPoints;

			public <TryFindRandomAnimal>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Pawn p)
			{
				return p.RaceProps.Animal && p.kindDef.combatPower <= (float)this.maxPoints && IncidentWorker_AnimalInsanityMass.AnimalUsable(p);
			}
		}
	}
}
