using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000625 RID: 1573
	[StaticConstructorOnStartup]
	public class TradeRequestComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x0600200B RID: 8203 RVA: 0x0011360C File Offset: 0x00111A0C
		public TradeRequestComp()
		{
			this.rewards = new ThingOwner<Thing>(this);
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x0600200C RID: 8204 RVA: 0x00113628 File Offset: 0x00111A28
		public bool ActiveRequest
		{
			get
			{
				return this.expiration > Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x00113650 File Offset: 0x00111A50
		public override string CompInspectStringExtra()
		{
			string result;
			if (this.ActiveRequest)
			{
				result = "CaravanRequestInfo".Translate(new object[]
				{
					GenLabel.ThingLabel(this.requestThingDef, null, this.requestCount).CapitalizeFirst(),
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

		// Token: 0x0600200E RID: 8206 RVA: 0x001136D8 File Offset: 0x00111AD8
		public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			if (this.ActiveRequest && CaravanVisitUtility.SettlementVisitedNow(caravan) == this.parent)
			{
				yield return this.FulfillRequestCommand(caravan);
			}
			yield break;
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x00113709 File Offset: 0x00111B09
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x00113718 File Offset: 0x00111B18
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.rewards;
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x00113733 File Offset: 0x00111B33
		public void Disable()
		{
			this.expiration = -1;
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x00113740 File Offset: 0x00111B40
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

		// Token: 0x06002013 RID: 8211 RVA: 0x001137B3 File Offset: 0x00111BB3
		public override void PostPostRemove()
		{
			base.PostPostRemove();
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x001137C8 File Offset: 0x00111BC8
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
						GenLabel.ThingLabel(this.requestThingDef, null, this.requestCount)
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
					GenLabel.ThingLabel(this.requestThingDef, null, this.requestCount)
				}));
			}
			return command_Action;
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x00113888 File Offset: 0x00111C88
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

		// Token: 0x06002016 RID: 8214 RVA: 0x0011397C File Offset: 0x00111D7C
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
				result = (apparel == null || !apparel.WornByCorpse);
			}
			return result;
		}

		// Token: 0x04001275 RID: 4725
		public ThingDef requestThingDef;

		// Token: 0x04001276 RID: 4726
		public int requestCount;

		// Token: 0x04001277 RID: 4727
		public ThingOwner rewards;

		// Token: 0x04001278 RID: 4728
		public int expiration = -1;

		// Token: 0x04001279 RID: 4729
		private static readonly Texture2D TradeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/FulfillTradeRequest", true);
	}
}
