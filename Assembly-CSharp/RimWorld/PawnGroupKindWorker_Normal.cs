using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000490 RID: 1168
	public class PawnGroupKindWorker_Normal : PawnGroupKindWorker
	{
		// Token: 0x060014A5 RID: 5285 RVA: 0x000B52B4 File Offset: 0x000B36B4
		public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker)
		{
			return (from x in groupMaker.options
			where x.kind.isFighter
			select x).Min((PawnGenOption g) => g.Cost);
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x000B5314 File Offset: 0x000B3714
		public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return base.CanGenerateFrom(parms, groupMaker) && PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms).Any<PawnGenOption>();
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x000B5364 File Offset: 0x000B3764
		protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
		{
			if (!this.CanGenerateFrom(parms, groupMaker))
			{
				if (errorOnZeroResults)
				{
					Log.Error(string.Concat(new object[]
					{
						"Cannot generate pawns for ",
						parms.faction,
						" with ",
						parms.points,
						". Defaulting to a single random cheap group."
					}), false);
				}
			}
			else
			{
				bool flag = parms.raidStrategy == null || parms.raidStrategy.pawnsCanBringFood || (parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer));
				Predicate<Pawn> predicate = (parms.raidStrategy == null) ? null : new Predicate<Pawn>((Pawn p) => parms.raidStrategy.Worker.CanUsePawn(p, outPawns));
				bool flag2 = false;
				foreach (PawnGenOption pawnGenOption in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms))
				{
					PawnKindDef kind = pawnGenOption.kind;
					Faction faction = parms.faction;
					int tile = parms.tile;
					bool allowFood = flag;
					bool inhabitants = parms.inhabitants;
					Predicate<Pawn> validatorPostGear = predicate;
					PawnGenerationRequest request = new PawnGenerationRequest(kind, faction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, true, 1f, false, true, allowFood, inhabitants, false, false, false, null, validatorPostGear, null, null, null, null, null, null);
					Pawn pawn = PawnGenerator.GeneratePawn(request);
					if (parms.forceOneIncap && !flag2)
					{
						pawn.health.forceIncap = true;
						pawn.mindState.canFleeIndividual = false;
						flag2 = true;
					}
					outPawns.Add(pawn);
				}
			}
		}
	}
}
