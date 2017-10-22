using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnGroupKindWorker_Trader : PawnGroupKindWorker
	{
		public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker)
		{
			return 0f;
		}

		public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return base.CanGenerateFrom(parms, groupMaker) && groupMaker.traders.Any() && (parms.tile == -1 || groupMaker.carriers.Any((Predicate<PawnGenOption>)((PawnGenOption x) => Find.WorldGrid[parms.tile].biome.IsPackAnimalAllowed(x.kind.race))));
		}

		protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
		{
			if (!this.CanGenerateFrom(parms, groupMaker))
			{
				if (errorOnZeroResults)
				{
					Log.Error("Cannot generate trader caravan for " + parms.faction + ".");
				}
			}
			else if (!parms.faction.def.caravanTraderKinds.Any())
			{
				Log.Error("Cannot generate trader caravan for " + parms.faction + " because it has no trader kinds.");
			}
			else
			{
				PawnGenOption pawnGenOption = groupMaker.traders.FirstOrDefault((Func<PawnGenOption, bool>)((PawnGenOption x) => !x.kind.trader));
				if (pawnGenOption != null)
				{
					Log.Error("Cannot generate arriving trader caravan for " + parms.faction + " because there is a pawn kind (" + pawnGenOption.kind.LabelCap + ") who is not a trader but is in a traders list.");
				}
				else
				{
					PawnGenOption pawnGenOption2 = groupMaker.carriers.FirstOrDefault((Func<PawnGenOption, bool>)((PawnGenOption x) => !x.kind.RaceProps.packAnimal));
					if (pawnGenOption2 != null)
					{
						Log.Error("Cannot generate arriving trader caravan for " + parms.faction + " because there is a pawn kind (" + pawnGenOption2.kind.LabelCap + ") who is not a carrier but is in a carriers list.");
					}
					else
					{
						TraderKindDef traderKindDef = (parms.traderKind == null) ? parms.faction.def.caravanTraderKinds.RandomElementByWeight((Func<TraderKindDef, float>)((TraderKindDef traderDef) => traderDef.commonality)) : parms.traderKind;
						Pawn pawn = this.GenerateTrader(parms, groupMaker, traderKindDef);
						outPawns.Add(pawn);
						ItemCollectionGeneratorParams parms2 = new ItemCollectionGeneratorParams
						{
							traderDef = traderKindDef,
							forTile = parms.tile,
							forFaction = parms.faction
						};
						List<Thing> wares = ItemCollectionGeneratorDefOf.TraderStock.Worker.Generate(parms2).InRandomOrder(null).ToList();
						foreach (Pawn slavesAndAnimalsFromWare in this.GetSlavesAndAnimalsFromWares(parms, pawn, wares))
						{
							outPawns.Add(slavesAndAnimalsFromWare);
						}
						this.GenerateCarriers(parms, groupMaker, pawn, wares, outPawns);
						this.GenerateGuards(parms, groupMaker, pawn, wares, outPawns);
					}
				}
			}
		}

		private Pawn GenerateTrader(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, TraderKindDef traderKind)
		{
			int tile = parms.tile;
			PawnGenerationRequest request = new PawnGenerationRequest(groupMaker.traders.RandomElementByWeight((Func<PawnGenOption, float>)((PawnGenOption x) => (float)x.selectionWeight)).kind, parms.faction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, parms.inhabitants, false, null, default(float?), default(float?), default(Gender?), default(float?), (string)null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			pawn.mindState.wantsToTradeWithColony = true;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			pawn.trader.traderKind = traderKind;
			parms.points -= pawn.kindDef.combatPower;
			return pawn;
		}

		private void GenerateCarriers(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, Pawn trader, List<Thing> wares, List<Pawn> outPawns)
		{
			List<Thing> list = (from x in wares
			where !(x is Pawn)
			select x).ToList();
			int i = 0;
			int num = Mathf.CeilToInt((float)((float)list.Count / 8.0));
			PawnKindDef kind = (from x in groupMaker.carriers
			where parms.tile == -1 || Find.WorldGrid[parms.tile].biome.IsPackAnimalAllowed(x.kind.race)
			select x).RandomElementByWeight((Func<PawnGenOption, float>)((PawnGenOption x) => (float)x.selectionWeight)).kind;
			List<Pawn> list2 = new List<Pawn>();
			for (int num2 = 0; num2 < num; num2++)
			{
				int tile = parms.tile;
				PawnGenerationRequest request = new PawnGenerationRequest(kind, parms.faction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, parms.inhabitants, false, null, default(float?), default(float?), default(Gender?), default(float?), (string)null);
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				if (i < list.Count)
				{
					pawn.inventory.innerContainer.TryAdd(list[i], true);
					i++;
				}
				list2.Add(pawn);
				outPawns.Add(pawn);
			}
			for (; i < list.Count; i++)
			{
				list2.RandomElement().inventory.innerContainer.TryAdd(list[i], true);
			}
		}

		private IEnumerable<Pawn> GetSlavesAndAnimalsFromWares(PawnGroupMakerParms parms, Pawn trader, List<Thing> wares)
		{
			for (int i = 0; i < wares.Count; i++)
			{
				Pawn p = wares[i] as Pawn;
				if (p != null)
				{
					if (p.Faction != parms.faction)
					{
						p.SetFaction(parms.faction, null);
					}
					yield return p;
				}
			}
		}

		private void GenerateGuards(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, Pawn trader, List<Thing> wares, List<Pawn> outPawns)
		{
			if (groupMaker.guards.Any())
			{
				float points = parms.points;
				foreach (PawnGenOption item2 in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(points, groupMaker.guards, parms))
				{
					int tile = parms.tile;
					bool inhabitants = parms.inhabitants;
					PawnGenerationRequest request = new PawnGenerationRequest(item2.kind, parms.faction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, true, 1f, false, true, true, inhabitants, false, null, default(float?), default(float?), default(Gender?), default(float?), (string)null);
					Pawn item = PawnGenerator.GeneratePawn(request);
					outPawns.Add(item);
				}
			}
		}
	}
}
