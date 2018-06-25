using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class CompLaunchable : ThingComp
	{
		private CompTransporter cachedCompTransporter;

		public static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment", true);

		private static readonly Texture2D LaunchCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);

		private const float FuelPerTile = 2.25f;

		public CompLaunchable()
		{
		}

		public Building FuelingPortSource
		{
			get
			{
				return FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(this.parent.Position, this.parent.Map);
			}
		}

		public bool ConnectedToFuelingPort
		{
			get
			{
				return this.FuelingPortSource != null;
			}
		}

		public bool FuelingPortSourceHasAnyFuel
		{
			get
			{
				return this.ConnectedToFuelingPort && this.FuelingPortSource.GetComp<CompRefuelable>().HasFuel;
			}
		}

		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.Transporter.LoadingInProgressOrReadyToLaunch;
			}
		}

		public bool AnythingLeftToLoad
		{
			get
			{
				return this.Transporter.AnythingLeftToLoad;
			}
		}

		public Thing FirstThingLeftToLoad
		{
			get
			{
				return this.Transporter.FirstThingLeftToLoad;
			}
		}

		public List<CompTransporter> TransportersInGroup
		{
			get
			{
				return this.Transporter.TransportersInGroup(this.parent.Map);
			}
		}

		public bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.Transporter.AnyInGroupHasAnythingLeftToLoad;
			}
		}

		public Thing FirstThingLeftToLoadInGroup
		{
			get
			{
				return this.Transporter.FirstThingLeftToLoadInGroup;
			}
		}

		public bool AnyInGroupIsUnderRoof
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					if (transportersInGroup[i].parent.Position.Roofed(this.parent.Map))
					{
						return true;
					}
				}
				return false;
			}
		}

		public CompTransporter Transporter
		{
			get
			{
				if (this.cachedCompTransporter == null)
				{
					this.cachedCompTransporter = this.parent.GetComp<CompTransporter>();
				}
				return this.cachedCompTransporter;
			}
		}

		public float FuelingPortSourceFuel
		{
			get
			{
				float result;
				if (!this.ConnectedToFuelingPort)
				{
					result = 0f;
				}
				else
				{
					result = this.FuelingPortSource.GetComp<CompRefuelable>().Fuel;
				}
				return result;
			}
		}

		public bool AllInGroupConnectedToFuelingPort
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					if (!transportersInGroup[i].Launchable.ConnectedToFuelingPort)
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool AllFuelingPortSourcesInGroupHaveAnyFuel
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					if (!transportersInGroup[i].Launchable.FuelingPortSourceHasAnyFuel)
					{
						return false;
					}
				}
				return true;
			}
		}

		private float FuelInLeastFueledFuelingPortSource
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				float num = 0f;
				bool flag = false;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					float fuelingPortSourceFuel = transportersInGroup[i].Launchable.FuelingPortSourceFuel;
					if (!flag || fuelingPortSourceFuel < num)
					{
						num = fuelingPortSourceFuel;
						flag = true;
					}
				}
				float result;
				if (!flag)
				{
					result = 0f;
				}
				else
				{
					result = num;
				}
				return result;
			}
		}

		private int MaxLaunchDistance
		{
			get
			{
				int result;
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					result = 0;
				}
				else
				{
					result = CompLaunchable.MaxLaunchDistanceAtFuelLevel(this.FuelInLeastFueledFuelingPortSource);
				}
				return result;
			}
		}

		private int MaxLaunchDistanceEverPossible
		{
			get
			{
				int result;
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					result = 0;
				}
				else
				{
					List<CompTransporter> transportersInGroup = this.TransportersInGroup;
					float num = 0f;
					for (int i = 0; i < transportersInGroup.Count; i++)
					{
						Building fuelingPortSource = transportersInGroup[i].Launchable.FuelingPortSource;
						if (fuelingPortSource != null)
						{
							num = Mathf.Max(num, fuelingPortSource.GetComp<CompRefuelable>().Props.fuelCapacity);
						}
					}
					result = CompLaunchable.MaxLaunchDistanceAtFuelLevel(num);
				}
				return result;
			}
		}

		private bool PodsHaveAnyPotentialCaravanOwner
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					ThingOwner innerContainer = transportersInGroup[i].innerContainer;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && CaravanUtility.IsOwner(pawn, Faction.OfPlayer))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo g in this.<CompGetGizmosExtra>__BaseCallProxy0())
			{
				yield return g;
			}
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				Command_Action launch = new Command_Action();
				launch.defaultLabel = "CommandLaunchGroup".Translate();
				launch.defaultDesc = "CommandLaunchGroupDesc".Translate();
				launch.icon = CompLaunchable.LaunchCommandTex;
				launch.alsoClickIfOtherInGroupClicked = false;
				launch.action = delegate()
				{
					if (this.AnyInGroupHasAnythingLeftToLoad)
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSendNotCompletelyLoadedPods".Translate(new object[]
						{
							this.FirstThingLeftToLoadInGroup.LabelCapNoCount
						}), new Action(this.StartChoosingDestination), false, null));
					}
					else
					{
						this.StartChoosingDestination();
					}
				};
				if (!this.AllInGroupConnectedToFuelingPort)
				{
					launch.Disable("CommandLaunchGroupFailNotConnectedToFuelingPort".Translate());
				}
				else if (!this.AllFuelingPortSourcesInGroupHaveAnyFuel)
				{
					launch.Disable("CommandLaunchGroupFailNoFuel".Translate());
				}
				else if (this.AnyInGroupIsUnderRoof)
				{
					launch.Disable("CommandLaunchGroupFailUnderRoof".Translate());
				}
				yield return launch;
			}
			yield break;
		}

		public override string CompInspectStringExtra()
		{
			string result;
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				if (!this.AllInGroupConnectedToFuelingPort)
				{
					result = "NotReadyForLaunch".Translate() + ": " + "NotAllInGroupConnectedToFuelingPort".Translate() + ".";
				}
				else if (!this.AllFuelingPortSourcesInGroupHaveAnyFuel)
				{
					result = "NotReadyForLaunch".Translate() + ": " + "NotAllFuelingPortSourcesInGroupHaveAnyFuel".Translate() + ".";
				}
				else if (this.AnyInGroupHasAnythingLeftToLoad)
				{
					result = "NotReadyForLaunch".Translate() + ": " + "TransportPodInGroupHasSomethingLeftToLoad".Translate() + ".";
				}
				else
				{
					result = "ReadyForLaunch".Translate();
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		private void StartChoosingDestination()
		{
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(this.parent));
			Find.WorldSelector.ClearSelection();
			int tile = this.parent.Map.Tile;
			Find.WorldTargeter.BeginTargeting(new Func<GlobalTargetInfo, bool>(this.ChoseWorldTarget), true, CompLaunchable.TargeterMouseAttachment, true, delegate
			{
				GenDraw.DrawWorldRadiusRing(tile, this.MaxLaunchDistance);
			}, delegate(GlobalTargetInfo target)
			{
				string result;
				if (!target.IsValid)
				{
					result = null;
				}
				else
				{
					int num = Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile, true, int.MaxValue);
					if (num > this.MaxLaunchDistance)
					{
						GUI.color = Color.red;
						if (num > this.MaxLaunchDistanceEverPossible)
						{
							result = "TransportPodDestinationBeyondMaximumRange".Translate();
						}
						else
						{
							result = "TransportPodNotEnoughFuel".Translate();
						}
					}
					else
					{
						IEnumerable<FloatMenuOption> transportPodsFloatMenuOptionsAt = this.GetTransportPodsFloatMenuOptionsAt(target.Tile);
						if (!transportPodsFloatMenuOptionsAt.Any<FloatMenuOption>())
						{
							result = "";
						}
						else if (transportPodsFloatMenuOptionsAt.Count<FloatMenuOption>() == 1)
						{
							if (transportPodsFloatMenuOptionsAt.First<FloatMenuOption>().Disabled)
							{
								GUI.color = Color.red;
							}
							result = transportPodsFloatMenuOptionsAt.First<FloatMenuOption>().Label;
						}
						else
						{
							MapParent mapParent = target.WorldObject as MapParent;
							if (mapParent != null)
							{
								result = "ClickToSeeAvailableOrders_WorldObject".Translate(new object[]
								{
									mapParent.LabelCap
								});
							}
							else
							{
								result = "ClickToSeeAvailableOrders_Empty".Translate();
							}
						}
					}
				}
				return result;
			});
		}

		private bool ChoseWorldTarget(GlobalTargetInfo target)
		{
			bool result;
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				result = true;
			}
			else if (!target.IsValid)
			{
				Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
				result = false;
			}
			else
			{
				int num = Find.WorldGrid.TraversalDistanceBetween(this.parent.Map.Tile, target.Tile, true, int.MaxValue);
				if (num > this.MaxLaunchDistance)
				{
					Messages.Message("MessageTransportPodsDestinationIsTooFar".Translate(new object[]
					{
						CompLaunchable.FuelNeededToLaunchAtDist((float)num).ToString("0.#")
					}), MessageTypeDefOf.RejectInput, false);
					result = false;
				}
				else
				{
					IEnumerable<FloatMenuOption> transportPodsFloatMenuOptionsAt = this.GetTransportPodsFloatMenuOptionsAt(target.Tile);
					if (!transportPodsFloatMenuOptionsAt.Any<FloatMenuOption>())
					{
						if (Find.World.Impassable(target.Tile))
						{
							Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
							result = false;
						}
						else
						{
							this.TryLaunch(target.Tile, null);
							result = true;
						}
					}
					else if (transportPodsFloatMenuOptionsAt.Count<FloatMenuOption>() == 1)
					{
						if (!transportPodsFloatMenuOptionsAt.First<FloatMenuOption>().Disabled)
						{
							transportPodsFloatMenuOptionsAt.First<FloatMenuOption>().action();
						}
						result = false;
					}
					else
					{
						Find.WindowStack.Add(new FloatMenu(transportPodsFloatMenuOptionsAt.ToList<FloatMenuOption>()));
						result = false;
					}
				}
			}
			return result;
		}

		public void TryLaunch(int destinationTile, TransportPodsArrivalAction arrivalAction)
		{
			if (!this.parent.Spawned)
			{
				Log.Error("Tried to launch " + this.parent + ", but it's unspawned.", false);
			}
			else
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				if (transportersInGroup == null)
				{
					Log.Error("Tried to launch " + this.parent + ", but it's not in any group.", false);
				}
				else if (this.LoadingInProgressOrReadyToLaunch && this.AllInGroupConnectedToFuelingPort && this.AllFuelingPortSourcesInGroupHaveAnyFuel)
				{
					Map map = this.parent.Map;
					int num = Find.WorldGrid.TraversalDistanceBetween(map.Tile, destinationTile, true, int.MaxValue);
					if (num <= this.MaxLaunchDistance)
					{
						this.Transporter.TryRemoveLord(map);
						int groupID = this.Transporter.groupID;
						float amount = Mathf.Max(CompLaunchable.FuelNeededToLaunchAtDist((float)num), 1f);
						for (int i = 0; i < transportersInGroup.Count; i++)
						{
							CompTransporter compTransporter = transportersInGroup[i];
							Building fuelingPortSource = compTransporter.Launchable.FuelingPortSource;
							if (fuelingPortSource != null)
							{
								fuelingPortSource.TryGetComp<CompRefuelable>().ConsumeFuel(amount);
							}
							ThingOwner directlyHeldThings = compTransporter.GetDirectlyHeldThings();
							ActiveDropPod activeDropPod = (ActiveDropPod)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod, null);
							activeDropPod.Contents = new ActiveDropPodInfo();
							activeDropPod.Contents.innerContainer.TryAddRangeOrTransfer(directlyHeldThings, true, true);
							DropPodLeaving dropPodLeaving = (DropPodLeaving)SkyfallerMaker.MakeSkyfaller(ThingDefOf.DropPodLeaving, activeDropPod);
							dropPodLeaving.groupID = groupID;
							dropPodLeaving.destinationTile = destinationTile;
							dropPodLeaving.arrivalAction = arrivalAction;
							compTransporter.CleanUpLoadingVars(map);
							compTransporter.parent.Destroy(DestroyMode.Vanish);
							GenSpawn.Spawn(dropPodLeaving, compTransporter.parent.Position, map, WipeMode.Vanish);
						}
						CameraJumper.TryHideWorld();
					}
				}
			}
		}

		public void Notify_FuelingPortSourceDeSpawned()
		{
			if (this.Transporter.CancelLoad())
			{
				Messages.Message("MessageTransportersLoadCanceled_FuelingPortGiverDeSpawned".Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		public static int MaxLaunchDistanceAtFuelLevel(float fuelLevel)
		{
			return Mathf.FloorToInt(fuelLevel / 2.25f);
		}

		public static float FuelNeededToLaunchAtDist(float dist)
		{
			return 2.25f * dist;
		}

		public IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptionsAt(int tile)
		{
			bool anything = false;
			if (TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(this.TransportersInGroup.Cast<IThingHolder>(), tile) && !Find.WorldObjects.AnySettlementAt(tile) && !Find.WorldObjects.AnySiteAt(tile))
			{
				anything = true;
				yield return new FloatMenuOption("FormCaravanHere".Translate(), delegate()
				{
					this.TryLaunch(tile, new TransportPodsArrivalAction_FormCaravan());
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			List<WorldObject> worldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < worldObjects.Count; i++)
			{
				if (worldObjects[i].Tile == tile)
				{
					foreach (FloatMenuOption o in worldObjects[i].GetTransportPodsFloatMenuOptions(this.TransportersInGroup.Cast<IThingHolder>(), this))
					{
						anything = true;
						yield return o;
					}
				}
			}
			if (!anything && !Find.World.Impassable(tile))
			{
				yield return new FloatMenuOption("TransportPodsContentsWillBeLost".Translate(), delegate()
				{
					this.TryLaunch(tile, null);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static CompLaunchable()
		{
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

			internal Command_Action <launch>__2;

			internal CompLaunchable $this;

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
					goto IL_1CB;
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
				if (!base.LoadingInProgressOrReadyToLaunch)
				{
					goto IL_1CB;
				}
				launch = new Command_Action();
				launch.defaultLabel = "CommandLaunchGroup".Translate();
				launch.defaultDesc = "CommandLaunchGroupDesc".Translate();
				launch.icon = CompLaunchable.LaunchCommandTex;
				launch.alsoClickIfOtherInGroupClicked = false;
				launch.action = delegate()
				{
					if (base.AnyInGroupHasAnythingLeftToLoad)
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSendNotCompletelyLoadedPods".Translate(new object[]
						{
							base.FirstThingLeftToLoadInGroup.LabelCapNoCount
						}), new Action(base.StartChoosingDestination), false, null));
					}
					else
					{
						base.StartChoosingDestination();
					}
				};
				if (!base.AllInGroupConnectedToFuelingPort)
				{
					launch.Disable("CommandLaunchGroupFailNotConnectedToFuelingPort".Translate());
				}
				else if (!base.AllFuelingPortSourcesInGroupHaveAnyFuel)
				{
					launch.Disable("CommandLaunchGroupFailNoFuel".Translate());
				}
				else if (base.AnyInGroupIsUnderRoof)
				{
					launch.Disable("CommandLaunchGroupFailUnderRoof".Translate());
				}
				this.$current = launch;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_1CB:
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
				CompLaunchable.<CompGetGizmosExtra>c__Iterator0 <CompGetGizmosExtra>c__Iterator = new CompLaunchable.<CompGetGizmosExtra>c__Iterator0();
				<CompGetGizmosExtra>c__Iterator.$this = this;
				return <CompGetGizmosExtra>c__Iterator;
			}

			internal void <>m__0()
			{
				if (base.AnyInGroupHasAnythingLeftToLoad)
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSendNotCompletelyLoadedPods".Translate(new object[]
					{
						base.FirstThingLeftToLoadInGroup.LabelCapNoCount
					}), new Action(base.StartChoosingDestination), false, null));
				}
				else
				{
					base.StartChoosingDestination();
				}
			}
		}

		[CompilerGenerated]
		private sealed class <StartChoosingDestination>c__AnonStorey2
		{
			internal int tile;

			internal CompLaunchable $this;

			public <StartChoosingDestination>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				GenDraw.DrawWorldRadiusRing(this.tile, this.$this.MaxLaunchDistance);
			}

			internal string <>m__1(GlobalTargetInfo target)
			{
				string result;
				if (!target.IsValid)
				{
					result = null;
				}
				else
				{
					int num = Find.WorldGrid.TraversalDistanceBetween(this.tile, target.Tile, true, int.MaxValue);
					if (num > this.$this.MaxLaunchDistance)
					{
						GUI.color = Color.red;
						if (num > this.$this.MaxLaunchDistanceEverPossible)
						{
							result = "TransportPodDestinationBeyondMaximumRange".Translate();
						}
						else
						{
							result = "TransportPodNotEnoughFuel".Translate();
						}
					}
					else
					{
						IEnumerable<FloatMenuOption> transportPodsFloatMenuOptionsAt = this.$this.GetTransportPodsFloatMenuOptionsAt(target.Tile);
						if (!transportPodsFloatMenuOptionsAt.Any<FloatMenuOption>())
						{
							result = "";
						}
						else if (transportPodsFloatMenuOptionsAt.Count<FloatMenuOption>() == 1)
						{
							if (transportPodsFloatMenuOptionsAt.First<FloatMenuOption>().Disabled)
							{
								GUI.color = Color.red;
							}
							result = transportPodsFloatMenuOptionsAt.First<FloatMenuOption>().Label;
						}
						else
						{
							MapParent mapParent = target.WorldObject as MapParent;
							if (mapParent != null)
							{
								result = "ClickToSeeAvailableOrders_WorldObject".Translate(new object[]
								{
									mapParent.LabelCap
								});
							}
							else
							{
								result = "ClickToSeeAvailableOrders_Empty".Translate();
							}
						}
					}
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <GetTransportPodsFloatMenuOptionsAt>c__Iterator1 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal bool <anything>__0;

			internal int tile;

			internal List<WorldObject> <worldObjects>__0;

			internal int <i>__1;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <o>__2;

			internal CompLaunchable $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			private CompLaunchable.<GetTransportPodsFloatMenuOptionsAt>c__Iterator1.<GetTransportPodsFloatMenuOptionsAt>c__AnonStorey3 $locvar1;

			[DebuggerHidden]
			public <GetTransportPodsFloatMenuOptionsAt>c__Iterator1()
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
					anything = false;
					if (TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(base.TransportersInGroup.Cast<IThingHolder>(), tile) && !Find.WorldObjects.AnySettlementAt(tile) && !Find.WorldObjects.AnySiteAt(tile))
					{
						anything = true;
						this.$current = new FloatMenuOption("FormCaravanHere".Translate(), delegate()
						{
							this.TryLaunch(tile, new TransportPodsArrivalAction_FormCaravan());
						}, MenuOptionPriority.Default, null, null, 0f, null, null);
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
					Block_7:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							o = enumerator.Current;
							anything = true;
							this.$current = o;
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
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					goto IL_202;
				case 3u:
					IL_28F:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				worldObjects = Find.WorldObjects.AllWorldObjects;
				i = 0;
				goto IL_210;
				IL_202:
				i++;
				IL_210:
				if (i >= worldObjects.Count)
				{
					if (!anything && !Find.World.Impassable(<GetTransportPodsFloatMenuOptionsAt>c__AnonStorey.tile))
					{
						this.$current = new FloatMenuOption("TransportPodsContentsWillBeLost".Translate(), delegate()
						{
							<GetTransportPodsFloatMenuOptionsAt>c__AnonStorey.<>f__ref$1.$this.TryLaunch(<GetTransportPodsFloatMenuOptionsAt>c__AnonStorey.tile, null);
						}, MenuOptionPriority.Default, null, null, 0f, null, null);
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
					goto IL_28F;
				}
				else
				{
					if (worldObjects[i].Tile != <GetTransportPodsFloatMenuOptionsAt>c__AnonStorey.tile)
					{
						goto IL_202;
					}
					enumerator = worldObjects[i].GetTransportPodsFloatMenuOptions(base.TransportersInGroup.Cast<IThingHolder>(), this).GetEnumerator();
					num = 4294967293u;
					goto Block_7;
				}
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
				case 2u:
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
				return this.System.Collections.Generic.IEnumerable<Verse.FloatMenuOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				CompLaunchable.<GetTransportPodsFloatMenuOptionsAt>c__Iterator1 <GetTransportPodsFloatMenuOptionsAt>c__Iterator = new CompLaunchable.<GetTransportPodsFloatMenuOptionsAt>c__Iterator1();
				<GetTransportPodsFloatMenuOptionsAt>c__Iterator.$this = this;
				<GetTransportPodsFloatMenuOptionsAt>c__Iterator.tile = tile;
				return <GetTransportPodsFloatMenuOptionsAt>c__Iterator;
			}

			private sealed class <GetTransportPodsFloatMenuOptionsAt>c__AnonStorey3
			{
				internal int tile;

				internal CompLaunchable.<GetTransportPodsFloatMenuOptionsAt>c__Iterator1 <>f__ref$1;

				public <GetTransportPodsFloatMenuOptionsAt>c__AnonStorey3()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$1.$this.TryLaunch(this.tile, new TransportPodsArrivalAction_FormCaravan());
				}

				internal void <>m__1()
				{
					this.<>f__ref$1.$this.TryLaunch(this.tile, null);
				}
			}
		}
	}
}
