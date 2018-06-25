using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000744 RID: 1860
	[StaticConstructorOnStartup]
	public class CompTransporter : ThingComp, IThingHolder
	{
		// Token: 0x04001675 RID: 5749
		public int groupID = -1;

		// Token: 0x04001676 RID: 5750
		public ThingOwner innerContainer;

		// Token: 0x04001677 RID: 5751
		public List<TransferableOneWay> leftToLoad;

		// Token: 0x04001678 RID: 5752
		private CompLaunchable cachedCompLaunchable;

		// Token: 0x04001679 RID: 5753
		private static readonly Texture2D CancelLoadCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x0400167A RID: 5754
		private static readonly Texture2D LoadCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LoadTransporter", true);

		// Token: 0x0400167B RID: 5755
		private static readonly Texture2D SelectPreviousInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectPreviousTransporter", true);

		// Token: 0x0400167C RID: 5756
		private static readonly Texture2D SelectAllInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectAllTransporters", true);

		// Token: 0x0400167D RID: 5757
		private static readonly Texture2D SelectNextInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectNextTransporter", true);

		// Token: 0x0400167E RID: 5758
		private static List<CompTransporter> tmpTransportersInGroup = new List<CompTransporter>();

		// Token: 0x06002919 RID: 10521 RVA: 0x0015E61D File Offset: 0x0015CA1D
		public CompTransporter()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x0600291A RID: 10522 RVA: 0x0015E63C File Offset: 0x0015CA3C
		public CompProperties_Transporter Props
		{
			get
			{
				return (CompProperties_Transporter)this.props;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x0600291B RID: 10523 RVA: 0x0015E65C File Offset: 0x0015CA5C
		public Map Map
		{
			get
			{
				return this.parent.MapHeld;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x0600291C RID: 10524 RVA: 0x0015E67C File Offset: 0x0015CA7C
		public bool AnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoad != null;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x0600291D RID: 10525 RVA: 0x0015E6A0 File Offset: 0x0015CAA0
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.groupID >= 0;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x0600291E RID: 10526 RVA: 0x0015E6C4 File Offset: 0x0015CAC4
		public bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoadInGroup != null;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x0600291F RID: 10527 RVA: 0x0015E6E8 File Offset: 0x0015CAE8
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

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002920 RID: 10528 RVA: 0x0015E720 File Offset: 0x0015CB20
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

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002921 RID: 10529 RVA: 0x0015E784 File Offset: 0x0015CB84
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

		// Token: 0x06002922 RID: 10530 RVA: 0x0015E7E0 File Offset: 0x0015CBE0
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

		// Token: 0x06002923 RID: 10531 RVA: 0x0015E838 File Offset: 0x0015CC38
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x0015E853 File Offset: 0x0015CC53
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x0015E864 File Offset: 0x0015CC64
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

		// Token: 0x06002926 RID: 10534 RVA: 0x0015E908 File Offset: 0x0015CD08
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

		// Token: 0x06002927 RID: 10535 RVA: 0x0015E948 File Offset: 0x0015CD48
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

		// Token: 0x06002928 RID: 10536 RVA: 0x0015E974 File Offset: 0x0015CD74
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.CancelLoad(map))
			{
				Messages.Message("MessageTransportersLoadCanceled_TransporterDestroyed".Translate(), MessageTypeDefOf.NegativeEvent, true);
			}
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null);
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x0015E9C8 File Offset: 0x0015CDC8
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

		// Token: 0x0600292A RID: 10538 RVA: 0x0015EA5A File Offset: 0x0015CE5A
		public void Notify_ThingAdded(Thing t)
		{
			this.SubtractFromToLoadList(t, t.stackCount);
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x0015EA6A File Offset: 0x0015CE6A
		public void Notify_ThingAddedAndMergedWith(Thing t, int mergedCount)
		{
			this.SubtractFromToLoadList(t, mergedCount);
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x0015EA78 File Offset: 0x0015CE78
		public bool CancelLoad()
		{
			return this.CancelLoad(this.Map);
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x0015EA9C File Offset: 0x0015CE9C
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

		// Token: 0x0600292E RID: 10542 RVA: 0x0015EB00 File Offset: 0x0015CF00
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

		// Token: 0x0600292F RID: 10543 RVA: 0x0015EB3D File Offset: 0x0015CF3D
		public void CleanUpLoadingVars(Map map)
		{
			this.groupID = -1;
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null);
			if (this.leftToLoad != null)
			{
				this.leftToLoad.Clear();
			}
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x0015EB78 File Offset: 0x0015CF78
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

		// Token: 0x06002931 RID: 10545 RVA: 0x0015EBFC File Offset: 0x0015CFFC
		private void SelectPreviousInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			int num = list.IndexOf(this);
			CameraJumper.TryJumpAndSelect(list[GenMath.PositiveMod(num - 1, list.Count)].parent);
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x0015EC44 File Offset: 0x0015D044
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

		// Token: 0x06002933 RID: 10547 RVA: 0x0015EC98 File Offset: 0x0015D098
		private void SelectNextInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			int num = list.IndexOf(this);
			CameraJumper.TryJumpAndSelect(list[(num + 1) % list.Count].parent);
		}
	}
}
