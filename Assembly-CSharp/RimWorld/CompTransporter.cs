using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
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

		public ThingOwner innerContainer;

		public List<TransferableOneWay> leftToLoad;

		private CompLaunchable cachedCompLaunchable;

		private static readonly Texture2D CancelLoadCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		private static readonly Texture2D LoadCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LoadTransporter", true);

		private static readonly Texture2D SelectPreviousInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectPreviousTransporter", true);

		private static readonly Texture2D SelectAllInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectAllTransporters", true);

		private static readonly Texture2D SelectNextInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectNextTransporter", true);

		private static List<CompTransporter> tmpTransportersInGroup = new List<CompTransporter>();

		[CompilerGenerated]
		private static Predicate<TransferableOneWay> <>f__am$cache0;

		public CompTransporter()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

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

		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.CancelLoad(map))
			{
				Messages.Message("MessageTransportersLoadCanceled_TransporterDestroyed".Translate(), MessageTypeDefOf.NegativeEvent, true);
			}
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null);
		}

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
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null);
			if (this.leftToLoad != null)
			{
				this.leftToLoad.Clear();
			}
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static CompTransporter()
		{
		}

		[CompilerGenerated]
		private static bool <get_FirstThingLeftToLoad>m__0(TransferableOneWay x)
		{
			return x.CountToTransfer != 0 && x.HasAnyThing;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <CompGetGizmosExtra>__BaseCallProxy0()
		{
			return base.CompGetGizmosExtra();
		}

		[CompilerGenerated]
		private sealed class <CompGetGizmosExtra>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <g>__1;

			internal Command_Action <cancelLoad>__2;

			internal Command_Action <selectPreviousInGroup>__2;

			internal Command_Action <selectAllInGroup>__2;

			internal Command_Action <selectNextInGroup>__2;

			internal Command_LoadToTransporter <loadGroup>__3;

			internal int <selectedTransportersCount>__3;

			internal CompLaunchable <launchable>__3;

			internal CompTransporter $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <CompGetGizmosExtra>c__Iterator0()
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
					enumerator = base.<CompGetGizmosExtra>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
				{
					Command_Action selectPreviousInGroup = new Command_Action();
					selectPreviousInGroup.defaultLabel = "CommandSelectPreviousTransporter".Translate();
					selectPreviousInGroup.defaultDesc = "CommandSelectPreviousTransporterDesc".Translate();
					selectPreviousInGroup.icon = CompTransporter.SelectPreviousInGroupCommandTex;
					selectPreviousInGroup.action = delegate()
					{
						base.SelectPreviousInGroup();
					};
					this.$current = selectPreviousInGroup;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				case 3u:
				{
					Command_Action selectAllInGroup = new Command_Action();
					selectAllInGroup.defaultLabel = "CommandSelectAllTransporters".Translate();
					selectAllInGroup.defaultDesc = "CommandSelectAllTransportersDesc".Translate();
					selectAllInGroup.icon = CompTransporter.SelectAllInGroupCommandTex;
					selectAllInGroup.action = delegate()
					{
						base.SelectAllInGroup();
					};
					this.$current = selectAllInGroup;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				case 4u:
				{
					Command_Action selectNextInGroup = new Command_Action();
					selectNextInGroup.defaultLabel = "CommandSelectNextTransporter".Translate();
					selectNextInGroup.defaultDesc = "CommandSelectNextTransporterDesc".Translate();
					selectNextInGroup.icon = CompTransporter.SelectNextInGroupCommandTex;
					selectNextInGroup.action = delegate()
					{
						base.SelectNextInGroup();
					};
					this.$current = selectNextInGroup;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				case 5u:
					goto IL_460;
				case 6u:
					goto IL_460;
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
				if (base.LoadingInProgressOrReadyToLaunch)
				{
					Command_Action cancelLoad = new Command_Action();
					cancelLoad.defaultLabel = "CommandCancelLoad".Translate();
					cancelLoad.defaultDesc = "CommandCancelLoadDesc".Translate();
					cancelLoad.icon = CompTransporter.CancelLoadCommandTex;
					cancelLoad.action = delegate()
					{
						SoundDefOf.Designate_Cancel.PlayOneShotOnCamera(null);
						base.CancelLoad();
					};
					this.$current = cancelLoad;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				loadGroup = new Command_LoadToTransporter();
				selectedTransportersCount = 0;
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
				launchable = base.Launchable;
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
				this.$current = loadGroup;
				if (!this.$disposing)
				{
					this.$PC = 6;
				}
				return true;
				IL_460:
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
				CompTransporter.<CompGetGizmosExtra>c__Iterator0 <CompGetGizmosExtra>c__Iterator = new CompTransporter.<CompGetGizmosExtra>c__Iterator0();
				<CompGetGizmosExtra>c__Iterator.$this = this;
				return <CompGetGizmosExtra>c__Iterator;
			}

			internal void <>m__0()
			{
				SoundDefOf.Designate_Cancel.PlayOneShotOnCamera(null);
				base.CancelLoad();
			}

			internal void <>m__1()
			{
				base.SelectPreviousInGroup();
			}

			internal void <>m__2()
			{
				base.SelectAllInGroup();
			}

			internal void <>m__3()
			{
				base.SelectNextInGroup();
			}
		}
	}
}
