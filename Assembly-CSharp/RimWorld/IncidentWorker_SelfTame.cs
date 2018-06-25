using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000340 RID: 832
	public class IncidentWorker_SelfTame : IncidentWorker
	{
		// Token: 0x06000E30 RID: 3632 RVA: 0x00078C2C File Offset: 0x0007702C
		private IEnumerable<Pawn> Candidates(Map map)
		{
			return from x in map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Animal && x.Faction == null && !x.Position.Fogged(x.Map) && !x.InMentalState && !x.Downed && x.RaceProps.wildness > 0f
			select x;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00078C70 File Offset: 0x00077070
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return this.Candidates(map).Any<Pawn>();
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00078CA0 File Offset: 0x000770A0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn = null;
			bool result;
			if (!this.Candidates(map).TryRandomElement(out pawn))
			{
				result = false;
			}
			else
			{
				if (pawn.guest != null)
				{
					pawn.guest.SetGuestStatus(null, false);
				}
				string text = pawn.LabelIndefinite();
				bool flag = pawn.Name != null;
				pawn.SetFaction(Faction.OfPlayer, null);
				string text2;
				if (!flag && pawn.Name != null)
				{
					if (pawn.Name.Numerical)
					{
						text2 = "LetterAnimalSelfTameAndNameNumerical".Translate(new object[]
						{
							text,
							pawn.Name.ToStringFull
						}).CapitalizeFirst();
					}
					else
					{
						text2 = "LetterAnimalSelfTameAndName".Translate(new object[]
						{
							text,
							pawn.Name.ToStringFull
						}).CapitalizeFirst();
					}
				}
				else
				{
					text2 = "LetterAnimalSelfTame".Translate(new object[]
					{
						pawn.LabelIndefinite()
					}).CapitalizeFirst();
				}
				Find.LetterStack.ReceiveLetter("LetterLabelAnimalSelfTame".Translate(new object[]
				{
					pawn.KindLabel
				}).CapitalizeFirst(), text2, LetterDefOf.PositiveEvent, pawn, null, null);
				result = true;
			}
			return result;
		}
	}
}
