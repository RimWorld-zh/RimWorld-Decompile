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
		private bool forPrisonersInt;

		private bool medicalInt;

		private bool alreadySetDefaultMed;

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
					if (Current.ProgramState != ProgramState.Playing)
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
				if (this.Medical)
				{
					Log.Warning("Tried to check for unowned sleeping slot on medical bed " + this);
					return false;
				}
				return this.owners.Count < this.SleepingSlotsCount;
			}
		}

		public bool AnyUnoccupiedSleepingSlot
		{
			get
			{
				for (int i = 0; i < this.SleepingSlotsCount; i++)
				{
					if (this.GetCurOccupant(i) == null)
					{
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<Pawn> CurOccupants
		{
			get
			{
				for (int i = 0; i < this.SleepingSlotsCount; i++)
				{
					Pawn occupant = this.GetCurOccupant(i);
					if (occupant != null)
					{
						yield return occupant;
					}
				}
			}
		}

		public override Color DrawColor
		{
			get
			{
				if (base.def.MadeFromStuff)
				{
					return base.DrawColor;
				}
				return this.DrawColorTwo;
			}
		}

		public override Color DrawColorTwo
		{
			get
			{
				if (!base.def.building.bed_humanlike)
				{
					return base.DrawColorTwo;
				}
				bool forPrisoners = this.ForPrisoners;
				bool medical = this.Medical;
				if (forPrisoners && medical)
				{
					return Building_Bed.SheetColorMedicalForPrisoner;
				}
				if (forPrisoners)
				{
					return Building_Bed.SheetColorForPrisoner;
				}
				if (medical)
				{
					return Building_Bed.SheetColorMedical;
				}
				if (base.def == ThingDefOf.RoyalBed)
				{
					return Building_Bed.SheetColorRoyal;
				}
				return Building_Bed.SheetColorNormal;
			}
		}

		public int SleepingSlotsCount
		{
			get
			{
				return base.def.size.x;
			}
		}

		public IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!base.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return base.Map.mapPawns.FreeColonists;
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
				if (base.Faction == Faction.OfPlayer)
				{
					return true;
				}
				int num = 0;
				while (num < this.owners.Count)
				{
					if (this.owners[num].Faction != Faction.OfPlayer && this.owners[num].HostFaction != Faction.OfPlayer)
					{
						num++;
						continue;
					}
					return true;
				}
				return false;
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
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			if (base.def.building.bed_humanlike && base.Faction == Faction.OfPlayer)
			{
				Command_Toggle pris = new Command_Toggle
				{
					defaultLabel = "CommandBedSetForPrisonersLabel".Translate(),
					defaultDesc = "CommandBedSetForPrisonersDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/ForPrisoners", true),
					isActive = (Func<bool>)(() => ((_003CGetGizmos_003Ec__Iterator154)/*Error near IL_0132: stateMachine*/)._003C_003Ef__this.ForPrisoners),
					toggleAction = (Action)delegate
					{
						((_003CGetGizmos_003Ec__Iterator154)/*Error near IL_0149: stateMachine*/)._003C_003Ef__this.ToggleForPrisonersByInterface();
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
				yield return (Gizmo)new Command_Toggle
				{
					defaultLabel = "CommandBedSetAsMedicalLabel".Translate(),
					defaultDesc = "CommandBedSetAsMedicalDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/AsMedical", true),
					isActive = (Func<bool>)(() => ((_003CGetGizmos_003Ec__Iterator154)/*Error near IL_0226: stateMachine*/)._003C_003Ef__this.Medical),
					toggleAction = (Action)delegate
					{
						((_003CGetGizmos_003Ec__Iterator154)/*Error near IL_023d: stateMachine*/)._003C_003Ef__this.Medical = !((_003CGetGizmos_003Ec__Iterator154)/*Error near IL_023d: stateMachine*/)._003C_003Ef__this.Medical;
					},
					hotKey = KeyBindingDefOf.Misc2
				};
				if (!this.ForPrisoners && !this.Medical)
				{
					yield return (Gizmo)new Command_Action
					{
						defaultLabel = "CommandBedSetOwnerLabel".Translate(),
						icon = ContentFinder<Texture2D>.Get("UI/Commands/AssignOwner", true),
						defaultDesc = "CommandBedSetOwnerDesc".Translate(),
						action = (Action)delegate
						{
							Find.WindowStack.Add(new Dialog_AssignBuildingOwner(((_003CGetGizmos_003Ec__Iterator154)/*Error near IL_02e7: stateMachine*/)._003C_003Ef__this));
						},
						hotKey = KeyBindingDefOf.Misc3
					};
				}
			}
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
					List<Building_Bed>.Enumerator enumerator4 = bedsToAffect.GetEnumerator();
					try
					{
						while (enumerator4.MoveNext())
						{
							Building_Bed current4 = enumerator4.Current;
							Room room2 = current4.GetRoom(RegionType.Set_Passable);
							current4.ForPrisoners = (newForPrisoners && !room2.TouchesMapEdge);
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
					}
					finally
					{
						((IDisposable)(object)enumerator4).Dispose();
					}
					List<Room>.Enumerator enumerator5 = list.GetEnumerator();
					try
					{
						while (enumerator5.MoveNext())
						{
							Room current5 = enumerator5.Current;
							current5.Notify_RoomShapeOrContainedBedsChanged();
						}
					}
					finally
					{
						((IDisposable)(object)enumerator5).Dispose();
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
					List<Building_Bed>.Enumerator enumerator3 = bedsToAffect.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							Building_Bed current3 = enumerator3.Current;
							if (newForPrisoners && !current3.ForPrisoners)
							{
								goto IL_0242;
							}
							if (!newForPrisoners && current3.ForPrisoners)
								goto IL_0242;
							continue;
							IL_0242:
							for (int i = 0; i < current3.owners.Count; i++)
							{
								stringBuilder.AppendLine();
								stringBuilder.Append(current3.owners[i].NameStringShort);
							}
						}
					}
					finally
					{
						((IDisposable)(object)enumerator3).Dispose();
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
					stringBuilder.AppendLine("RoomSurgerySuccessChanceFactor".Translate() + ": " + this.GetRoom(RegionType.Set_Passable).GetStat(RoomStatDefOf.SurgerySuccessChanceFactor).ToStringPercent());
					stringBuilder.AppendLine("RoomInfectionChanceFactor".Translate() + ": " + this.GetRoom(RegionType.Set_Passable).GetStat(RoomStatDefOf.InfectionChanceFactor).ToStringPercent());
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
			if (myPawn.RaceProps.Humanlike && !this.ForPrisoners && this.Medical && !myPawn.Drafted && base.Faction == Faction.OfPlayer)
			{
				if (!HealthAIUtility.ShouldSeekMedicalRest(myPawn) && !HealthAIUtility.ShouldSeekMedicalRestUrgent(myPawn))
				{
					yield return new FloatMenuOption("UseMedicalBed".Translate() + " (" + "NotInjured".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				else
				{
					Action sleep = (Action)delegate
					{
						if (!((_003CGetFloatMenuOptions_003Ec__Iterator155)/*Error near IL_00ee: stateMachine*/)._003C_003Ef__this.ForPrisoners && ((_003CGetFloatMenuOptions_003Ec__Iterator155)/*Error near IL_00ee: stateMachine*/)._003C_003Ef__this.Medical && ((_003CGetFloatMenuOptions_003Ec__Iterator155)/*Error near IL_00ee: stateMachine*/).myPawn.CanReserveAndReach((Thing)((_003CGetFloatMenuOptions_003Ec__Iterator155)/*Error near IL_00ee: stateMachine*/)._003C_003Ef__this, PathEndMode.ClosestTouch, Danger.Deadly, ((_003CGetFloatMenuOptions_003Ec__Iterator155)/*Error near IL_00ee: stateMachine*/)._003C_003Ef__this.SleepingSlotsCount, -1, null, true))
						{
							Job job = new Job(JobDefOf.LayDown, (Thing)((_003CGetFloatMenuOptions_003Ec__Iterator155)/*Error near IL_00ee: stateMachine*/)._003C_003Ef__this);
							job.restUntilHealed = true;
							((_003CGetFloatMenuOptions_003Ec__Iterator155)/*Error near IL_00ee: stateMachine*/).myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							((_003CGetFloatMenuOptions_003Ec__Iterator155)/*Error near IL_00ee: stateMachine*/).myPawn.mindState.ResetLastDisturbanceTick();
						}
					};
					if (this.AnyUnoccupiedSleepingSlot)
					{
						yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("UseMedicalBed".Translate(), sleep, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, (Thing)this, "ReservedBy");
					}
					else
					{
						yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("UseMedicalBed".Translate(), sleep, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, (Thing)this, "SomeoneElseSleeping");
					}
				}
			}
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
			if (!base.Spawned)
			{
				return null;
			}
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			List<Thing> list = base.Map.thingGrid.ThingsListAt(sleepingSlotPos);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null && pawn.CurJob != null && pawn.jobs.curDriver.layingDown == LayingDownState.LayingInBed)
				{
					return pawn;
				}
			}
			return null;
		}

		public int GetCurOccupantSlotIndex(Pawn curOccupant)
		{
			for (int i = 0; i < this.SleepingSlotsCount; i++)
			{
				if (this.GetCurOccupant(i) == curOccupant)
				{
					return i;
				}
			}
			Log.Error("Could not find pawn " + curOccupant + " on any of sleeping slots.");
			return 0;
		}

		public Pawn GetCurOccupantAt(IntVec3 pos)
		{
			for (int i = 0; i < this.SleepingSlotsCount; i++)
			{
				if (this.GetSleepingSlotPos(i) == pos)
				{
					return this.GetCurOccupant(i);
				}
			}
			return null;
		}

		public IntVec3 GetSleepingSlotPos(int index)
		{
			if (index >= 0 && index < this.SleepingSlotsCount)
			{
				CellRect cellRect = this.OccupiedRect();
				if (base.Rotation == Rot4.North)
				{
					int newX = cellRect.minX + index;
					IntVec3 position = base.Position;
					return new IntVec3(newX, position.y, cellRect.minZ);
				}
				if (base.Rotation == Rot4.East)
				{
					int minX = cellRect.minX;
					IntVec3 position2 = base.Position;
					return new IntVec3(minX, position2.y, cellRect.maxZ - index);
				}
				if (base.Rotation == Rot4.South)
				{
					int newX2 = cellRect.minX + index;
					IntVec3 position3 = base.Position;
					return new IntVec3(newX2, position3.y, cellRect.maxZ);
				}
				int maxX = cellRect.maxX;
				IntVec3 position4 = base.Position;
				return new IntVec3(maxX, position4.y, cellRect.maxZ - index);
			}
			Log.Error("Tried to get sleeping slot pos with index " + index + ", but there are only " + this.SleepingSlotsCount + " sleeping slots available.");
			return base.Position;
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
