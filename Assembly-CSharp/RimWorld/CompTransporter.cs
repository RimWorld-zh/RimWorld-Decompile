using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

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
				return (CompProperties_Transporter)base.props;
			}
		}

		public Map Map
		{
			get
			{
				return base.parent.MapHeld;
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
					this.cachedCompLaunchable = base.parent.GetComp<CompLaunchable>();
				}
				return this.cachedCompLaunchable;
			}
		}

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
					TransferableOneWay transferableOneWay = this.leftToLoad.Find((Predicate<TransferableOneWay>)((TransferableOneWay x) => x.CountToTransfer != 0 && x.HasAnyThing));
					result = ((transferableOneWay == null) ? null : transferableOneWay.AnyThing);
				}
				return result;
			}
		}

		public Thing FirstThingLeftToLoadInGroup
		{
			get
			{
				List<CompTransporter> list = this.TransportersInGroup(base.parent.Map);
				int num = 0;
				Thing result;
				while (true)
				{
					if (num < list.Count)
					{
						Thing firstThingLeftToLoad = list[num].FirstThingLeftToLoad;
						if (firstThingLeftToLoad != null)
						{
							result = firstThingLeftToLoad;
							break;
						}
						num++;
						continue;
					}
					result = null;
					break;
				}
				return result;
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
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[1]
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

		public override void CompTick()
		{
			base.CompTick();
			this.innerContainer.ThingOwnerTick(true);
		}

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

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			using (IEnumerator<Gizmo> enumerator = this._003CCompGetGizmosExtra_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				yield return (Gizmo)new Command_Action
				{
					defaultLabel = "CommandCancelLoad".Translate(),
					defaultDesc = "CommandCancelLoadDesc".Translate(),
					icon = CompTransporter.CancelLoadCommandTex,
					action = (Action)delegate
					{
						SoundDefOf.DesignateCancel.PlayOneShotOnCamera(null);
						((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_0124: stateMachine*/)._0024this.CancelLoad();
					}
				};
				/*Error: Unable to find new state assignment for yield return*/;
			}
			Command_LoadToTransporter loadGroup = new Command_LoadToTransporter();
			int selectedTransportersCount = 0;
			for (int i = 0; i < Find.Selector.NumSelected; i++)
			{
				Thing thing = Find.Selector.SelectedObjectsListForReading[i] as Thing;
				if (thing != null && thing.def == base.parent.def)
				{
					CompLaunchable compLaunchable = thing.TryGetComp<CompLaunchable>();
					if (compLaunchable == null || (compLaunchable.FuelingPortSource != null && compLaunchable.FuelingPortSourceHasAnyFuel))
					{
						selectedTransportersCount++;
					}
				}
			}
			loadGroup.defaultLabel = "CommandLoadTransporter".Translate(selectedTransportersCount.ToString());
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
			yield return (Gizmo)loadGroup;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0469:
			/*Error near IL_046a: Unexpected return in MoveNext()*/;
		}

		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.CancelLoad(map))
			{
				Messages.Message("MessageTransportersLoadCanceled_TransporterDestroyed".Translate(), MessageTypeDefOf.NegativeEvent);
			}
			this.innerContainer.TryDropAll(base.parent.Position, map, ThingPlaceMode.Near);
		}

		public void AddToTheToLoadList(TransferableOneWay t, int count)
		{
			if (t.HasAnyThing && t.CountToTransfer > 0)
			{
				if (this.leftToLoad == null)
				{
					this.leftToLoad = new List<TransferableOneWay>();
				}
				if (TransferableUtility.TransferableMatching(t.AnyThing, this.leftToLoad) != null)
				{
					Log.Error("Transferable already exists.");
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

		public void Notify_ThingAdded(Thing t)
		{
			this.SubtractFromToLoadList(t, t.stackCount);
		}

		public void Notify_ThingAddedAndMergedWith(Thing t, int mergedCount)
		{
			this.SubtractFromToLoadList(t, mergedCount);
		}

		public bool CancelLoad()
		{
			return this.CancelLoad(this.Map);
		}

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

		public void CleanUpLoadingVars(Map map)
		{
			this.groupID = -1;
			this.innerContainer.TryDropAll(base.parent.Position, map, ThingPlaceMode.Near);
			if (this.leftToLoad != null)
			{
				this.leftToLoad.Clear();
			}
		}

		private void SubtractFromToLoadList(Thing t, int count)
		{
			if (this.leftToLoad != null)
			{
				TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(t, this.leftToLoad);
				if (transferableOneWay != null)
				{
					transferableOneWay.AdjustBy(-count);
					if (transferableOneWay.CountToTransfer <= 0)
					{
						this.leftToLoad.Remove(transferableOneWay);
					}
					if (!this.AnyInGroupHasAnythingLeftToLoad)
					{
						Messages.Message("MessageFinishedLoadingTransporters".Translate(), (Thing)base.parent, MessageTypeDefOf.TaskCompletion);
					}
				}
			}
		}

		private void SelectPreviousInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			int num = list.IndexOf(this);
			CameraJumper.TryJumpAndSelect((Thing)list[GenMath.PositiveMod(num - 1, list.Count)].parent);
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
			CameraJumper.TryJumpAndSelect((Thing)list[(num + 1) % list.Count].parent);
		}
	}
}
