using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_SelfTame : IncidentWorker
	{
		private IEnumerable<Pawn> Candidates(Map map)
		{
			return from x in map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Animal && x.Faction == null && !x.Position.Fogged(x.Map) && !x.InMentalState && !x.Downed && x.RaceProps.wildness > 0.0
			select x;
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Map map = (Map)target;
			return this.Candidates(map).Any();
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn = null;
			if (!this.Candidates(map).TryRandomElementByWeight<Pawn>((Func<Pawn, float>)((Pawn x) => x.RaceProps.wildness), out pawn))
			{
				return false;
			}
			if (pawn.guest != null)
			{
				pawn.guest.SetGuestStatus(null, false);
			}
			string text = pawn.LabelIndefinite();
			bool flag = pawn.Name != null;
			pawn.SetFaction(Faction.OfPlayer, null);
			string text2 = (flag || pawn.Name == null) ? "LetterAnimalSelfTame".Translate(pawn.LabelIndefinite()).CapitalizeFirst() : ((!pawn.Name.Numerical) ? "LetterAnimalSelfTameAndName".Translate(text, pawn.Name.ToStringFull).CapitalizeFirst() : "LetterAnimalSelfTameAndNameNumerical".Translate(text, pawn.Name.ToStringFull).CapitalizeFirst());
			Find.LetterStack.ReceiveLetter("LetterLabelAnimalSelfTame".Translate(pawn.KindLabel).CapitalizeFirst(), text2, LetterDefOf.PositiveEvent, pawn, null);
			return true;
		}
	}
}
