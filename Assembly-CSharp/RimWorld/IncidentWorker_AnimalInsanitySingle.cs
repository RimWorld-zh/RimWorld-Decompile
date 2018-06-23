using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000323 RID: 803
	public class IncidentWorker_AnimalInsanitySingle : IncidentWorker
	{
		// Token: 0x040008C2 RID: 2242
		private const int FixedPoints = 30;

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0007570C File Offset: 0x00073B0C
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

		// Token: 0x06000DBA RID: 3514 RVA: 0x0007574C File Offset: 0x00073B4C
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

		// Token: 0x06000DBB RID: 3515 RVA: 0x000757C8 File Offset: 0x00073BC8
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
