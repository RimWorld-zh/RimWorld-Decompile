using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class CompLaunchable : ThingComp
	{
		private CompTransporter cachedCompTransporter;

		private static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment", true);

		private static readonly Texture2D LaunchCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);

		private const float FuelPerTile = 2.25f;

		public Building FuelingPortSource
		{
			get
			{
				return FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(base.parent.Position, base.parent.Map);
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
				return this.Transporter.TransportersInGroup(base.parent.Map);
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
					if (transportersInGroup[i].parent.Position.Roofed(base.parent.Map))
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
					this.cachedCompTransporter = base.parent.GetComp<CompTransporter>();
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

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			using (IEnumerator<Gizmo> enumerator = base.CompGetGizmosExtra().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!this.LoadingInProgressOrReadyToLaunch)
				yield break;
			Command_Action launch = new Command_Action
			{
				defaultLabel = "CommandLaunchGroup".Translate(),
				defaultDesc = "CommandLaunchGroupDesc".Translate(),
				icon = CompLaunchable.LaunchCommandTex,
				alsoClickIfOtherInGroupClicked = false,
				action = delegate
				{
					if (((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_011b: stateMachine*/)._0024this.AnyInGroupHasAnythingLeftToLoad)
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSendNotCompletelyLoadedPods".Translate(((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_011b: stateMachine*/)._0024this.FirstThingLeftToLoadInGroup.LabelCapNoCount), ((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_011b: stateMachine*/)._0024this.StartChoosingDestination, false, null));
					}
					else
					{
						((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_011b: stateMachine*/)._0024this.StartChoosingDestination();
					}
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
			yield return (Gizmo)launch;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_01ce:
			/*Error near IL_01cf: Unexpected return in MoveNext()*/;
		}

		public override string CompInspectStringExtra()
		{
			if (this.LoadingInProgressOrReadyToLaunch)
			{
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
			return null;
		}

		private void StartChoosingDestination()
		{
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(base.parent));
			Find.WorldSelector.ClearSelection();
			int tile = base.parent.Map.Tile;
			Find.WorldTargeter.BeginTargeting(this.ChoseWorldTarget, true, CompLaunchable.TargeterMouseAttachment, true, delegate
			{
				GenDraw.DrawWorldRadiusRing(tile, this.MaxLaunchDistance);
			}, delegate(GlobalTargetInfo target)
			{
				if (!target.IsValid)
				{
					return null;
				}
				int num = Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile);
				if (num > this.MaxLaunchDistance)
				{
					if (num > this.MaxLaunchDistanceEverPossible)
					{
						return "TransportPodDestinationBeyondMaximumRange".Translate();
					}
					return "TransportPodNotEnoughFuel".Translate();
				}
				return null;
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
				Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput);
				return false;
			}
			int num = Find.WorldGrid.TraversalDistanceBetween(base.parent.Map.Tile, target.Tile);
			if (num > this.MaxLaunchDistance)
			{
				float num2 = CompLaunchable.FuelNeededToLaunchAtDist((float)num);
				Messages.Message("MessageTransportPodsDestinationIsTooFar".Translate(num2.ToString("0.#")), MessageTypeDefOf.RejectInput);
				return false;
			}
			MapParent mapParent = target.WorldObject as MapParent;
			bool flag = false;
			if (mapParent != null && mapParent.HasMap)
			{
				Map myMap = base.parent.Map;
				Map map = mapParent.Map;
				Current.Game.VisibleMap = map;
				Find.Targeter.BeginTargeting(TargetingParameters.ForDropPodsDestination(), delegate(LocalTargetInfo x)
				{
					if (this.LoadingInProgressOrReadyToLaunch)
					{
						this.TryLaunch(x.ToGlobalTargetInfo(map), PawnsArriveMode.Undecided, false);
					}
				}, null, delegate
				{
					if (Find.Maps.Contains(myMap))
					{
						Current.Game.VisibleMap = myMap;
					}
				}, CompLaunchable.TargeterMouseAttachment);
				return true;
			}
			if (mapParent != null)
			{
				Settlement settlement = mapParent as Settlement;
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				if (settlement != null && settlement.Visitable)
				{
					list.Add(new FloatMenuOption("VisitSettlement".Translate(target.WorldObject.Label), delegate
					{
						if (this.LoadingInProgressOrReadyToLaunch)
						{
							this.TryLaunch(target, PawnsArriveMode.Undecided, false);
							CameraJumper.TryHideWorld();
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				if (mapParent.TransportPodsCanLandAndGenerateMap)
				{
					list.Add(new FloatMenuOption("DropAtEdge".Translate(), delegate
					{
						if (this.LoadingInProgressOrReadyToLaunch)
						{
							this.TryLaunch(target, PawnsArriveMode.EdgeDrop, true);
							CameraJumper.TryHideWorld();
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
					list.Add(new FloatMenuOption("DropInCenter".Translate(), delegate
					{
						if (this.LoadingInProgressOrReadyToLaunch)
						{
							this.TryLaunch(target, PawnsArriveMode.CenterDrop, true);
							CameraJumper.TryHideWorld();
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				if (list.Any())
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
			if (flag)
			{
				if (Find.World.Impassable(target.Tile))
				{
					Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput);
					return false;
				}
				this.TryLaunch(target, PawnsArriveMode.Undecided, false);
				return true;
			}
			return false;
		}

		private void TryLaunch(GlobalTargetInfo target, PawnsArriveMode arriveMode, bool attackOnArrival)
		{
			if (!base.parent.Spawned)
			{
				Log.Error("Tried to launch " + base.parent + ", but it's unspawned.");
			}
			else
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				if (transportersInGroup == null)
				{
					Log.Error("Tried to launch " + base.parent + ", but it's not in any group.");
				}
				else if (this.LoadingInProgressOrReadyToLaunch && this.AllInGroupConnectedToFuelingPort && this.AllFuelingPortSourcesInGroupHaveAnyFuel)
				{
					Map map = base.parent.Map;
					int num = Find.WorldGrid.TraversalDistanceBetween(map.Tile, target.Tile);
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
							dropPodLeaving.destinationTile = target.Tile;
							dropPodLeaving.destinationCell = target.Cell;
							dropPodLeaving.arriveMode = arriveMode;
							dropPodLeaving.attackOnArrival = attackOnArrival;
							compTransporter.CleanUpLoadingVars(map);
							compTransporter.parent.Destroy(DestroyMode.Vanish);
							GenSpawn.Spawn(dropPodLeaving, compTransporter.parent.Position, map);
						}
					}
				}
			}
		}

		public void Notify_FuelingPortSourceDeSpawned()
		{
			if (this.Transporter.CancelLoad())
			{
				Messages.Message("MessageTransportersLoadCanceled_FuelingPortGiverDeSpawned".Translate(), base.parent, MessageTypeDefOf.NegativeEvent);
			}
		}

		public static int MaxLaunchDistanceAtFuelLevel(float fuelLevel)
		{
			return Mathf.FloorToInt((float)(fuelLevel / 2.25));
		}

		public static float FuelNeededToLaunchAtDist(float dist)
		{
			return (float)(2.25 * dist);
		}
	}
}
