using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000325 RID: 805
	public class IncidentWorker_AnimalInsanitySingle : IncidentWorker
	{
		// Token: 0x040008C5 RID: 2245
		private const int FixedPoints = 30;

		// Token: 0x06000DBC RID: 3516 RVA: 0x00075864 File Offset: 0x00073C64
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

		// Token: 0x06000DBD RID: 3517 RVA: 0x000758A4 File Offset: 0x00073CA4
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
				Find.LetterStack.ReceiveLetter("LetterLabelAnimalInsanitySingle".Translate(), text, LetterDefOf.ThreatSmall, pawn, null, null);
				result = true;
			}
			return result;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00075920 File Offset: 0x00073D20
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
	}
}
