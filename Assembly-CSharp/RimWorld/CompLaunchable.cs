using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000718 RID: 1816
	[StaticConstructorOnStartup]
	public class CompLaunchable : ThingComp
	{
		// Token: 0x040015E8 RID: 5608
		private CompTransporter cachedCompTransporter;

		// Token: 0x040015E9 RID: 5609
		public static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment", true);

		// Token: 0x040015EA RID: 5610
		private static readonly Texture2D LaunchCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);

		// Token: 0x040015EB RID: 5611
		private const float FuelPerTile = 2.25f;

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x060027F3 RID: 10227 RVA: 0x00155A48 File Offset: 0x00153E48
		public Building FuelingPortSource
		{
			get
			{
				return FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(this.parent.Position, this.parent.Map);
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x060027F4 RID: 10228 RVA: 0x00155A78 File Offset: 0x00153E78
		public bool ConnectedToFuelingPort
		{
			get
			{
				return this.FuelingPortSource != null;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x060027F5 RID: 10229 RVA: 0x00155A9C File Offset: 0x00153E9C
		public bool FuelingPortSourceHasAnyFuel
		{
			get
			{
				return this.ConnectedToFuelingPort && this.FuelingPortSource.GetComp<CompRefuelable>().HasFuel;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x060027F6 RID: 10230 RVA: 0x00155AD0 File Offset: 0x00153ED0
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.Transporter.LoadingInProgressOrReadyToLaunch;
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x060027F7 RID: 10231 RVA: 0x00155AF0 File Offset: 0x00153EF0
		public bool AnythingLeftToLoad
		{
			get
			{
				return this.Transporter.AnythingLeftToLoad;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x00155B10 File Offset: 0x00153F10
		public Thing FirstThingLeftToLoad
		{
			get
			{
				return this.Transporter.FirstThingLeftToLoad;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x060027F9 RID: 10233 RVA: 0x00155B30 File Offset: 0x00153F30
		public List<CompTransporter> TransportersInGroup
		{
			get
			{
				return this.Transporter.TransportersInGroup(this.parent.Map);
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x060027FA RID: 10234 RVA: 0x00155B5C File Offset: 0x00153F5C
		public bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.Transporter.AnyInGroupHasAnythingLeftToLoad;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x060027FB RID: 10235 RVA: 0x00155B7C File Offset: 0x00153F7C
		public Thing FirstThingLeftToLoadInGroup
		{
			get
			{
				return this.Transporter.FirstThingLeftToLoadInGroup;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x060027FC RID: 10236 RVA: 0x00155B9C File Offset: 0x00153F9C
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

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x060027FD RID: 10237 RVA: 0x00155C00 File Offset: 0x00154000
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

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x060027FE RID: 10238 RVA: 0x00155C38 File Offset: 0x00154038
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

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x060027FF RID: 10239 RVA: 0x00155C74 File Offset: 0x00154074
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

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06002800 RID: 10240 RVA: 0x00155CC8 File Offset: 0x001540C8
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

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06002801 RID: 10241 RVA: 0x00155D1C File Offset: 0x0015411C
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

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06002802 RID: 10242 RVA: 0x00155D98 File Offset: 0x00154198
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

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06002803 RID: 10243 RVA: 0x00155DCC File Offset: 0x001541CC
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

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06002804 RID: 10244 RVA: 0x00155E54 File Offset: 0x00154254
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

		// Token: 0x06002805 RID: 10245 RVA: 0x00155EE0 File Offset: 0x001542E0
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

		// Token: 0x06002806 RID: 10246 RVA: 0x00155F0C File Offset: 0x0015430C
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

		// Token: 0x06002807 RID: 10247 RVA: 0x00155FDC File Offset: 0x001543DC
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

		// Token: 0x06002808 RID: 10248 RVA: 0x00156064 File Offset: 0x00154464
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

		// Token: 0x06002809 RID: 10249 RVA: 0x001561D0 File Offset: 0x001545D0
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

		// Token: 0x0600280A RID: 10250 RVA: 0x001563A6 File Offset: 0x001547A6
		public void Notify_FuelingPortSourceDeSpawned()
		{
			if (this.Transporter.CancelLoad())
			{
				Messages.Message("MessageTransportersLoadCanceled_FuelingPortGiverDeSpawned".Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x001563DC File Offset: 0x001547DC
		public static int MaxLaunchDistanceAtFuelLevel(float fuelLevel)
		{
			return Mathf.FloorToInt(fuelLevel / 2.25f);
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x00156400 File Offset: 0x00154800
		public static float FuelNeededToLaunchAtDist(float dist)
		{
			return 2.25f * dist;
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x0015641C File Offset: 0x0015481C
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
	}
}
