using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class Settlement : MapParent, ITrader
	{
		public Settlement_TraderTracker trader;

		public List<Pawn> previouslyGeneratedInhabitants = new List<Pawn>();

		public static readonly Texture2D ShowSellableItemsCommand = ContentFinder<Texture2D>.Get("UI/Commands/SellableItems", true);

		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		public static readonly Texture2D AttackCommand = ContentFinder<Texture2D>.Get("UI/Commands/AttackSettlement", true);

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache0;

		public Settlement()
		{
		}

		protected override bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return !this.Attackable;
			}
		}

		public virtual bool Visitable
		{
			get
			{
				return base.Faction != Faction.OfPlayer && (base.Faction == null || !base.Faction.HostileTo(Faction.OfPlayer));
			}
		}

		public virtual bool Attackable
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		public TraderKindDef TraderKind
		{
			get
			{
				TraderKindDef result;
				if (this.trader == null)
				{
					result = null;
				}
				else
				{
					result = this.trader.TraderKind;
				}
				return result;
			}
		}

		public IEnumerable<Thing> Goods
		{
			get
			{
				IEnumerable<Thing> result;
				if (this.trader == null)
				{
					result = null;
				}
				else
				{
					result = this.trader.StockListForReading;
				}
				return result;
			}
		}

		public int RandomPriceFactorSeed
		{
			get
			{
				int result;
				if (this.trader == null)
				{
					result = 0;
				}
				else
				{
					result = this.trader.RandomPriceFactorSeed;
				}
				return result;
			}
		}

		public string TraderName
		{
			get
			{
				string result;
				if (this.trader == null)
				{
					result = null;
				}
				else
				{
					result = this.trader.TraderName;
				}
				return result;
			}
		}

		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				float result;
				if (this.trader == null)
				{
					result = 0f;
				}
				else
				{
					result = this.trader.TradePriceImprovementOffsetForPlayer;
				}
				return result;
			}
		}

		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			IEnumerable<Thing> result;
			if (this.trader == null)
			{
				result = null;
			}
			else
			{
				result = this.trader.ColonyThingsWillingToBuy(playerNegotiator);
			}
			return result;
		}

		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.previouslyGeneratedInhabitants, "previouslyGeneratedInhabitants", LookMode.Reference, new object[0]);
			Scribe_Deep.Look<Settlement_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.previouslyGeneratedInhabitants.RemoveAll((Pawn x) => x == null);
			}
		}

		public override void Tick()
		{
			base.Tick();
			if (this.trader != null)
			{
				this.trader.TraderTrackerTick();
			}
		}

		public override void Notify_MyMapRemoved(Map map)
		{
			base.Notify_MyMapRemoved(map);
			for (int i = this.previouslyGeneratedInhabitants.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.previouslyGeneratedInhabitants[i];
				if (pawn.DestroyedOrNull() || !pawn.IsWorldPawn())
				{
					this.previouslyGeneratedInhabitants.RemoveAt(i);
				}
			}
		}

		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return !base.Map.IsPlayerHome && !base.Map.mapPawns.AnyPawnBlockingMapRemoval;
		}

		public override void PostRemove()
		{
			base.PostRemove();
			if (this.trader != null)
			{
				this.trader.TryDestroyStock();
			}
		}

		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Faction != Faction.OfPlayer)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += base.Faction.PlayerRelationKind.GetLabel();
				if (!base.Faction.def.hidden)
				{
					text = text + " (" + base.Faction.PlayerGoodwill.ToStringWithSign() + ")";
				}
			}
			return text;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			if (this.TraderKind != null)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandShowSellableItems".Translate(),
					defaultDesc = "CommandShowSellableItemsDesc".Translate(),
					icon = Settlement.ShowSellableItemsCommand,
					action = delegate()
					{
						Find.WindowStack.Add(new Dialog_SellableItems(this.TraderKind));
					}
				};
			}
			if (base.Faction != Faction.OfPlayer)
			{
				if (!PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.FormCaravan))
				{
					Command_Action formCaravan = new Command_Action();
					formCaravan.defaultLabel = "CommandFormCaravan".Translate();
					formCaravan.defaultDesc = "CommandFormCaravanDesc".Translate();
					formCaravan.icon = Settlement.FormCaravanCommand;
					formCaravan.action = delegate()
					{
						Find.Tutor.learningReadout.TryActivateConcept(ConceptDefOf.FormCaravan);
						Messages.Message("MessageSelectOwnBaseToFormCaravan".Translate(), MessageTypeDefOf.RejectInput, false);
					};
					yield return formCaravan;
				}
			}
			yield break;
		}

		public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			if (this.CanTradeNow && CaravanVisitUtility.SettlementVisitedNow(caravan) == this)
			{
				yield return CaravanVisitUtility.TradeCommand(caravan);
			}
			if (CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, this))
			{
				yield return FactionGiftUtility.OfferGiftsCommand(caravan, this);
			}
			foreach (Gizmo g in this.<GetCaravanGizmos>__BaseCallProxy1(caravan))
			{
				yield return g;
			}
			if (this.Attackable)
			{
				yield return new Command_Action
				{
					icon = Settlement.AttackCommand,
					defaultLabel = "CommandAttackSettlement".Translate(),
					defaultDesc = "CommandAttackSettlementDesc".Translate(),
					action = delegate()
					{
						SettlementUtility.Attack(caravan, this.$this);
					}
				};
			}
			yield break;
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption o in this.<GetFloatMenuOptions>__BaseCallProxy2(caravan))
			{
				yield return o;
			}
			if (CaravanVisitUtility.SettlementVisitedNow(caravan) != this)
			{
				foreach (FloatMenuOption f in CaravanArrivalAction_VisitSettlement.GetFloatMenuOptions(caravan, this))
				{
					yield return f;
				}
			}
			foreach (FloatMenuOption f2 in CaravanArrivalAction_OfferGifts.GetFloatMenuOptions(caravan, this))
			{
				yield return f2;
			}
			foreach (FloatMenuOption f3 in CaravanArrivalAction_AttackSettlement.GetFloatMenuOptions(caravan, this))
			{
				yield return f3;
			}
			yield break;
		}

		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption o in this.<GetTransportPodsFloatMenuOptions>__BaseCallProxy3(pods, representative))
			{
				yield return o;
			}
			foreach (FloatMenuOption f in TransportPodsArrivalAction_VisitSettlement.GetFloatMenuOptions(representative, pods, this))
			{
				yield return f;
			}
			foreach (FloatMenuOption f2 in TransportPodsArrivalAction_GiveGift.GetFloatMenuOptions(representative, pods, this))
			{
				yield return f2;
			}
			foreach (FloatMenuOption f3 in TransportPodsArrivalAction_AttackSettlement.GetFloatMenuOptions(representative, pods, this))
			{
				yield return f3;
			}
			yield break;
		}

		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			if (this.trader != null)
			{
				outChildren.Add(this.trader);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Settlement()
		{
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__0(Pawn x)
		{
			return x == null;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <GetGizmos>__BaseCallProxy0()
		{
			return base.GetGizmos();
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <GetCaravanGizmos>__BaseCallProxy1(Caravan caravan)
		{
			return base.GetCaravanGizmos(caravan);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<FloatMenuOption> <GetFloatMenuOptions>__BaseCallProxy2(Caravan caravan)
		{
			return base.GetFloatMenuOptions(caravan);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<FloatMenuOption> <GetTransportPodsFloatMenuOptions>__BaseCallProxy3(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			return base.GetTransportPodsFloatMenuOptions(pods, representative);
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <g>__1;

			internal Command_Action <showSellableItems>__2;

			internal Command_Action <formCaravan>__3;

			internal Settlement $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			private static Action <>f__am$cache0;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator0()
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
					enumerator = base.<GetGizmos>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_14A;
				case 3u:
					goto IL_1FE;
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
						g = enumerator.Current;
						this.$current = g;
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
				if (base.TraderKind != null)
				{
					Command_Action showSellableItems = new Command_Action();
					showSellableItems.defaultLabel = "CommandShowSellableItems".Translate();
					showSellableItems.defaultDesc = "CommandShowSellableItemsDesc".Translate();
					showSellableItems.icon = Settlement.ShowSellableItemsCommand;
					showSellableItems.action = delegate()
					{
						Find.WindowStack.Add(new Dialog_SellableItems(base.TraderKind));
					};
					this.$current = showSellableItems;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_14A:
				if (base.Faction != Faction.OfPlayer)
				{
					if (!PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.FormCaravan))
					{
						formCaravan = new Command_Action();
						formCaravan.defaultLabel = "CommandFormCaravan".Translate();
						formCaravan.defaultDesc = "CommandFormCaravanDesc".Translate();
						formCaravan.icon = Settlement.FormCaravanCommand;
						formCaravan.action = delegate()
						{
							Find.Tutor.learningReadout.TryActivateConcept(ConceptDefOf.FormCaravan);
							Messages.Message("MessageSelectOwnBaseToFormCaravan".Translate(), MessageTypeDefOf.RejectInput, false);
						};
						this.$current = formCaravan;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
				}
				IL_1FE:
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Settlement.<GetGizmos>c__Iterator0 <GetGizmos>c__Iterator = new Settlement.<GetGizmos>c__Iterator0();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}

			internal void <>m__0()
			{
				Find.WindowStack.Add(new Dialog_SellableItems(base.TraderKind));
			}

			private static void <>m__1()
			{
				Find.Tutor.learningReadout.TryActivateConcept(ConceptDefOf.FormCaravan);
				Messages.Message("MessageSelectOwnBaseToFormCaravan".Translate(), MessageTypeDefOf.RejectInput, false);
			}
		}

		[CompilerGenerated]
		private sealed class <GetCaravanGizmos>c__Iterator1 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal Caravan caravan;

			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <g>__1;

			internal Command_Action <attack>__2;

			internal Settlement $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			private Settlement.<GetCaravanGizmos>c__Iterator1.<GetCaravanGizmos>c__AnonStorey4 $locvar1;

			[DebuggerHidden]
			public <GetCaravanGizmos>c__Iterator1()
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
					if (base.CanTradeNow && CaravanVisitUtility.SettlementVisitedNow(caravan) == this)
					{
						this.$current = CaravanVisitUtility.TradeCommand(caravan);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_FD;
				case 3u:
					goto IL_122;
				case 4u:
					goto IL_22B;
				default:
					return false;
				}
				if (CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(<GetCaravanGizmos>c__AnonStorey.caravan, this))
				{
					this.$current = FactionGiftUtility.OfferGiftsCommand(<GetCaravanGizmos>c__AnonStorey.caravan, this);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_FD:
				enumerator = base.<GetCaravanGizmos>__BaseCallProxy1(<GetCaravanGizmos>c__AnonStorey.caravan).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_122:
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						g = enumerator.Current;
						this.$current = g;
						if (!this.$disposing)
						{
							this.$PC = 3;
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
				if (this.Attackable)
				{
					Command_Action attack = new Command_Action();
					attack.icon = Settlement.AttackCommand;
					attack.defaultLabel = "CommandAttackSettlement".Translate();
					attack.defaultDesc = "CommandAttackSettlementDesc".Translate();
					attack.action = delegate()
					{
						SettlementUtility.Attack(<GetCaravanGizmos>c__AnonStorey.caravan, <GetCaravanGizmos>c__AnonStorey.<>f__ref$1.$this);
					};
					this.$current = attack;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_22B:
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
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
				case 3u:
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
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Settlement.<GetCaravanGizmos>c__Iterator1 <GetCaravanGizmos>c__Iterator = new Settlement.<GetCaravanGizmos>c__Iterator1();
				<GetCaravanGizmos>c__Iterator.$this = this;
				<GetCaravanGizmos>c__Iterator.caravan = caravan;
				return <GetCaravanGizmos>c__Iterator;
			}

			private sealed class <GetCaravanGizmos>c__AnonStorey4
			{
				internal Caravan caravan;

				internal Settlement.<GetCaravanGizmos>c__Iterator1 <>f__ref$1;

				public <GetCaravanGizmos>c__AnonStorey4()
				{
				}

				internal void <>m__0()
				{
					SettlementUtility.Attack(this.caravan, this.<>f__ref$1.$this);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__Iterator2 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal Caravan caravan;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <o>__1;

			internal IEnumerator<FloatMenuOption> $locvar1;

			internal FloatMenuOption <f>__2;

			internal IEnumerator<FloatMenuOption> $locvar2;

			internal FloatMenuOption <f>__3;

			internal IEnumerator<FloatMenuOption> $locvar3;

			internal FloatMenuOption <f>__4;

			internal Settlement $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetFloatMenuOptions>c__Iterator2()
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
					enumerator = base.<GetFloatMenuOptions>__BaseCallProxy2(caravan).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_FD;
				case 3u:
					Block_5:
					try
					{
						switch (num)
						{
						}
						if (enumerator3.MoveNext())
						{
							f2 = enumerator3.Current;
							this.$current = f2;
							if (!this.$disposing)
							{
								this.$PC = 3;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator3 != null)
							{
								enumerator3.Dispose();
							}
						}
					}
					enumerator4 = CaravanArrivalAction_AttackSettlement.GetFloatMenuOptions(caravan, this).GetEnumerator();
					num = 4294967293u;
					goto Block_6;
				case 4u:
					goto IL_22A;
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
						o = enumerator.Current;
						this.$current = o;
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
				if (CaravanVisitUtility.SettlementVisitedNow(caravan) == this)
				{
					goto IL_174;
				}
				enumerator2 = CaravanArrivalAction_VisitSettlement.GetFloatMenuOptions(caravan, this).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_FD:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						f = enumerator2.Current;
						this.$current = f;
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
				IL_174:
				enumerator3 = CaravanArrivalAction_OfferGifts.GetFloatMenuOptions(caravan, this).GetEnumerator();
				num = 4294967293u;
				goto Block_5;
				Block_6:
				try
				{
					IL_22A:
					switch (num)
					{
					}
					if (enumerator4.MoveNext())
					{
						f3 = enumerator4.Current;
						this.$current = f3;
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator4 != null)
						{
							enumerator4.Dispose();
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
				case 3u:
					try
					{
					}
					finally
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
						}
					}
					break;
				case 4u:
					try
					{
					}
					finally
					{
						if (enumerator4 != null)
						{
							enumerator4.Dispose();
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
				Settlement.<GetFloatMenuOptions>c__Iterator2 <GetFloatMenuOptions>c__Iterator = new Settlement.<GetFloatMenuOptions>c__Iterator2();
				<GetFloatMenuOptions>c__Iterator.$this = this;
				<GetFloatMenuOptions>c__Iterator.caravan = caravan;
				return <GetFloatMenuOptions>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetTransportPodsFloatMenuOptions>c__Iterator3 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal IEnumerable<IThingHolder> pods;

			internal CompLaunchable representative;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <o>__1;

			internal IEnumerator<FloatMenuOption> $locvar1;

			internal FloatMenuOption <f>__2;

			internal IEnumerator<FloatMenuOption> $locvar2;

			internal FloatMenuOption <f>__3;

			internal IEnumerator<FloatMenuOption> $locvar3;

			internal FloatMenuOption <f>__4;

			internal Settlement $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetTransportPodsFloatMenuOptions>c__Iterator3()
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
					enumerator = base.<GetTransportPodsFloatMenuOptions>__BaseCallProxy3(pods, representative).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_F2;
				case 3u:
					goto IL_18E;
				case 4u:
					goto IL_22A;
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
						o = enumerator.Current;
						this.$current = o;
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
				enumerator2 = TransportPodsArrivalAction_VisitSettlement.GetFloatMenuOptions(representative, pods, this).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_F2:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						f = enumerator2.Current;
						this.$current = f;
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
				enumerator3 = TransportPodsArrivalAction_GiveGift.GetFloatMenuOptions(representative, pods, this).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_18E:
					switch (num)
					{
					}
					if (enumerator3.MoveNext())
					{
						f2 = enumerator3.Current;
						this.$current = f2;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
						}
					}
				}
				enumerator4 = TransportPodsArrivalAction_AttackSettlement.GetFloatMenuOptions(representative, pods, this).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_22A:
					switch (num)
					{
					}
					if (enumerator4.MoveNext())
					{
						f3 = enumerator4.Current;
						this.$current = f3;
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator4 != null)
						{
							enumerator4.Dispose();
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
				case 3u:
					try
					{
					}
					finally
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
						}
					}
					break;
				case 4u:
					try
					{
					}
					finally
					{
						if (enumerator4 != null)
						{
							enumerator4.Dispose();
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
				Settlement.<GetTransportPodsFloatMenuOptions>c__Iterator3 <GetTransportPodsFloatMenuOptions>c__Iterator = new Settlement.<GetTransportPodsFloatMenuOptions>c__Iterator3();
				<GetTransportPodsFloatMenuOptions>c__Iterator.$this = this;
				<GetTransportPodsFloatMenuOptions>c__Iterator.pods = pods;
				<GetTransportPodsFloatMenuOptions>c__Iterator.representative = representative;
				return <GetTransportPodsFloatMenuOptions>c__Iterator;
			}
		}
	}
}
