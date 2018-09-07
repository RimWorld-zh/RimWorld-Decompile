using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld.Planet
{
	public class TransportPodsArrivalAction_AttackSettlement : TransportPodsArrivalAction
	{
		private SettlementBase settlement;

		private PawnsArrivalModeDef arrivalMode;

		public TransportPodsArrivalAction_AttackSettlement()
		{
		}

		public TransportPodsArrivalAction_AttackSettlement(SettlementBase settlement, PawnsArrivalModeDef arrivalMode)
		{
			this.settlement = settlement;
			this.arrivalMode = arrivalMode;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<SettlementBase>(ref this.settlement, "settlement", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, this.settlement);
		}

		public override bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return !this.settlement.HasMap;
		}

		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			bool flag = !this.settlement.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.settlement.Tile, null);
			string label = "LetterLabelCaravanEnteredEnemyBase".Translate();
			string text = "LetterTransportPodsLandedInEnemyBase".Translate(new object[]
			{
				this.settlement.Label
			}).CapitalizeFirst();
			SettlementUtility.AffectRelationsOnAttacked(this.settlement, ref text);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(new object[]
				{
					Faction.OfPlayer.def.pawnsPlural
				}), true, true);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, lookTarget, this.settlement.Faction, null);
			this.arrivalMode.Worker.TravelingTransportPodsArrived(pods, orGenerateMap);
		}

		public static FloatMenuAcceptanceReport CanAttack(IEnumerable<IThingHolder> pods, SettlementBase settlement)
		{
			if (settlement == null || !settlement.Spawned || !settlement.Attackable)
			{
				return false;
			}
			if (!TransportPodsArrivalActionUtility.AnyNonDownedColonist(pods))
			{
				return false;
			}
			if (settlement.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(new object[]
				{
					settlement.EnterCooldownDaysLeft().ToString("0.#")
				}));
			}
			return true;
		}

		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, SettlementBase settlement)
		{
			foreach (FloatMenuOption f in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement), () => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.EdgeDrop), "AttackAndDropAtEdge".Translate(new object[]
			{
				settlement.Label
			}), representative, settlement.Tile))
			{
				yield return f;
			}
			foreach (FloatMenuOption f2 in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement), () => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.CenterDrop), "AttackAndDropInCenter".Translate(new object[]
			{
				settlement.Label
			}), representative, settlement.Tile))
			{
				yield return f2;
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__Iterator0 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal SettlementBase settlement;

			internal CompLaunchable representative;

			internal IEnumerable<IThingHolder> pods;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <f>__1;

			internal IEnumerator<FloatMenuOption> $locvar1;

			internal FloatMenuOption <f>__2;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			private TransportPodsArrivalAction_AttackSettlement.<GetFloatMenuOptions>c__Iterator0.<GetFloatMenuOptions>c__AnonStorey1 $locvar2;

			[DebuggerHidden]
			public <GetFloatMenuOptions>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement), () => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.EdgeDrop), "AttackAndDropAtEdge".Translate(new object[]
					{
						settlement.Label
					}), representative, settlement.Tile).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_1A4;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						f = enumerator.Current;
						this.$current = f;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				enumerator2 = TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(() => TransportPodsArrivalAction_AttackSettlement.CanAttack(<GetFloatMenuOptions>c__AnonStorey.pods, <GetFloatMenuOptions>c__AnonStorey.settlement), () => new TransportPodsArrivalAction_AttackSettlement(<GetFloatMenuOptions>c__AnonStorey.settlement, PawnsArrivalModeDefOf.CenterDrop), "AttackAndDropInCenter".Translate(new object[]
				{
					<GetFloatMenuOptions>c__AnonStorey.settlement.Label
				}), representative, <GetFloatMenuOptions>c__AnonStorey.settlement.Tile).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_1A4:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						f2 = enumerator2.Current;
						this.$current = f2;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.FloatMenuOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TransportPodsArrivalAction_AttackSettlement.<GetFloatMenuOptions>c__Iterator0 <GetFloatMenuOptions>c__Iterator = new TransportPodsArrivalAction_AttackSettlement.<GetFloatMenuOptions>c__Iterator0();
				<GetFloatMenuOptions>c__Iterator.settlement = settlement;
				<GetFloatMenuOptions>c__Iterator.representative = representative;
				<GetFloatMenuOptions>c__Iterator.pods = pods;
				return <GetFloatMenuOptions>c__Iterator;
			}

			private sealed class <GetFloatMenuOptions>c__AnonStorey1
			{
				internal IEnumerable<IThingHolder> pods;

				internal SettlementBase settlement;

				public <GetFloatMenuOptions>c__AnonStorey1()
				{
				}

				internal FloatMenuAcceptanceReport <>m__0()
				{
					return TransportPodsArrivalAction_AttackSettlement.CanAttack(this.pods, this.settlement);
				}

				internal TransportPodsArrivalAction_AttackSettlement <>m__1()
				{
					return new TransportPodsArrivalAction_AttackSettlement(this.settlement, PawnsArrivalModeDefOf.EdgeDrop);
				}

				internal FloatMenuAcceptanceReport <>m__2()
				{
					return TransportPodsArrivalAction_AttackSettlement.CanAttack(this.pods, this.settlement);
				}

				internal TransportPodsArrivalAction_AttackSettlement <>m__3()
				{
					return new TransportPodsArrivalAction_AttackSettlement(this.settlement, PawnsArrivalModeDefOf.CenterDrop);
				}
			}
		}
	}
}
