using RimWorld.Planet;
using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ChoiceLetter_ItemStashFeeDemand : ChoiceLetter, IThingHolder
	{
		public Map map;

		public int fee;

		public int siteDaysTimeout;

		public ThingOwner items;

		public Faction siteFaction;

		public SitePartDef sitePart;

		public Faction alliedFaction;

		public bool sitePartsKnown;

		protected override IEnumerable<DiaOption> Choices
		{
			get
			{
				DiaOption accept = new DiaOption("ItemStashQuest_Accept".Translate())
				{
					action = (Action)delegate
					{
						int tile = default(int);
						if (!TileFinder.TryFindNewSiteTile(out tile))
						{
							Find.LetterStack.RemoveLetter(((_003C_003Ec__Iterator19E)/*Error near IL_0044: stateMachine*/)._003C_003Ef__this);
						}
						else
						{
							Site o = IncidentWorker_QuestItemStash.CreateSite(tile, ((_003C_003Ec__Iterator19E)/*Error near IL_0044: stateMachine*/)._003C_003Ef__this.sitePart, ((_003C_003Ec__Iterator19E)/*Error near IL_0044: stateMachine*/)._003C_003Ef__this.siteDaysTimeout, ((_003C_003Ec__Iterator19E)/*Error near IL_0044: stateMachine*/)._003C_003Ef__this.siteFaction, ((_003C_003Ec__Iterator19E)/*Error near IL_0044: stateMachine*/)._003C_003Ef__this.items, ((_003C_003Ec__Iterator19E)/*Error near IL_0044: stateMachine*/)._003C_003Ef__this.sitePartsKnown);
							CameraJumper.TryJumpAndSelect((WorldObject)o);
							TradeUtility.LaunchSilver(((_003C_003Ec__Iterator19E)/*Error near IL_0044: stateMachine*/)._003C_003Ef__this.map, ((_003C_003Ec__Iterator19E)/*Error near IL_0044: stateMachine*/)._003C_003Ef__this.fee);
							Find.LetterStack.RemoveLetter(((_003C_003Ec__Iterator19E)/*Error near IL_0044: stateMachine*/)._003C_003Ef__this);
						}
					},
					resolveTree = true
				};
				if (this.map == null || !TradeUtility.ColonyHasEnoughSilver(this.map, this.fee))
				{
					accept.Disable("NeedSilverLaunchable".Translate(this.fee));
				}
				yield return accept;
				yield return base.Reject;
				yield return base.Postpone;
			}
		}

		public override bool StillValid
		{
			get
			{
				if (!base.StillValid)
				{
					return false;
				}
				if (this.alliedFaction.HostileTo(Faction.OfPlayer))
				{
					return false;
				}
				if (this.map != null && !Find.Maps.Contains(this.map))
				{
					return false;
				}
				return true;
			}
		}

		public ChoiceLetter_ItemStashFeeDemand()
		{
			this.items = new ThingOwner<Thing>(this);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_Values.Look<int>(ref this.fee, "fee", 0, false);
			Scribe_Values.Look<int>(ref this.siteDaysTimeout, "siteDaysTimeout", 0, false);
			Scribe_Deep.Look<ThingOwner>(ref this.items, "items", new object[1]
			{
				this
			});
			Scribe_References.Look<Faction>(ref this.siteFaction, "siteFaction", false);
			Scribe_Defs.Look<SitePartDef>(ref this.sitePart, "sitePart");
			Scribe_References.Look<Faction>(ref this.alliedFaction, "alliedFaction", false);
			Scribe_Values.Look<bool>(ref this.sitePartsKnown, "sitePartsKnown", false, false);
		}

		public override void Removed()
		{
			base.Removed();
			this.items.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return this.items;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		virtual IThingHolder get_ParentHolder()
		{
			return base.ParentHolder;
		}

		IThingHolder IThingHolder.get_ParentHolder()
		{
			//ILSpy generated this explicit interface implementation from .override directive in get_ParentHolder
			return this.get_ParentHolder();
		}
	}
}
