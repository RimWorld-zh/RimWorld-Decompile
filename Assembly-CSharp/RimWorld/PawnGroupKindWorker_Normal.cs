using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class PawnGroupKindWorker_Normal : PawnGroupKindWorker
	{
		public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker)
		{
			return groupMaker.options.Min((Func<PawnGenOption, float>)((PawnGenOption g) => g.Cost));
		}

		public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return (byte)(base.CanGenerateFrom(parms, groupMaker) ? (PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms).Any() ? 1 : 0) : 0) != 0;
		}

		protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
		{
			if (!this.CanGenerateFrom(parms, groupMaker))
			{
				if (errorOnZeroResults)
				{
					Log.Error("Cannot generate pawns for " + parms.faction + " with " + parms.points + ". Defaulting to a single random cheap group.");
				}
			}
			else
			{
				bool flag = parms.raidStrategy == null || parms.raidStrategy.pawnsCanBringFood || (parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer));
				bool flag2 = false;
				foreach (PawnGenOption item in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms))
				{
					PawnKindDef kind = item.kind;
					Faction faction = parms.faction;
					int tile = parms.tile;
					bool allowFood = flag;
					bool inhabitants = parms.inhabitants;
					PawnGenerationRequest request = new PawnGenerationRequest(kind, faction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, true, 1f, false, true, allowFood, inhabitants, false, false, false, null, default(float?), default(float?), default(float?), default(Gender?), default(float?), (string)null);
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
