using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000491 RID: 1169
	public class PawnGroupKindWorker_Trader : PawnGroupKindWorker
	{
		// Token: 0x060014AB RID: 5291 RVA: 0x000B5620 File Offset: 0x000B3A20
		public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker)
		{
			return 0f;
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x000B563C File Offset: 0x000B3A3C
		public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return base.CanGenerateFrom(parms, groupMaker) && groupMaker.traders.Any<PawnGenOption>() && (parms.tile == -1 || groupMaker.carriers.Any((PawnGenOption x) => Find.WorldGrid[parms.tile].biome.IsPackAnimalAllowed(x.kind.race)));
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x000B56B0 File Offset: 0x000B3AB0
		protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
		{
			if (!this.CanGenerateFrom(parms, groupMaker))
			{
				if (errorOnZeroResults)
				{
					Log.Error("Cannot generate trader caravan for " + parms.faction + ".", false);
				}
			}
			else if (!parms.faction.def.caravanTraderKinds.Any<TraderKindDef>())
			{
				Log.Error("Cannot generate trader caravan for " + parms.faction + " because it has no trader kinds.", false);
			}
			else
			{
				PawnGenOption pawnGenOption = groupMaker.traders.FirstOrDefault((PawnGenOption x) => !x.kind.trader);
				if (pawnGenOption != null)
				{
					Log.Error(string.Concat(new object[]
					{
						"Cannot generate arriving trader caravan for ",
						parms.faction,
						" because there is a pawn kind (",
						pawnGenOption.kind.LabelCap,
						") who is not a trader but is in a traders list."
					}), false);
				}
				else
				{
					PawnGenOption pawnGenOption2 = groupMaker.carriers.FirstOrDefault((PawnGenOption x) => !x.kind.RaceProps.packAnimal);
					if (pawnGenOption2 != null)
					{
						Log.Error(string.Concat(new object[]
						{
							"Cannot generate arriving trader caravan for ",
							parms.faction,
							" because there is a pawn kind (",
							pawnGenOption2.kind.LabelCap,
							") who is not a carrier but is in a carriers list."
						}), false);
					}
					else
					{
						TraderKindDef traderKindDef;
						if (parms.traderKind != null)
						{
							traderKindDef = parms.traderKind;
						}
						else
						{
							traderKindDef = parms.faction.def.caravanTraderKinds.RandomElementByWeight((TraderKindDef traderDef) => traderDef.CalculatedCommonality);
						}
						TraderKindDef traderKindDef2 = traderKindDef;
						Pawn pawn = this.GenerateTrader(parms, groupMaker, traderKindDef2);
						outPawns.Add(pawn);
						ThingSetMakerParams parms2 = default(ThingSetMakerParams);
						parms2.traderDef = traderKindDef2;
						parms2.tile = new int?(parms.tile);
						parms2.traderFaction = parms.faction;
						List<Thing> wares = ThingSetMakerDefOf.TraderStock.root.Generate(parms2).InRandomOrder(null).ToList<Thing>();
						foreach (Pawn item in this.GetSlavesAndAnimalsFromWares(parms, pawn, wares))
						{
							outPawns.Add(item);
						}
						this.GenerateCarriers(parms, groupMaker, pawn, wares, outPawns);
						this.GenerateGuards(parms, groupMaker, pawn, wares, outPawns);
					}
				}
			}
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x000B5934 File Offset: 0x000B3D34
		private Pawn GenerateTrader(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, TraderKindDef traderKind)
		{
			PawnKindDef kind = groupMaker.traders.RandomElementByWeight((PawnGenOption x) => x.selectionWeight).kind;
			Faction faction = parms.faction;
			int tile = parms.tile;
			PawnGenerationRequest request = new PawnGenerationRequest(kind, faction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, parms.inhabitants, false, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			pawn.mindState.wantsToTradeWithColony = true;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			pawn.trader.traderKind = traderKind;
			parms.points -= pawn.kindDef.combatPower;
			return pawn;
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x000B5A24 File Offset: 0x000B3E24
		private void GenerateCarriers(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, Pawn trader, List<Thing> wares, List<Pawn> outPawns)
		{
			List<Thing> list = (from x in wares
			where !(x is Pawn)
			select x).ToList<Thing>();
			int i = 0;
			int num = Mathf.CeilToInt((float)list.Count / 8f);
			PawnKindDef kind = (from x in groupMaker.carriers
			where parms.tile == -1 || Find.WorldGrid[parms.tile].biome.IsPackAnimalAllowed(x.kind.race)
			select x).RandomElementByWeight((PawnGenOption x) => x.selectionWeight).kind;
			List<Pawn> list2 = new List<Pawn>();
			for (int j = 0; j < num; j++)
			{
				PawnKindDef kind2 = kind;
				Faction faction = parms.faction;
				int tile = parms.tile;
				PawnGenerationRequest request = new PawnGenerationRequest(kind2, faction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, parms.inhabitants, false, false, false, null, null, null, null, null, null, null, null);
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				if (i < list.Count)
				{
					pawn.inventory.innerContainer.TryAdd(list[i], true);
					i++;
				}
				list2.Add(pawn);
				outPawns.Add(pawn);
			}
			while (i < list.Count)
			{
				list2.RandomElement<Pawn>().inventory.innerContainer.TryAdd(list[i], true);
				i++;
			}
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x000B5BD8 File Offset: 0x000B3FD8
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
			yield break;
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x000B5C0C File Offset: 0x000B400C
		private void GenerateGuards(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, Pawn trader, List<Thing> wares, List<Pawn> outPawns)
		{
			if (groupMaker.guards.Any<PawnGenOption>())
			{
				float points = parms.points;
				foreach (PawnGenOption pawnGenOption in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(points, groupMaker.guards, parms))
				{
					PawnKindDef kind = pawnGenOption.kind;
					Faction faction = parms.faction;
					int tile = parms.tile;
					bool inhabitants = parms.inhabitants;
					PawnGenerationRequest request = new PawnGenerationRequest(kind, faction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, true, 1f, false, true, true, inhabitants, false, false, false, null, null, null, null, null, null, null, null);
					Pawn item = PawnGenerator.GeneratePawn(request);
					outPawns.Add(item);
				}
			}
		}
	}
}
