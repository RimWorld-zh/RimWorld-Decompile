using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000325 RID: 805
	public class IncidentWorker_AnimalInsanitySingle : IncidentWorker
	{
		// Token: 0x040008C2 RID: 2242
		private const int FixedPoints = 30;

		// Token: 0x06000DBD RID: 3517 RVA: 0x0007585C File Offset: 0x00073C5C
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

		// Token: 0x06000DBE RID: 3518 RVA: 0x0007589C File Offset: 0x00073C9C
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

		// Token: 0x06000DBF RID: 3519 RVA: 0x00075918 File Offset: 0x00073D18
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
