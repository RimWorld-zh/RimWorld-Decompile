using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000746 RID: 1862
	[StaticConstructorOnStartup]
	public class CompTransporter : ThingComp, IThingHolder
	{
		// Token: 0x0600291D RID: 10525 RVA: 0x0015E095 File Offset: 0x0015C495
		public CompTransporter()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x0600291E RID: 10526 RVA: 0x0015E0B4 File Offset: 0x0015C4B4
		public CompProperties_Transporter Props
		{
			get
			{
				return (CompProperties_Transporter)this.props;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x0600291F RID: 10527 RVA: 0x0015E0D4 File Offset: 0x0015C4D4
		public Map Map
		{
			get
			{
				return this.parent.MapHeld;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06002920 RID: 10528 RVA: 0x0015E0F4 File Offset: 0x0015C4F4
		public bool AnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoad != null;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06002921 RID: 10529 RVA: 0x0015E118 File Offset: 0x0015C518
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.groupID >= 0;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06002922 RID: 10530 RVA: 0x0015E13C File Offset: 0x0015C53C
		public bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoadInGroup != null;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002923 RID: 10531 RVA: 0x0015E160 File Offset: 0x0015C560
		public CompLaunchable Launchable
		{
			get
			{
				if (this.cachedCompLaunchable == null)
				{
					this.cachedCompLaunchable = this.parent.GetComp<CompLaunchable>();
				}
				return this.cachedCompLaunchable;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002924 RID: 10532 RVA: 0x0015E198 File Offset: 0x0015C598
		public Thing FirstThingLeftToLoad
		{
			get
			{
				Thing result;
				if (this.leftToLoad == null)
				{
					result = null;
				}
				else
				{
					TransferableOneWay transferableOneWay = this.leftToLoad.Find((TransferableOneWay x) => x.CountToTransfer != 0 && x.HasAnyThing);
					if (transferableOneWay != null)
					{
						result = transferableOneWay.AnyThing;
					}
					else
					{
						result = null;
					}
				}
				return result;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002925 RID: 10533 RVA: 0x0015E1FC File Offset: 0x0015C5FC
		public Thing FirstThingLeftToLoadInGroup
		{
			get
			{
				List<CompTransporter> list = this.TransportersInGroup(this.parent.Map);
				for (int i = 0; i < list.Count; i++)
				{
					Thing firstThingLeftToLoad = list[i].FirstThingLeftToLoad;
					if (firstThingLeftToLoad != null)
					{
						return firstThingLeftToLoad;
					}
				}
				return null;
			}
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x0015E258 File Offset: 0x0015C658
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Collections.Look<TransferableOneWay>(ref this.leftToLoad, "leftToLoad", LookMode.Deep, new object[0]);
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x0015E2B0 File Offset: 0x0015C6B0
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x0015E2CB File Offset: 0x0015C6CB
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x0015E2DC File Offset: 0x0015C6DC
		public override void CompTick()
		{
			base.CompTick();
			this.innerContainer.ThingOwnerTick(true);
			if (this.Props.restEffectiveness != 0f)
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Pawn pawn = this.innerContainer[i] as Pawn;
					if (pawn != null && !pawn.Dead && pawn.needs.rest != null)
					{
						pawn.needs.rest.TickResting(this.Props.restEffectiveness);
					}
				}
			}
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x0015E380 File Offset: 0x0015C780
		public List<CompTransporter> TransportersInGroup(Map map)
		{
			List<CompTransporter> result;
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				result = null;
			}
			else
			{
				TransporterUtility.GetTransportersInGroup(this.groupID, map, CompTransporter.tmpTransportersInGroup);
				result = CompTransporter.tmpTransportersInGroup;
			}
			return result;
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x0015E3C0 File Offset: 0x0015C7C0
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo g in this.<CompGetGizmosExtra>__BaseCallProxy0())
			{
				yield return g;
			}
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandCancelLoad".Translate(),
					defaultDesc = "CommandCancelLoadDesc".Translate(),
					icon = CompTransporter.CancelLoadCommandTex,
					action = delegate()
					{
						SoundDefOf.Designate_Cancel.PlayOneShotOnCamera(null);
						this.CancelLoad();
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "CommandSelectPreviousTransporter".Translate(),
					defaultDesc = "CommandSelectPreviousTransporterDesc".Translate(),
					icon = CompTransporter.SelectPreviousInGroupCommandTex,
					action = delegate()
					{
						this.SelectPreviousInGroup();
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "CommandSelectAllTransporters".Translate(),
					defaultDesc = "CommandSelectAllTransportersDesc".Translate(),
					icon = CompTransporter.SelectAllInGroupCommandTex,
					action = delegate()
					{
						this.SelectAllInGroup();
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "CommandSelectNextTransporter".Translate(),
					defaultDesc = "CommandSelectNextTransporterDesc".Translate(),
					icon = CompTransporter.SelectNextInGroupCommandTex,
					action = delegate()
					{
						this.SelectNextInGroup();
					}
				};
			}
			else
			{
				Command_LoadToTransporter loadGroup = new Command_LoadToTransporter();
				int selectedTransportersCount = 0;
				for (int i = 0; i < Find.Selector.NumSelected; i++)
				{
					Thing thing = Find.Selector.SelectedObjectsListForReading[i] as Thing;
					if (thing != null && thing.def == this.parent.def)
					{
						CompLaunchable compLaunchable = thing.TryGetComp<CompLaunchable>();
						if (compLaunchable == null || (compLaunchable.FuelingPortSource != null && compLaunchable.FuelingPortSourceHasAnyFuel))
						{
							selectedTransportersCount++;
						}
					}
				}
				loadGroup.defaultLabel = "CommandLoadTransporter".Translate(new object[]
				{
					selectedTransportersCount.ToString()
				});
				loadGroup.defaultDesc = "CommandLoadTransporterDesc".Translate();
				loadGroup.icon = CompTransporter.LoadCommandTex;
				loadGroup.transComp = this;
				CompLaunchable launchable = this.Launchable;
				if (launchable != null)
				{
					if (!launchable.ConnectedToFuelingPort)
					{
						loadGroup.Disable("CommandLoadTransporterFailNotConnectedToFuelingPort".Translate());
					}
					else if (!launchable.FuelingPortSourceHasAnyFuel)
					{
						loadGroup.Disable("CommandLoadTransporterFailNoFuel".Translate());
					}
				}
				yield return loadGroup;
			}
			yield break;
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x0015E3EC File Offset: 0x0015C7EC
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.CancelLoad(map))
			{
				Messages.Message("MessageTransportersLoadCanceled_TransporterDestroyed".Translate(), MessageTypeDefOf.NegativeEvent, true);
			}
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null);
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x0015E440 File Offset: 0x0015C840
		public void AddToTheToLoadList(TransferableOneWay t, int count)
		{
			if (t.HasAnyThing && t.CountToTransfer > 0)
			{
				if (this.leftToLoad == null)
				{
					this.leftToLoad = new List<TransferableOneWay>();
				}
				if (TransferableUtility.TransferableMatching<TransferableOneWay>(t.AnyThing, this.leftToLoad, TransferAsOneMode.PodsOrCaravanPacking) != null)
				{
					Log.Error("Transferable already exists.", false);
				}
				else
				{
					TransferableOneWay transferableOneWay = new TransferableOneWay();
					this.leftToLoad.Add(transferableOneWay);
					transferableOneWay.things.AddRange(t.things);
					transferableOneWay.AdjustTo(count);
				}
			}
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x0015E4D2 File Offset: 0x0015C8D2
		public void Notify_ThingAdded(Thing t)
		{
			this.SubtractFromToLoadList(t, t.stackCount);
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x0015E4E2 File Offset: 0x0015C8E2
		public void Notify_ThingAddedAndMergedWith(Thing t, int mergedCount)
		{
			this.SubtractFromToLoadList(t, mergedCount);
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x0015E4F0 File Offset: 0x0015C8F0
		public bool CancelLoad()
		{
			return this.CancelLoad(this.Map);
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x0015E514 File Offset: 0x0015C914
		public bool CancelLoad(Map map)
		{
			bool result;
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				result = false;
			}
			else
			{
				this.TryRemoveLord(map);
				List<CompTransporter> list = this.TransportersInGroup(map);
				for (int i = 0; i < list.Count; i++)
				{
					list[i].CleanUpLoadingVars(map);
				}
				this.CleanUpLoadingVars(map);
				result = true;
			}
			return result;
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x0015E578 File Offset: 0x0015C978
		public void TryRemoveLord(Map map)
		{
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				Lord lord = TransporterUtility.FindLord(this.groupID, map);
				if (lord != null)
				{
					map.lordManager.RemoveLord(lord);
				}
			}
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x0015E5B5 File Offset: 0x0015C9B5
		public void CleanUpLoadingVars(Map map)
		{
			this.groupID = -1;
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null);
			if (this.leftToLoad != null)
			{
				this.leftToLoad.Clear();
			}
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x0015E5F0 File Offset: 0x0015C9F0
		private void SubtractFromToLoadList(Thing t, int count)
		{
			if (this.leftToLoad != null)
			{
				TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(t, this.leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
				if (transferableOneWay != null)
				{
					transferableOneWay.AdjustBy(-count);
					if (transferableOneWay.CountToTransfer <= 0)
					{
						this.leftToLoad.Remove(transferableOneWay);
					}
					if (!this.AnyInGroupHasAnythingLeftToLoad)
					{
						Messages.Message("MessageFinishedLoadingTransporters".Translate(), this.parent, MessageTypeDefOf.TaskCompletion, true);
					}
				}
			}
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x0015E674 File Offset: 0x0015CA74
		private void SelectPreviousInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			int num = list.IndexOf(this);
			CameraJumper.TryJumpAndSelect(list[GenMath.PositiveMod(num - 1, list.Count)].parent);
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x0015E6BC File Offset: 0x0015CABC
		private void SelectAllInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			Selector selector = Find.Selector;
			selector.ClearSelection();
			for (int i = 0; i < list.Count; i++)
			{
				selector.Select(list[i].parent, true, true);
			}
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x0015E710 File Offset: 0x0015CB10
		private void SelectNextInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			int num = list.IndexOf(this);
			CameraJumper.TryJumpAndSelect(list[(num + 1) % list.Count].parent);
		}

		// Token: 0x04001673 RID: 5747
		public int groupID = -1;

		// Token: 0x04001674 RID: 5748
		public ThingOwner innerContainer;

		// Token: 0x04001675 RID: 5749
		public List<TransferableOneWay> leftToLoad;

		// Token: 0x04001676 RID: 5750
		private CompLaunchable cachedCompLaunchable;

		// Token: 0x04001677 RID: 5751
		private static readonly Texture2D CancelLoadCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04001678 RID: 5752
		private static readonly Texture2D LoadCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LoadTransporter", true);

		// Token: 0x04001679 RID: 5753
		private static readonly Texture2D SelectPreviousInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectPreviousTransporter", true);

		// Token: 0x0400167A RID: 5754
		private static readonly Texture2D SelectAllInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectAllTransporters", true);

		// Token: 0x0400167B RID: 5755
		private static readonly Texture2D SelectNextInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectNextTransporter", true);

		// Token: 0x0400167C RID: 5756
		private static List<CompTransporter> tmpTransportersInGroup = new List<CompTransporter>();
	}
}
