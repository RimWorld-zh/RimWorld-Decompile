using RimWorld.Planet;
using System;
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
				int num = 0;
				bool result;
				while (true)
				{
					if (num < transportersInGroup.Count)
					{
						if (transportersInGroup[num].parent.Position.Roofed(base.parent.Map))
						{
							result = true;
							break;
						}
						num++;
						continue;
					}
					result = false;
					break;
				}
				return result;
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
				return (float)(this.ConnectedToFuelingPort ? this.FuelingPortSource.GetComp<CompRefuelable>().Fuel : 0.0);
			}
		}

		public bool AllInGroupConnectedToFuelingPort
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				int num = 0;
				bool result;
				while (true)
				{
					if (num < transportersInGroup.Count)
					{
						if (!transportersInGroup[num].Launchable.ConnectedToFuelingPort)
						{
							result = false;
							break;
						}
						num++;
						continue;
					}
					result = true;
					break;
				}
				return result;
			}
		}

		public bool AllFuelingPortSourcesInGroupHaveAnyFuel
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				int num = 0;
				bool result;
				while (true)
				{
					if (num < transportersInGroup.Count)
					{
						if (!transportersInGroup[num].Launchable.FuelingPortSourceHasAnyFuel)
						{
							result = false;
							break;
						}
						num++;
						continue;
					}
					result = true;
					break;
				}
				return result;
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
				return (float)(flag ? num : 0.0);
			}
		}

		private int MaxLaunchDistance
		{
			get
			{
				return this.LoadingInProgressOrReadyToLaunch ? CompLaunchable.MaxLaunchDistanceAtFuelLevel(this.FuelInLeastFueledFuelingPortSource) : 0;
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
			if (!this.LoadingInProgressOrReadyToLaunch)
				yield break;
			Command_Action launch = new Command_Action
			{
				defaultLabel = "CommandLaunchGroup".Translate(),
				defaultDesc = "CommandLaunchGroupDesc".Translate(),
				icon = CompLaunchable.LaunchCommandTex,
				action = (Action)delegate
				{
					if (((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_0114: stateMachine*/)._0024this.AnyInGroupHasAnythingLeftToLoad)
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSendNotCompletelyLoadedPods".Translate(((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_0114: stateMachine*/)._0024this.FirstThingLeftToLoadInGroup.LabelCapNoCount), new Action(((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_0114: stateMachine*/)._0024this.StartChoosingDestination), false, (string)null));
					}
					else
					{
						((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_0114: stateMachine*/)._0024this.StartChoosingDestination();
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
			IL_01c8:
			/*Error near IL_01c9: Unexpected return in MoveNext()*/;
		}

		public override string CompInspectStringExtra()
		{
			return (!this.LoadingInProgressOrReadyToLaunch) ? null : (this.AllInGroupConnectedToFuelingPort ? (this.AllFuelingPortSourcesInGroupHaveAnyFuel ? ((!this.AnyInGroupHasAnythingLeftToLoad) ? "ReadyForLaunch".Translate() : ("NotReadyForLaunch".Translate() + ": " + "TransportPodInGroupHasSomethingLeftToLoad".Translate() + ".")) : ("NotReadyForLaunch".Translate() + ": " + "NotAllFuelingPortSourcesInGroupHaveAnyFuel".Translate() + ".")) : ("NotReadyForLaunch".Translate() + ": " + "NotAllInGroupConnectedToFuelingPort".Translate() + "."));
		}

		private void StartChoosingDestination()
		{
			CameraJumper.TryJump(CameraJumper.GetWorldTarget((Thing)base.parent));
			Find.WorldSelector.ClearSelection();
			int tile = base.parent.Map.Tile;
			Find.WorldTargeter.BeginTargeting(new Func<GlobalTargetInfo, bool>(this.ChoseWorldTarget), true, CompLaunchable.TargeterMouseAttachment, true, (Action)delegate
			{
				GenDraw.DrawWorldRadiusRing(tile, this.MaxLaunchDistance);
			}, (Func<GlobalTargetInfo, string>)delegate(GlobalTargetInfo target)
			{
				string result;
				if (!target.IsValid)
				{
					result = (string)null;
				}
				else
				{
					int num = Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile);
					result = ((num <= this.MaxLaunchDistance) ? null : ((num <= this.MaxLaunchDistanceEverPossible) ? "TransportPodNotEnoughFuel".Translate() : "TransportPodDestinationBeyondMaximumRange".Translate()));
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
				Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput);
				result = false;
			}
			else
			{
				int num = Find.WorldGrid.TraversalDistanceBetween(base.parent.Map.Tile, target.Tile);
				if (num > this.MaxLaunchDistance)
				{
					float num2 = CompLaunchable.FuelNeededToLaunchAtDist((float)num);
					Messages.Message("MessageTransportPodsDestinationIsTooFar".Translate(num2.ToString("0.#")), MessageTypeDefOf.RejectInput);
					result = false;
				}
				else
				{
					MapParent mapParent = target.WorldObject as MapParent;
					bool flag = false;
					if (mapParent != null && mapParent.HasMap)
					{
						Map myMap = base.parent.Map;
						Map map = mapParent.Map;
						Current.Game.VisibleMap = map;
						Find.Targeter.BeginTargeting(TargetingParameters.ForDropPodsDestination(), (Action<LocalTargetInfo>)delegate(LocalTargetInfo x)
						{
							if (this.LoadingInProgressOrReadyToLaunch)
							{
								this.TryLaunch(x.ToGlobalTargetInfo(map), PawnsArriveMode.Undecided, false);
							}
						}, null, (Action)delegate
						{
							if (Find.Maps.Contains(myMap))
							{
								Current.Game.VisibleMap = myMap;
							}
						}, CompLaunchable.TargeterMouseAttachment);
						result = true;
					}
					else
					{
						if (mapParent != null)
						{
							Settlement settlement = mapParent as Settlement;
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							if (settlement != null && settlement.Visitable)
							{
								list.Add(new FloatMenuOption("VisitSettlement".Translate(target.WorldObject.Label), (Action)delegate()
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
								list.Add(new FloatMenuOption("DropAtEdge".Translate(), (Action)delegate()
								{
									if (this.LoadingInProgressOrReadyToLaunch)
									{
										this.TryLaunch(target, PawnsArriveMode.EdgeDrop, true);
										CameraJumper.TryHideWorld();
									}
								}, MenuOptionPriority.Default, null, null, 0f, null, null));
								list.Add(new FloatMenuOption("DropInCenter".Translate(), (Action)delegate()
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
								result = true;
								goto IL_02cd;
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
								result = false;
							}
							else
							{
								this.TryLaunch(target, PawnsArriveMode.Undecided, false);
								result = true;
							}
						}
						else
						{
							result = false;
						}
					}
				}
			}
			goto IL_02cd;
			IL_02cd:
			return result;
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
				Messages.Message("MessageTransportersLoadCanceled_FuelingPortGiverDeSpawned".Translate(), (Thing)base.parent, MessageTypeDefOf.NegativeEvent);
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
