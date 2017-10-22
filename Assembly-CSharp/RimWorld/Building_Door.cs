using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	public class Building_Door : Building
	{
		public CompPowerTrader powerComp;

		private bool openInt = false;

		private bool holdOpenInt = false;

		private int lastFriendlyTouchTick = -9999;

		protected int ticksUntilClose = 0;

		protected int visualTicksOpen = 0;

		private bool freePassageWhenClearedReachabilityCache;

		private const float BaseDoorOpenTicks = 45f;

		private const int AutomaticCloseDelayTicks = 60;

		private const int ApproachCloseDelayTicks = 300;

		private const int MaxTicksSinceFriendlyTouchToAutoClose = 301;

		private const float VisualDoorOffsetStart = 0f;

		private const float VisualDoorOffsetEnd = 0.45f;

		public bool Open
		{
			get
			{
				return this.openInt;
			}
		}

		public bool HoldOpen
		{
			get
			{
				return this.holdOpenInt;
			}
		}

		public bool FreePassage
		{
			get
			{
				return this.openInt && (this.holdOpenInt || !this.WillCloseSoon);
			}
		}

		public bool WillCloseSoon
		{
			get
			{
				bool result;
				if (!base.Spawned)
				{
					result = true;
				}
				else if (!this.openInt)
				{
					result = true;
				}
				else if (this.holdOpenInt)
				{
					result = false;
				}
				else if (this.ticksUntilClose > 0 && this.ticksUntilClose <= 60 && this.CanCloseAutomatically)
				{
					result = true;
				}
				else
				{
					for (int i = 0; i < 5; i++)
					{
						IntVec3 c = base.Position + GenAdj.CardinalDirectionsAndInside[i];
						if (c.InBounds(base.Map))
						{
							List<Thing> thingList = c.GetThingList(base.Map);
							for (int j = 0; j < thingList.Count; j++)
							{
								Pawn pawn = thingList[j] as Pawn;
								if (pawn != null && !pawn.HostileTo(this) && !pawn.Downed && (pawn.Position == base.Position || (pawn.pather.MovingNow && pawn.pather.nextCell == base.Position)))
								{
									goto IL_012a;
								}
							}
						}
					}
					result = false;
				}
				goto IL_0158;
				IL_0158:
				return result;
				IL_012a:
				result = true;
				goto IL_0158;
			}
		}

		public bool BlockedOpenMomentary
		{
			get
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				int num = 0;
				bool result;
				while (true)
				{
					if (num < thingList.Count)
					{
						Thing thing = thingList[num];
						if (thing.def.category != ThingCategory.Item && thing.def.category != ThingCategory.Pawn)
						{
							num++;
							continue;
						}
						result = true;
					}
					else
					{
						result = false;
					}
					break;
				}
				return result;
			}
		}

		public bool DoorPowerOn
		{
			get
			{
				return this.powerComp != null && this.powerComp.PowerOn;
			}
		}

		public bool SlowsPawns
		{
			get
			{
				return !this.DoorPowerOn || this.TicksToOpenNow > 20;
			}
		}

		public int TicksToOpenNow
		{
			get
			{
				float num = (float)(45.0 / this.GetStatValue(StatDefOf.DoorOpenSpeed, true));
				if (this.DoorPowerOn)
				{
					num = (float)(num * 0.25);
				}
				return Mathf.RoundToInt(num);
			}
		}

		private bool CanCloseAutomatically
		{
			get
			{
				return this.DoorPowerOn && this.FriendlyTouchedRecently;
			}
		}

		private bool FriendlyTouchedRecently
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastFriendlyTouchTick + 301;
			}
		}

		private int VisualTicksToOpen
		{
			get
			{
				return this.TicksToOpenNow;
			}
		}

		public override bool FireBulwark
		{
			get
			{
				return !this.Open && base.FireBulwark;
			}
		}

		public override void PostMake()
		{
			base.PostMake();
			this.powerComp = base.GetComp<CompPowerTrader>();
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.ClearReachabilityCache(map);
			if (this.BlockedOpenMomentary)
			{
				this.DoorOpen(60);
			}
		}

		public override void DeSpawn()
		{
			Map map = base.Map;
			base.DeSpawn();
			this.ClearReachabilityCache(map);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.openInt, "open", false, false);
			Scribe_Values.Look<bool>(ref this.holdOpenInt, "holdOpen", false, false);
			Scribe_Values.Look<int>(ref this.lastFriendlyTouchTick, "lastFriendlyTouchTick", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.openInt)
			{
				this.visualTicksOpen = this.VisualTicksToOpen;
			}
		}

		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			base.SetFaction(newFaction, recruiter);
			if (base.Spawned)
			{
				this.ClearReachabilityCache(base.Map);
			}
		}

		public override void Tick()
		{
			base.Tick();
			if (this.FreePassage != this.freePassageWhenClearedReachabilityCache)
			{
				this.ClearReachabilityCache(base.Map);
			}
			if (!this.openInt)
			{
				if (this.visualTicksOpen > 0)
				{
					this.visualTicksOpen--;
				}
				if ((Find.TickManager.TicksGame + base.thingIDNumber.HashOffset()) % 375 == 0)
				{
					GenTemperature.EqualizeTemperaturesThroughBuilding(this, 1f, false);
				}
			}
			else if (this.openInt)
			{
				if (this.visualTicksOpen < this.VisualTicksToOpen)
				{
					this.visualTicksOpen++;
				}
				if (!this.holdOpenInt)
				{
					if (base.Map.thingGrid.CellContains(base.Position, ThingCategory.Pawn))
					{
						this.ticksUntilClose = 60;
					}
					else
					{
						this.ticksUntilClose--;
						if (this.ticksUntilClose <= 0 && this.CanCloseAutomatically)
						{
							this.DoorTryClose();
						}
					}
				}
				if ((Find.TickManager.TicksGame + base.thingIDNumber.HashOffset()) % 22 == 0)
				{
					GenTemperature.EqualizeTemperaturesThroughBuilding(this, 1f, false);
				}
			}
		}

		public void FriendlyTouched()
		{
			this.lastFriendlyTouchTick = Find.TickManager.TicksGame;
		}

		public void Notify_PawnApproaching(Pawn p)
		{
			if (!p.HostileTo(this))
			{
				this.FriendlyTouched();
			}
			if (this.PawnCanOpen(p))
			{
				base.Map.fogGrid.Notify_PawnEnteringDoor(this, p);
				if (!this.SlowsPawns)
				{
					this.DoorOpen(300);
				}
			}
		}

		public bool CanPhysicallyPass(Pawn p)
		{
			return this.FreePassage || this.PawnCanOpen(p);
		}

		public virtual bool PawnCanOpen(Pawn p)
		{
			Lord lord = p.GetLord();
			return (lord != null && lord.LordJob != null && lord.LordJob.CanOpenAnyDoor(p)) || (p.IsWildMan() && !p.mindState.wildManEverReachedOutside) || base.Faction == null || (p.guest != null && p.guest.Released) || GenAI.MachinesLike(base.Faction, p);
		}

		public override bool BlocksPawn(Pawn p)
		{
			return !this.openInt && !this.PawnCanOpen(p);
		}

		protected void DoorOpen(int ticksToClose = 60)
		{
			this.ticksUntilClose = ticksToClose;
			if (!this.openInt)
			{
				this.openInt = true;
				if (this.DoorPowerOn)
				{
					base.def.building.soundDoorOpenPowered.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				else
				{
					base.def.building.soundDoorOpenManual.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
			}
		}

		protected void DoorTryClose()
		{
			if (!this.holdOpenInt && !this.BlockedOpenMomentary)
			{
				this.openInt = false;
				if (this.DoorPowerOn)
				{
					base.def.building.soundDoorClosePowered.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				else
				{
					base.def.building.soundDoorCloseManual.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
			}
		}

		public void StartManualOpenBy(Pawn opener)
		{
			this.DoorOpen(60);
		}

		public void StartManualCloseBy(Pawn closer)
		{
			this.DoorTryClose();
		}

		public override void Draw()
		{
			base.Rotation = Building_Door.DoorRotationAt(base.Position, base.Map);
			float num = Mathf.Clamp01((float)this.visualTicksOpen / (float)this.VisualTicksToOpen);
			float d = (float)(0.44999998807907104 * num);
			for (int i = 0; i < 2; i++)
			{
				Vector3 vector = default(Vector3);
				Mesh mesh;
				if (i == 0)
				{
					vector = new Vector3(0f, 0f, -1f);
					mesh = MeshPool.plane10;
				}
				else
				{
					vector = new Vector3(0f, 0f, 1f);
					mesh = MeshPool.plane10Flip;
				}
				Rot4 rotation = base.Rotation;
				rotation.Rotate(RotationDirection.Clockwise);
				vector = rotation.AsQuat * vector;
				Vector3 vector2 = this.DrawPos;
				vector2.y = Altitudes.AltitudeFor(AltitudeLayer.DoorMoveable);
				vector2 += vector * d;
				Graphics.DrawMesh(mesh, vector2, base.Rotation.AsQuat, this.Graphic.MatAt(base.Rotation, null), 0);
			}
			base.Comps_PostDraw();
		}

		private static int AlignQualityAgainst(IntVec3 c, Map map)
		{
			int result;
			if (!c.InBounds(map))
			{
				result = 0;
			}
			else if (!c.Walkable(map))
			{
				result = 9;
			}
			else
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
						goto IL_005f;
					Thing thing2 = thing as Blueprint;
					if (thing2 != null)
					{
						if (thing2.def.entityDefToBuild.passability == Traversability.Impassable)
							goto IL_008d;
						if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
							goto IL_00b4;
					}
				}
				result = 0;
			}
			goto IL_00d4;
			IL_00b4:
			result = 1;
			goto IL_00d4;
			IL_00d4:
			return result;
			IL_005f:
			result = 1;
			goto IL_00d4;
			IL_008d:
			result = 9;
			goto IL_00d4;
		}

		public static Rot4 DoorRotationAt(IntVec3 loc, Map map)
		{
			int num = 0;
			int num2 = 0;
			num += Building_Door.AlignQualityAgainst(loc + IntVec3.East, map);
			num += Building_Door.AlignQualityAgainst(loc + IntVec3.West, map);
			num2 += Building_Door.AlignQualityAgainst(loc + IntVec3.North, map);
			num2 += Building_Door.AlignQualityAgainst(loc + IntVec3.South, map);
			return (num < num2) ? Rot4.East : Rot4.North;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (base.Faction != Faction.OfPlayer)
				yield break;
			yield return (Gizmo)new Command_Toggle
			{
				defaultLabel = "CommandToggleDoorHoldOpen".Translate(),
				defaultDesc = "CommandToggleDoorHoldOpenDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc3,
				icon = TexCommand.HoldOpen,
				isActive = (Func<bool>)(() => ((_003CGetGizmos_003Ec__Iterator0)/*Error near IL_0129: stateMachine*/)._0024this.holdOpenInt),
				toggleAction = (Action)delegate
				{
					((_003CGetGizmos_003Ec__Iterator0)/*Error near IL_0140: stateMachine*/)._0024this.holdOpenInt = !((_003CGetGizmos_003Ec__Iterator0)/*Error near IL_0140: stateMachine*/)._0024this.holdOpenInt;
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_017b:
			/*Error near IL_017c: Unexpected return in MoveNext()*/;
		}

		private void ClearReachabilityCache(Map map)
		{
			map.reachability.ClearCache();
			this.freePassageWhenClearedReachabilityCache = this.FreePassage;
		}
	}
}
