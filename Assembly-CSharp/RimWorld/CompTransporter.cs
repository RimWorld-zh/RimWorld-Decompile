using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class CompTransporter : ThingComp, IThingHolder
	{
		public int groupID = -1;

		private ThingOwner innerContainer;

		public List<TransferableOneWay> leftToLoad;

		private CompLaunchable cachedCompLaunchable;

		private static readonly Texture2D CancelLoadCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		private static readonly Texture2D LoadCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LoadTransporter", true);

		private static readonly Texture2D SelectPreviousInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectPreviousTransporter", true);

		private static readonly Texture2D SelectAllInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectAllTransporters", true);

		private static readonly Texture2D SelectNextInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectNextTransporter", true);

		private static List<CompTransporter> tmpTransportersInGroup = new List<CompTransporter>();

		public CompProperties_Transporter Props
		{
			get
			{
				return (CompProperties_Transporter)this.props;
			}
		}

		public Map Map
		{
			get
			{
				return this.parent.MapHeld;
			}
		}

		public bool AnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoad != null;
			}
		}

		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.groupID >= 0;
			}
		}

		public bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoadInGroup != null;
			}
		}

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

		public Thing FirstThingLeftToLoad
		{
			get
			{
				if (this.leftToLoad == null)
				{
					return null;
				}
				TransferableOneWay transferableOneWay = this.leftToLoad.Find((TransferableOneWay x) => x.CountToTransfer != 0 && x.HasAnyThing);
				if (transferableOneWay != null)
				{
					return transferableOneWay.AnyThing;
				}
				return null;
			}
		}

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

		public CompTransporter()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

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

		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		public List<CompTransporter> TransportersInGroup(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return null;
			}
			TransporterUtility.GetTransportersInGroup(this.groupID, map, CompTransporter.tmpTransportersInGroup);
			return CompTransporter.tmpTransportersInGroup;
		}

		[DebuggerHidden]
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompTransporter.<CompGetGizmosExtra>c__Iterator16C <CompGetGizmosExtra>c__Iterator16C = new CompTransporter.<CompGetGizmosExtra>c__Iterator16C();
			<CompGetGizmosExtra>c__Iterator16C.<>f__this = this;
			CompTransporter.<CompGetGizmosExtra>c__Iterator16C expr_0E = <CompGetGizmosExtra>c__Iterator16C;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.CancelLoad(map))
			{
				Messages.Message("MessageTransportersLoadCanceled_TransporterDestroyed".Translate(), MessageSound.Negative);
			}
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near);
		}

		public void AddToTheToLoadList(TransferableOneWay t, int count)
		{
			if (!t.HasAnyThing || t.CountToTransfer <= 0)
			{
				return;
			}
			if (this.leftToLoad == null)
			{
				this.leftToLoad = new List<TransferableOneWay>();
			}
			if (TransferableUtility.TransferableMatching<TransferableOneWay>(t.AnyThing, this.leftToLoad) != null)
			{
				Log.Error("Transferable already exists.");
				return;
			}
			TransferableOneWay transferableOneWay = new TransferableOneWay();
			this.leftToLoad.Add(transferableOneWay);
			transferableOneWay.things.AddRange(t.things);
			transferableOneWay.AdjustTo(count);
		}

		public void Notify_ThingAdded(Thing t)
		{
			this.SubtractFromToLoadList(t);
		}

		public bool CancelLoad()
		{
			return this.CancelLoad(this.Map);
		}

		public bool CancelLoad(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return false;
			}
			this.TryRemoveLord(map);
			List<CompTransporter> list = this.TransportersInGroup(map);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].CleanUpLoadingVars(map);
			}
			this.CleanUpLoadingVars(map);
			return true;
		}

		public void TryRemoveLord(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return;
			}
			Lord lord = TransporterUtility.FindLord(this.groupID, map);
			if (lord != null)
			{
				map.lordManager.RemoveLord(lord);
			}
		}

		public void CleanUpLoadingVars(Map map)
		{
			this.groupID = -1;
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near);
			if (this.leftToLoad != null)
			{
				this.leftToLoad.Clear();
			}
		}

		private void SubtractFromToLoadList(Thing t)
		{
			if (this.leftToLoad == null)
			{
				return;
			}
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.leftToLoad);
			if (transferableOneWay == null)
			{
				return;
			}
			transferableOneWay.AdjustBy(-t.stackCount);
			if (transferableOneWay.CountToTransfer <= 0)
			{
				this.leftToLoad.Remove(transferableOneWay);
			}
			if (!this.AnyInGroupHasAnythingLeftToLoad)
			{
				Messages.Message("MessageFinishedLoadingTransporters".Translate(), this.parent, MessageSound.Benefit);
			}
		}

		private void SelectPreviousInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			int num = list.IndexOf(this);
			CameraJumper.TryJumpAndSelect(list[GenMath.PositiveMod(num - 1, list.Count)].parent);
		}

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

		private void SelectNextInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			int num = list.IndexOf(this);
			CameraJumper.TryJumpAndSelect(list[(num + 1) % list.Count].parent);
		}

		virtual IThingHolder get_ParentHolder()
		{
			return base.ParentHolder;
		}
	}
}
