using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class TradeRequestComp : WorldObjectComp, IThingHolder
	{
		public ThingDef requestThingDef;

		public int requestCount;

		public ThingOwner rewards;

		public int expiration = -1;

		private static readonly Texture2D TradeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/FulfillTradeRequest", true);

		public TradeRequestComp()
		{
			this.rewards = new ThingOwner<Thing>(this);
		}

		public bool ActiveRequest
		{
			get
			{
				return this.expiration > Find.TickManager.TicksGame;
			}
		}

		public override string CompInspectStringExtra()
		{
			string result;
			if (this.ActiveRequest)
			{
				result = "CaravanRequestInfo".Translate(new object[]
				{
					TradeRequestUtility.RequestedThingLabel(this.requestThingDef, this.requestCount).CapitalizeFirst(),
					GenThing.ThingsToCommaList(this.rewards, true, true, -1).CapitalizeFirst(),
					(this.expiration - Find.TickManager.TicksGame).ToStringTicksToDays("F1")
				});
			}
			else
			{
				result = null;
			}
			return result;
		}

		public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			if (this.ActiveRequest && CaravanVisitUtility.SettlementVisitedNow(caravan) == this.parent)
			{
				yield return this.FulfillRequestCommand(caravan);
			}
			yield break;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return this.rewards;
		}

		public void Disable()
		{
			this.expiration = -1;
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.requestThingDef, "requestThingDef");
			Scribe_Values.Look<int>(ref this.requestCount, "requestCount", 0, false);
			Scribe_Deep.Look<ThingOwner>(ref this.rewards, "rewards", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.expiration, "expiration", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.TradeRequestCompPostLoadInit(this);
			}
		}

		public override void PostPostRemove()
		{
			base.PostPostRemove();
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		private Command FulfillRequestCommand(Caravan caravan)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandFulfillTradeOffer".Translate();
			command_Action.defaultDesc = "CommandFulfillTradeOfferDesc".Translate();
			command_Action.icon = TradeRequestComp.TradeCommandTex;
			command_Action.action = delegate()
			{
				if (!this.ActiveRequest)
				{
					Log.Error("Attempted to fulfill an unavailable request", false);
				}
				else if (!CaravanInventoryUtility.HasThings(caravan, this.requestThingDef, this.requestCount, new Func<Thing, bool>(this.PlayerCanGive)))
				{
					Messages.Message("CommandFulfillTradeOfferFailInsufficient".Translate(new object[]
					{
						TradeRequestUtility.RequestedThingLabel(this.requestThingDef, this.requestCount)
					}), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CommandFulfillTradeOfferConfirm".Translate(new object[]
					{
						GenLabel.ThingLabel(this.requestThingDef, null, this.requestCount),
						GenThing.ThingsToCommaList(this.rewards, true, true, -1)
					}), delegate
					{
						this.Fulfill(caravan);
					}, false, null));
				}
			};
			if (!CaravanInventoryUtility.HasThings(caravan, this.requestThingDef, this.requestCount, new Func<Thing, bool>(this.PlayerCanGive)))
			{
				command_Action.Disable("CommandFulfillTradeOfferFailInsufficient".Translate(new object[]
				{
					TradeRequestUtility.RequestedThingLabel(this.requestThingDef, this.requestCount)
				}));
			}
			return command_Action;
		}

		private void Fulfill(Caravan caravan)
		{
			int remaining = this.requestCount;
			List<Thing> list = CaravanInventoryUtility.TakeThings(caravan, delegate(Thing thing)
			{
				int result;
				if (this.requestThingDef != thing.def)
				{
					result = 0;
				}
				else if (!this.PlayerCanGive(thing))
				{
					result = 0;
				}
				else
				{
					int num = Mathf.Min(remaining, thing.stackCount);
					remaining -= num;
					result = num;
				}
				return result;
			});
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Destroy(DestroyMode.Vanish);
			}
			while (this.rewards.Count > 0)
			{
				Thing thing2 = this.rewards.Last<Thing>();
				this.rewards.Remove(thing2);
				CaravanInventoryUtility.GiveThing(caravan, thing2);
			}
			if (this.parent.Faction != null)
			{
				Faction faction = this.parent.Faction;
				Faction ofPlayer = Faction.OfPlayer;
				int goodwillChange = 5;
				string reason = "GoodwillChangedReason_FulfilledTradeRequest".Translate();
				GlobalTargetInfo? lookTarget = new GlobalTargetInfo?(this.parent);
				faction.TryAffectGoodwillWith(ofPlayer, goodwillChange, true, true, reason, lookTarget);
			}
			this.Disable();
		}

		private bool PlayerCanGive(Thing thing)
		{
			bool result;
			if (thing.GetRotStage() != RotStage.Fresh)
			{
				result = false;
			}
			else
			{
				Apparel apparel = thing as Apparel;
				if (apparel != null && apparel.WornByCorpse)
				{
					result = false;
				}
				else
				{
					CompQuality compQuality = thing.TryGetComp<CompQuality>();
					result = (compQuality == null || compQuality.Quality >= QualityCategory.Normal);
				}
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static TradeRequestComp()
		{
		}

		[CompilerGenerated]
		private sealed class <GetCaravanGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal Caravan caravan;

			internal TradeRequestComp $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetCaravanGizmos>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (base.ActiveRequest && CaravanVisitUtility.SettlementVisitedNow(caravan) == this.parent)
					{
						this.$current = base.FulfillRequestCommand(caravan);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
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
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TradeRequestComp.<GetCaravanGizmos>c__Iterator0 <GetCaravanGizmos>c__Iterator = new TradeRequestComp.<GetCaravanGizmos>c__Iterator0();
				<GetCaravanGizmos>c__Iterator.$this = this;
				<GetCaravanGizmos>c__Iterator.caravan = caravan;
				return <GetCaravanGizmos>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <FulfillRequestCommand>c__AnonStorey1
		{
			internal Caravan caravan;

			internal TradeRequestComp $this;

			public <FulfillRequestCommand>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				if (!this.$this.ActiveRequest)
				{
					Log.Error("Attempted to fulfill an unavailable request", false);
				}
				else if (!CaravanInventoryUtility.HasThings(this.caravan, this.$this.requestThingDef, this.$this.requestCount, new Func<Thing, bool>(this.$this.PlayerCanGive)))
				{
					Messages.Message("CommandFulfillTradeOfferFailInsufficient".Translate(new object[]
					{
						TradeRequestUtility.RequestedThingLabel(this.$this.requestThingDef, this.$this.requestCount)
					}), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CommandFulfillTradeOfferConfirm".Translate(new object[]
					{
						GenLabel.ThingLabel(this.$this.requestThingDef, null, this.$this.requestCount),
						GenThing.ThingsToCommaList(this.$this.rewards, true, true, -1)
					}), delegate
					{
						this.$this.Fulfill(this.caravan);
					}, false, null));
				}
			}

			internal void <>m__1()
			{
				this.$this.Fulfill(this.caravan);
			}
		}

		[CompilerGenerated]
		private sealed class <Fulfill>c__AnonStorey2
		{
			internal int remaining;

			internal TradeRequestComp $this;

			public <Fulfill>c__AnonStorey2()
			{
			}

			internal int <>m__0(Thing thing)
			{
				int result;
				if (this.$this.requestThingDef != thing.def)
				{
					result = 0;
				}
				else if (!this.$this.PlayerCanGive(thing))
				{
					result = 0;
				}
				else
				{
					int num = Mathf.Min(this.remaining, thing.stackCount);
					this.remaining -= num;
					result = num;
				}
				return result;
			}
		}
	}
}
