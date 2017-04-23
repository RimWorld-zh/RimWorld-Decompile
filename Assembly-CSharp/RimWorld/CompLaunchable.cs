using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class CompLaunchable : ThingComp
	{
		private const float FuelPerTile = 2.25f;

		private CompTransporter cachedCompTransporter;

		private static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment", true);

		private static readonly Texture2D LaunchCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);

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
				if (!this.ConnectedToFuelingPort)
				{
					return 0f;
				}
				return this.FuelingPortSource.GetComp<CompRefuelable>().Fuel;
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
				if (!flag)
				{
					return 0f;
				}
				return num;
			}
		}

		private int MaxLaunchDistance
		{
			get
			{
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					return 0;
				}
				return CompLaunchable.MaxLaunchDistanceAtFuelLevel(this.FuelInLeastFueledFuelingPortSource);
			}
		}

		private int MaxLaunchDistanceEverPossible
		{
			get
			{
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					return 0;
				}
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
				return CompLaunchable.MaxLaunchDistanceAtFuelLevel(num);
			}
		}

		[DebuggerHidden]
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompLaunchable.<CompGetGizmosExtra>c__Iterator164 <CompGetGizmosExtra>c__Iterator = new CompLaunchable.<CompGetGizmosExtra>c__Iterator164();
			<CompGetGizmosExtra>c__Iterator.<>f__this = this;
			CompLaunchable.<CompGetGizmosExtra>c__Iterator164 expr_0E = <CompGetGizmosExtra>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override string CompInspectStringExtra()
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return null;
			}
			if (!this.AllInGroupConnectedToFuelingPort)
			{
				return "NotReadyForLaunch".Translate() + ": " + "NotAllInGroupConnectedToFuelingPort".Translate() + ".";
			}
			if (!this.AllFuelingPortSourcesInGroupHaveAnyFuel)
			{
				return "NotReadyForLaunch".Translate() + ": " + "NotAllFuelingPortSourcesInGroupHaveAnyFuel".Translate() + ".";
			}
			if (this.AnyInGroupHasAnythingLeftToLoad)
			{
				return "NotReadyForLaunch".Translate() + ": " + "TransportPodInGroupHasSomethingLeftToLoad".Translate() + ".";
			}
			return "ReadyForLaunch".Translate();
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
				if (!target.IsValid)
				{
					return null;
				}
				int num = Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile);
				if (num <= this.MaxLaunchDistance)
				{
					return null;
				}
				if (num > this.MaxLaunchDistanceEverPossible)
				{
					return "TransportPodDestinationBeyondMaximumRange".Translate();
				}
				return "TransportPodNotEnoughFuel".Translate();
			});
		}

		private bool ChoseWorldTarget(GlobalTargetInfo target)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return true;
			}
			if (!target.IsValid)
			{
				Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageSound.RejectInput);
				return false;
			}
			int num = Find.WorldGrid.TraversalDistanceBetween(this.parent.Map.Tile, target.Tile);
			if (num > this.MaxLaunchDistance)
			{
				Messages.Message("MessageTransportPodsDestinationIsTooFar".Translate(new object[]
				{
					CompLaunchable.FuelNeededToLaunchAtDist((float)num).ToString("0.#")
				}), MessageSound.RejectInput);
				return false;
			}
			MapParent mapParent = target.WorldObject as MapParent;
			if (mapParent != null && mapParent.HasMap)
			{
				Map myMap = this.parent.Map;
				Map map = mapParent.Map;
				Current.Game.VisibleMap = map;
				Targeter arg_13B_0 = Find.Targeter;
				Action actionWhenFinished = delegate
				{
					if (Find.Maps.Contains(myMap))
					{
						Current.Game.VisibleMap = myMap;
					}
				};
				arg_13B_0.BeginTargeting(TargetingParameters.ForDropPodsDestination(), delegate(LocalTargetInfo x)
				{
					if (!this.LoadingInProgressOrReadyToLaunch)
					{
						return;
					}
					this.TryLaunch(x.ToGlobalTargetInfo(map), PawnsArriveMode.Undecided, false);
				}, null, actionWhenFinished, CompLaunchable.TargeterMouseAttachment);
				return true;
			}
			bool flag;
			if (target.WorldObject is Settlement)
			{
				Settlement settlement = (Settlement)target.WorldObject;
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				if (settlement.Visitable)
				{
					list.Add(new FloatMenuOption("VisitSettlement".Translate(new object[]
					{
						target.WorldObject.Label
					}), delegate
					{
						if (!this.LoadingInProgressOrReadyToLaunch)
						{
							return;
						}
						this.TryLaunch(target, PawnsArriveMode.Undecided, false);
						CameraJumper.TryHideWorld();
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				if (settlement.Attackable)
				{
					list.Add(new FloatMenuOption("DropAtEdge".Translate(), delegate
					{
						if (!this.LoadingInProgressOrReadyToLaunch)
						{
							return;
						}
						this.TryLaunch(target, PawnsArriveMode.EdgeDrop, true);
						CameraJumper.TryHideWorld();
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
					list.Add(new FloatMenuOption("DropInCenter".Translate(), delegate
					{
						if (!this.LoadingInProgressOrReadyToLaunch)
						{
							return;
						}
						this.TryLaunch(target, PawnsArriveMode.CenterDrop, true);
						CameraJumper.TryHideWorld();
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				if (list.Any<FloatMenuOption>())
				{
					Find.WorldTargeter.closeWorldTabWhenFinished = false;
					Find.WindowStack.Add(new FloatMenu(list));
					return true;
				}
				flag = true;
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
			if (Find.World.Impassable(target.Tile))
			{
				Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageSound.RejectInput);
				return false;
			}
			this.TryLaunch(target, PawnsArriveMode.Undecided, false);
			return true;
		}

		private void TryLaunch(GlobalTargetInfo target, PawnsArriveMode arriveMode, bool attackOnArrival)
		{
			if (!this.parent.Spawned)
			{
				Log.Error("Tried to launch " + this.parent + ", but it's unspawned.");
				return;
			}
			List<CompTransporter> transportersInGroup = this.TransportersInGroup;
			if (transportersInGroup == null)
			{
				Log.Error("Tried to launch " + this.parent + ", but it's not in any group.");
				return;
			}
			if (!this.LoadingInProgressOrReadyToLaunch || !this.AllInGroupConnectedToFuelingPort || !this.AllFuelingPortSourcesInGroupHaveAnyFuel)
			{
				return;
			}
			Map map = this.parent.Map;
			int num = Find.WorldGrid.TraversalDistanceBetween(map.Tile, target.Tile);
			if (num > this.MaxLaunchDistance)
			{
				return;
			}
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
				DropPodLeaving dropPodLeaving = (DropPodLeaving)ThingMaker.MakeThing(ThingDefOf.DropPodLeaving, null);
				dropPodLeaving.groupID = groupID;
				dropPodLeaving.destinationTile = target.Tile;
				dropPodLeaving.destinationCell = target.Cell;
				dropPodLeaving.arriveMode = arriveMode;
				dropPodLeaving.attackOnArrival = attackOnArrival;
				ThingOwner directlyHeldThings = compTransporter.GetDirectlyHeldThings();
				dropPodLeaving.Contents = new ActiveDropPodInfo();
				dropPodLeaving.Contents.innerContainer.TryAddRange(directlyHeldThings);
				directlyHeldThings.Clear();
				compTransporter.CleanUpLoadingVars(map);
				compTransporter.parent.Destroy(DestroyMode.Vanish);
				GenSpawn.Spawn(dropPodLeaving, compTransporter.parent.Position, map);
			}
		}

		public void Notify_FuelingPortSourceDeSpawned()
		{
			if (this.Transporter.CancelLoad())
			{
				Messages.Message("MessageTransportersLoadCanceled_FuelingPortGiverDeSpawned".Translate(), this.parent, MessageSound.Negative);
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
	}
}
