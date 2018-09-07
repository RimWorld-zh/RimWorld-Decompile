using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnGroupKindWorker_Trader : PawnGroupKindWorker
	{
		[CompilerGenerated]
		private static Func<PawnGenOption, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<PawnGenOption, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<TraderKindDef, float> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<PawnGenOption, float> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<PawnGenOption, float> <>f__am$cache5;

		public PawnGroupKindWorker_Trader()
		{
		}

		public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker)
		{
			return 0f;
		}

		public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return base.CanGenerateFrom(parms, groupMaker) && groupMaker.traders.Any<PawnGenOption>() && (parms.tile == -1 || groupMaker.carriers.Any((PawnGenOption x) => Find.WorldGrid[parms.tile].biome.IsPackAnimalAllowed(x.kind.race)));
		}

		protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
		{
			if (!this.CanGenerateFrom(parms, groupMaker))
			{
				if (errorOnZeroResults)
				{
					Log.Error("Cannot generate trader caravan for " + parms.faction + ".", false);
				}
				return;
			}
			if (!parms.faction.def.caravanTraderKinds.Any<TraderKindDef>())
			{
				Log.Error("Cannot generate trader caravan for " + parms.faction + " because it has no trader kinds.", false);
				return;
			}
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
				return;
			}
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
				return;
			}
			if (parms.seed != null)
			{
				Log.Warning("Deterministic seed not implemented for this pawn group kind worker. The result will be random anyway.", false);
			}
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

		private void GenerateGuards(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, Pawn trader, List<Thing> wares, List<Pawn> outPawns)
		{
			if (!groupMaker.guards.Any<PawnGenOption>())
			{
				return;
			}
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

		public override IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			throw new NotImplementedException();
		}

		[CompilerGenerated]
		private static bool <GeneratePawns>m__0(PawnGenOption x)
		{
			return !x.kind.trader;
		}

		[CompilerGenerated]
		private static bool <GeneratePawns>m__1(PawnGenOption x)
		{
			return !x.kind.RaceProps.packAnimal;
		}

		[CompilerGenerated]
		private static float <GeneratePawns>m__2(TraderKindDef traderDef)
		{
			return traderDef.CalculatedCommonality;
		}

		[CompilerGenerated]
		private static float <GenerateTrader>m__3(PawnGenOption x)
		{
			return x.selectionWeight;
		}

		[CompilerGenerated]
		private static bool <GenerateCarriers>m__4(Thing x)
		{
			return !(x is Pawn);
		}

		[CompilerGenerated]
		private static float <GenerateCarriers>m__5(PawnGenOption x)
		{
			return x.selectionWeight;
		}

		[CompilerGenerated]
		private sealed class <CanGenerateFrom>c__AnonStorey1
		{
			internal PawnGroupMakerParms parms;

			public <CanGenerateFrom>c__AnonStorey1()
			{
			}

			internal bool <>m__0(PawnGenOption x)
			{
				return Find.WorldGrid[this.parms.tile].biome.IsPackAnimalAllowed(x.kind.race);
			}
		}

		[CompilerGenerated]
		private sealed class <GenerateCarriers>c__AnonStorey2
		{
			internal PawnGroupMakerParms parms;

			public <GenerateCarriers>c__AnonStorey2()
			{
			}

			internal bool <>m__0(PawnGenOption x)
			{
				return this.parms.tile == -1 || Find.WorldGrid[this.parms.tile].biome.IsPackAnimalAllowed(x.kind.race);
			}
		}

		[CompilerGenerated]
		private sealed class <GetSlavesAndAnimalsFromWares>c__Iterator0 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal int <i>__1;

			internal List<Thing> wares;

			internal Pawn <p>__2;

			internal PawnGroupMakerParms parms;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetSlavesAndAnimalsFromWares>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					IL_AB:
					i++;
					break;
				default:
					return false;
				}
				if (i >= wares.Count)
				{
					this.$PC = -1;
				}
				else
				{
					p = (wares[i] as Pawn);
					if (p == null)
					{
						goto IL_AB;
					}
					if (p.Faction != parms.faction)
					{
						p.SetFaction(parms.faction, null);
					}
					this.$current = p;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				return false;
			}

			Pawn IEnumerator<Pawn>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Pawn>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PawnGroupKindWorker_Trader.<GetSlavesAndAnimalsFromWares>c__Iterator0 <GetSlavesAndAnimalsFromWares>c__Iterator = new PawnGroupKindWorker_Trader.<GetSlavesAndAnimalsFromWares>c__Iterator0();
				<GetSlavesAndAnimalsFromWares>c__Iterator.wares = wares;
				<GetSlavesAndAnimalsFromWares>c__Iterator.parms = parms;
				return <GetSlavesAndAnimalsFromWares>c__Iterator;
			}
		}
	}
}
