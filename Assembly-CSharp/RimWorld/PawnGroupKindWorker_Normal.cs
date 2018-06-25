using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class PawnGroupKindWorker_Normal : PawnGroupKindWorker
	{
		[CompilerGenerated]
		private static Func<PawnGenOption, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<PawnGenOption, float> <>f__am$cache1;

		public PawnGroupKindWorker_Normal()
		{
		}

		public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker)
		{
			return (from x in groupMaker.options
			where x.kind.isFighter
			select x).Min((PawnGenOption g) => g.Cost);
		}

		public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return base.CanGenerateFrom(parms, groupMaker) && PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms).Any<PawnGenOption>();
		}

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

		[CompilerGenerated]
		private static bool <MinPointsToGenerateAnything>m__0(PawnGenOption x)
		{
			return x.kind.isFighter;
		}

		[CompilerGenerated]
		private static float <MinPointsToGenerateAnything>m__1(PawnGenOption g)
		{
			return g.Cost;
		}

		[CompilerGenerated]
		private sealed class <GeneratePawns>c__AnonStorey0
		{
			internal PawnGroupMakerParms parms;

			internal List<Pawn> outPawns;

			public <GeneratePawns>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Pawn p)
			{
				return this.parms.raidStrategy.Worker.CanUsePawn(p, this.outPawns);
			}
		}
	}
}
