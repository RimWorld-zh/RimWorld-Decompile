using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	public class Building_Bed : Building, IAssignableBuilding
	{
		private bool forPrisonersInt = false;

		private bool medicalInt = false;

		private bool alreadySetDefaultMed = false;

		public List<Pawn> owners = new List<Pawn>();

		private static int lastPrisonerSetChangeFrame = -1;

		private static readonly Color SheetColorNormal = new Color(0.6313726f, 0.8352941f, 0.7058824f);

		private static readonly Color SheetColorRoyal = new Color(0.670588255f, 0.9137255f, 0.745098054f);

		private static readonly Color SheetColorForPrisoner = new Color(1f, 0.7176471f, 0.129411772f);

		private static readonly Color SheetColorMedical = new Color(0.3882353f, 0.623529434f, 0.8862745f);

		private static readonly Color SheetColorMedicalForPrisoner = new Color(0.654902f, 0.3764706f, 0.152941182f);

		public bool ForPrisoners
		{
			get
			{
				return this.forPrisonersInt;
			}
			set
			{
				if (value != this.forPrisonersInt && base.def.building.bed_humanlike)
				{
					if (((Current.ProgramState != ProgramState.Playing) ? Scribe.mode : LoadSaveMode.Inactive) != 0)
					{
						Log.Error("Tried to set ForPrisoners while game mode was " + Current.ProgramState);
					}
					else
					{
						this.RemoveAllOwners();
						this.forPrisonersInt = value;
						this.Notify_ColorChanged();
						if (base.Spawned)
						{
							base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
							this.NotifyRoomBedTypeChanged();
						}
					}
				}
			}
		}

		public bool Medical
		{
			get
			{
				return this.medicalInt;
			}
			set
			{
				if (value != this.medicalInt && base.def.building.bed_humanlike)
				{
					this.RemoveAllOwners();
					this.medicalInt = value;
					this.Notify_ColorChanged();
					if (base.Spawned)
					{
						base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
						this.NotifyRoomBedTypeChanged();
					}
					this.FacilityChanged();
				}
			}
		}

		public bool AnyUnownedSleepingSlot
		{
			get
			{
				bool result;
				if (this.Medical)
				{
					Log.Warning("Tried to check for unowned sleeping slot on medical bed " + this);
					result = false;
				}
				else
				{
					result = (this.owners.Count < this.SleepingSlotsCount);
				}
				return result;
			}
		}

		public bool AnyUnoccupiedSleepingSlot
		{
			get
			{
				int num = 0;
				bool result;
				while (true)
				{
					if (num < this.SleepingSlotsCount)
					{
						if (this.GetCurOccupant(num) == null)
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

		public IEnumerable<Pawn> CurOccupants
		{
			get
			{
				int i = 0;
				Pawn occupant;
				while (true)
				{
					if (i < this.SleepingSlotsCount)
					{
						occupant = this.GetCurOccupant(i);
						if (occupant == null)
						{
							i++;
							continue;
						}
						break;
					}
					yield break;
				}
				yield return occupant;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public override Color DrawColor
		{
			get
			{
				return (!base.def.MadeFromStuff) ? this.DrawColorTwo : base.DrawColor;
			}
		}

		public override Color DrawColorTwo
		{
			get
			{
				Color result;
				if (!base.def.building.bed_humanlike)
				{
					result = base.DrawColorTwo;
				}
				else
				{
					bool forPrisoners = this.ForPrisoners;
					bool medical = this.Medical;
					result = ((!forPrisoners || !medical) ? ((!forPrisoners) ? ((!medical) ? ((base.def != ThingDefOf.RoyalBed) ? Building_Bed.SheetColorNormal : Building_Bed.SheetColorRoyal) : Building_Bed.SheetColorMedical) : Building_Bed.SheetColorForPrisoner) : Building_Bed.SheetColorMedicalForPrisoner);
				}
				return result;
			}
		}

		public int SleepingSlotsCount
		{
			get
			{
				return BedUtility.GetSleepingSlotsCount(base.def.size);
			}
		}

		public IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				return base.Spawned ? base.Map.mapPawns.FreeColonists : Enumerable.Empty<Pawn>();
			}
		}

		public IEnumerable<Pawn> AssignedPawns
		{
			get
			{
				return this.owners;
			}
		}

		public int MaxAssignedPawnsCount
		{
			get
			{
				return this.SleepingSlotsCount;
			}
		}

		private bool PlayerCanSeeOwners
		{
			get
			{
				bool result;
				if (base.Faction == Faction.OfPlayer)
				{
					result = true;
				}
				else
				{
					int num = 0;
					while (num < this.owners.Count)
					{
						if (this.owners[num].Faction != Faction.OfPlayer && this.owners[num].HostFaction != Faction.OfPlayer)
						{
							num++;
							continue;
						}
						goto IL_0056;
					}
					result = false;
				}
				goto IL_007a;
				IL_0056:
				result = true;
				goto IL_007a;
				IL_007a:
				return result;
			}
		}

		public void TryAssignPawn(Pawn owner)
		{
			owner.ownership.ClaimBedIfNonMedical(this);
		}

		public void TryUnassignPawn(Pawn pawn)
		{
			if (this.owners.Contains(pawn))
			{
				pawn.ownership.UnclaimBed();
			}
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(base.Position);
			if (validRegionAt_NoRebuild != null && validRegionAt_NoRebuild.Room.isPrisonCell)
			{
				this.ForPrisoners = true;
			}
			if (!this.alreadySetDefaultMed)
			{
				this.alreadySetDefaultMed = true;
				if (base.def.building.bed_defaultMedical)
				{
					this.Medical = true;
				}
			}
		}

		public override void DeSpawn()
		{
			this.RemoveAllOwners();
			this.ForPrisoners = false;
			this.Medical = false;
			this.alreadySetDefaultMed = false;
			Room room = this.GetRoom(RegionType.Set_Passable);
			base.DeSpawn();
			if (room != null)
			{
				room.Notify_RoomShapeOrContainedBedsChanged();
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forPrisonersInt, "forPrisoners", false, false);
			Scribe_Values.Look<bool>(ref this.medicalInt, "medical", false, false);
			Scribe_Values.Look<bool>(ref this.alreadySetDefaultMed, "alreadySetDefaultMed", false, false);
		}

		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Room room = this.GetRoom(RegionType.Set_Passable);
			if (room != null && Building_Bed.RoomCanBePrisonCell(room))
			{
				room.DrawFieldEdges();
			}
		}

		public static bool RoomCanBePrisonCell(Room r)
		{
			return !r.TouchesMapEdge && !r.IsHuge && r.RegionType == RegionType.Normal;
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
			if (!base.def.building.bed_humanlike)
				yield break;
			if (base.Faction != Faction.OfPlayer)
				yield break;
			Command_Toggle pris = new Command_Toggle
			{
				defaultLabel = "CommandBedSetForPrisonersLabel".Translate(),
				defaultDesc = "CommandBedSetForPrisonersDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Commands/ForPrisoners", true),
				isActive = (Func<bool>)(() => ((_003CGetGizmos_003Ec__Iterator1)/*Error near IL_0142: stateMachine*/)._0024this.ForPrisoners),
				toggleAction = (Action)delegate
				{
					((_003CGetGizmos_003Ec__Iterator1)/*Error near IL_0159: stateMachine*/)._0024this.ToggleForPrisonersByInterface();
				}
			};
			if (!Building_Bed.RoomCanBePrisonCell(this.GetRoom(RegionType.Set_Passable)) && !this.ForPrisoners)
			{
				pris.Disable("CommandBedSetForPrisonersFailOutdoors".Translate());
			}
			pris.hotKey = KeyBindingDefOf.Misc3;
			pris.turnOffSound = null;
			pris.turnOnSound = null;
			yield return (Gizmo)pris;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0357:
			/*Error near IL_0358: Unexpected return in MoveNext()*/;
		}

		private void ToggleForPrisonersByInterface()
		{
			if (Building_Bed.lastPrisonerSetChangeFrame != Time.frameCount)
			{
				Building_Bed.lastPrisonerSetChangeFrame = Time.frameCount;
				bool newForPrisoners = !this.ForPrisoners;
				SoundDef soundDef = (!newForPrisoners) ? SoundDefOf.CheckboxTurnedOff : SoundDefOf.CheckboxTurnedOn;
				soundDef.PlayOneShotOnCamera(null);
				List<Building_Bed> bedsToAffect = new List<Building_Bed>();
				foreach (Building_Bed item in (from so in Find.Selector.SelectedObjects
				where so is Building_Bed
				select so).Cast<Building_Bed>())
				{
					if (item.ForPrisoners != newForPrisoners)
					{
						Room room = item.GetRoom(RegionType.Set_Passable);
						if (room != null && Building_Bed.RoomCanBePrisonCell(room))
						{
							foreach (Building_Bed containedBed in room.ContainedBeds)
							{
								if (!bedsToAffect.Contains(containedBed))
								{
									bedsToAffect.Add(containedBed);
								}
							}
						}
						else if (!bedsToAffect.Contains(item))
						{
							bedsToAffect.Add(item);
						}
					}
				}
				Action action = (Action)delegate
				{
					List<Room> list = new List<Room>();
					foreach (Building_Bed item in bedsToAffect)
					{
						Room room2 = item.GetRoom(RegionType.Set_Passable);
						item.ForPrisoners = (newForPrisoners && !room2.TouchesMapEdge);
						for (int j = 0; j < this.SleepingSlotsCount; j++)
						{
							Pawn curOccupant = this.GetCurOccupant(j);
							if (curOccupant != null)
							{
								curOccupant.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
							}
						}
						if (!list.Contains(room2) && !room2.TouchesMapEdge)
						{
							list.Add(room2);
						}
					}
					foreach (Room item2 in list)
					{
						item2.Notify_RoomShapeOrContainedBedsChanged();
					}
				};
				if ((from b in bedsToAffect
				where b.owners.Any() && b != this
				select b).Count() == 0)
				{
					action();
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (newForPrisoners)
					{
						stringBuilder.Append("TurningOnPrisonerBedWarning".Translate());
					}
					else
					{
						stringBuilder.Append("TurningOffPrisonerBedWarning".Translate());
					}
					stringBuilder.AppendLine();
					foreach (Building_Bed item3 in bedsToAffect)
					{
						if (newForPrisoners && !item3.ForPrisoners)
						{
							goto IL_024d;
						}
						if (!newForPrisoners && item3.ForPrisoners)
							goto IL_024d;
						continue;
						IL_024d:
						for (int i = 0; i < item3.owners.Count; i++)
						{
							stringBuilder.AppendLine();
							stringBuilder.Append(item3.owners[i].NameStringShort);
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append("AreYouSure".Translate());
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), action, false, (string)null));
				}
			}
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (base.def.building.bed_humanlike)
			{
				stringBuilder.AppendLine();
				if (this.ForPrisoners)
				{
					stringBuilder.AppendLine("ForPrisonerUse".Translate());
				}
				else if (this.PlayerCanSeeOwners)
				{
					stringBuilder.AppendLine("ForColonistUse".Translate());
				}
				if (this.Medical)
				{
					stringBuilder.AppendLine("MedicalBed".Translate());
					if (base.Spawned)
					{
						stringBuilder.AppendLine("RoomInfectionChanceFactor".Translate() + ": " + this.GetRoom(RegionType.Set_Passable).GetStat(RoomStatDefOf.InfectionChanceFactor).ToStringPercent());
					}
				}
				else if (this.PlayerCanSeeOwners)
				{
					if (this.owners.Count == 0)
					{
						stringBuilder.AppendLine("Owner".Translate() + ": " + "Nobody".Translate().ToLower());
					}
					else if (this.owners.Count == 1)
					{
						stringBuilder.AppendLine("Owner".Translate() + ": " + this.owners[0].Label);
					}
					else
					{
						stringBuilder.Append("Owners".Translate() + ": ");
						bool flag = false;
						for (int i = 0; i < this.owners.Count; i++)
						{
							if (flag)
							{
								stringBuilder.Append(", ");
							}
							flag = true;
							stringBuilder.Append(this.owners[i].LabelShort);
						}
						stringBuilder.AppendLine();
					}
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			_003CGetFloatMenuOptions_003Ec__Iterator2 _003CGetFloatMenuOptions_003Ec__Iterator = (_003CGetFloatMenuOptions_003Ec__Iterator2)/*Error near IL_003a: stateMachine*/;
			if (!myPawn.RaceProps.Humanlike)
				yield break;
			if (this.ForPrisoners)
				yield break;
			if (!this.Medical)
				yield break;
			if (myPawn.Drafted)
				yield break;
			if (base.Faction != Faction.OfPlayer)
				yield break;
			if (!RestUtility.CanUseBedEver(myPawn, base.def))
				yield break;
			if (!HealthAIUtility.ShouldSeekMedicalRest(myPawn) && !HealthAIUtility.ShouldSeekMedicalRestUrgent(myPawn))
			{
				yield return new FloatMenuOption("UseMedicalBed".Translate() + " (" + "NotInjured".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			Action sleep = (Action)delegate()
			{
				if (!_003CGetFloatMenuOptions_003Ec__Iterator._0024this.ForPrisoners && _003CGetFloatMenuOptions_003Ec__Iterator._0024this.Medical && myPawn.CanReserveAndReach((Thing)_003CGetFloatMenuOptions_003Ec__Iterator._0024this, PathEndMode.ClosestTouch, Danger.Deadly, _003CGetFloatMenuOptions_003Ec__Iterator._0024this.SleepingSlotsCount, -1, null, true))
				{
					Job job = new Job(JobDefOf.LayDown, (Thing)_003CGetFloatMenuOptions_003Ec__Iterator._0024this);
					job.restUntilHealed = true;
					myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					myPawn.mindState.ResetLastDisturbanceTick();
				}
			};
			if (this.AnyUnoccupiedSleepingSlot)
			{
				yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("UseMedicalBed".Translate(), sleep, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, (Thing)this, "ReservedBy");
				/*Error: Unable to find new state assignment for yield return*/;
			}
			yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("UseMedicalBed".Translate(), sleep, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, (Thing)this, "SomeoneElseSleeping");
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override void DrawGUIOverlay()
		{
			if (!this.Medical && Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest && this.PlayerCanSeeOwners)
			{
				Color defaultThingLabelColor = GenMapUI.DefaultThingLabelColor;
				if (!this.owners.Any())
				{
					GenMapUI.DrawThingLabel(this, "Unowned".Translate(), defaultThingLabelColor);
				}
				else if (this.owners.Count == 1)
				{
					if (this.owners[0].InBed() && this.owners[0].CurrentBed() == this)
						return;
					GenMapUI.DrawThingLabel(this, this.owners[0].NameStringShort, defaultThingLabelColor);
				}
				else
				{
					for (int i = 0; i < this.owners.Count; i++)
					{
						if (!this.owners[i].InBed() || this.owners[i].CurrentBed() != this || !(this.owners[i].Position == this.GetSleepingSlotPos(i)))
						{
							Vector3 multiOwnersLabelScreenPosFor = this.GetMultiOwnersLabelScreenPosFor(i);
							GenMapUI.DrawThingLabel(multiOwnersLabelScreenPosFor, this.owners[i].NameStringShort, defaultThingLabelColor);
						}
					}
				}
			}
		}

		public Pawn GetCurOccupant(int slotIndex)
		{
			Pawn result;
			Pawn pawn;
			if (!base.Spawned)
			{
				result = null;
			}
			else
			{
				IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
				List<Thing> list = base.Map.thingGrid.ThingsListAt(sleepingSlotPos);
				for (int i = 0; i < list.Count; i++)
				{
					pawn = (list[i] as Pawn);
					if (pawn != null && pawn.CurJob != null && pawn.jobs.curDriver.layingDown == LayingDownState.LayingInBed)
						goto IL_0073;
				}
				result = null;
			}
			goto IL_0094;
			IL_0094:
			return result;
			IL_0073:
			result = pawn;
			goto IL_0094;
		}

		public int GetCurOccupantSlotIndex(Pawn curOccupant)
		{
			int num = 0;
			int result;
			while (true)
			{
				if (num < this.SleepingSlotsCount)
				{
					if (this.GetCurOccupant(num) == curOccupant)
					{
						result = num;
						break;
					}
					num++;
					continue;
				}
				Log.Error("Could not find pawn " + curOccupant + " on any of sleeping slots.");
				result = 0;
				break;
			}
			return result;
		}

		public Pawn GetCurOccupantAt(IntVec3 pos)
		{
			int num = 0;
			Pawn result;
			while (true)
			{
				if (num < this.SleepingSlotsCount)
				{
					if (this.GetSleepingSlotPos(num) == pos)
					{
						result = this.GetCurOccupant(num);
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

		public IntVec3 GetSleepingSlotPos(int index)
		{
			return BedUtility.GetSleepingSlotPos(index, base.Position, base.Rotation, base.def.size);
		}

		public void SortOwners()
		{
			this.owners.SortBy((Func<Pawn, int>)((Pawn x) => x.thingIDNumber));
		}

		private void RemoveAllOwners()
		{
			for (int num = this.owners.Count - 1; num >= 0; num--)
			{
				this.owners[num].ownership.UnclaimBed();
			}
		}

		private void NotifyRoomBedTypeChanged()
		{
			Room room = this.GetRoom(RegionType.Set_Passable);
			if (room != null)
			{
				room.Notify_BedTypeChanged();
			}
		}

		private void FacilityChanged()
		{
			CompFacility compFacility = this.TryGetComp<CompFacility>();
			CompAffectedByFacilities compAffectedByFacilities = this.TryGetComp<CompAffectedByFacilities>();
			if (compFacility != null)
			{
				compFacility.Notify_ThingChanged();
			}
			if (compAffectedByFacilities != null)
			{
				compAffectedByFacilities.Notify_ThingChanged();
			}
		}

		private Vector3 GetMultiOwnersLabelScreenPosFor(int slotIndex)
		{
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			Vector3 drawPos = this.DrawPos;
			if (base.Rotation.IsHorizontal)
			{
				drawPos.z = (float)((float)sleepingSlotPos.z + 0.60000002384185791);
			}
			else
			{
				drawPos.x = (float)((float)sleepingSlotPos.x + 0.5);
				drawPos.z += -0.4f;
			}
			Vector2 v = drawPos.MapToUIPosition();
			if (!base.Rotation.IsHorizontal && this.SleepingSlotsCount == 2)
			{
				v = this.AdjustOwnerLabelPosToAvoidOverlapping(v, slotIndex);
			}
			return v;
		}

		private Vector3 AdjustOwnerLabelPosToAvoidOverlapping(Vector3 screenPos, int slotIndex)
		{
			Text.Font = GameFont.Tiny;
			Vector2 vector = Text.CalcSize(this.owners[slotIndex].NameStringShort);
			float num = (float)(vector.x + 1.0);
			Vector2 vector2 = this.DrawPos.MapToUIPosition();
			float num2 = Mathf.Abs(screenPos.x - vector2.x);
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			if (num > num2 * 2.0)
			{
				float num3 = 0f;
				if (slotIndex == 0)
				{
					IntVec3 sleepingSlotPos2 = this.GetSleepingSlotPos(1);
					num3 = (float)sleepingSlotPos2.x;
				}
				else
				{
					IntVec3 sleepingSlotPos3 = this.GetSleepingSlotPos(0);
					num3 = (float)sleepingSlotPos3.x;
				}
				if ((float)sleepingSlotPos.x < num3)
				{
					screenPos.x -= (float)((num - num2 * 2.0) / 2.0);
				}
				else
				{
					screenPos.x += (float)((num - num2 * 2.0) / 2.0);
				}
			}
			return screenPos;
		}
	}
}
