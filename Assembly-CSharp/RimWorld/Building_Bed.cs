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
	// Token: 0x0200069D RID: 1693
	public class Building_Bed : Building, IAssignableBuilding
	{
		// Token: 0x0400140D RID: 5133
		private bool forPrisonersInt = false;

		// Token: 0x0400140E RID: 5134
		private bool medicalInt = false;

		// Token: 0x0400140F RID: 5135
		private bool alreadySetDefaultMed = false;

		// Token: 0x04001410 RID: 5136
		public List<Pawn> owners = new List<Pawn>();

		// Token: 0x04001411 RID: 5137
		private static int lastPrisonerSetChangeFrame = -1;

		// Token: 0x04001412 RID: 5138
		private static readonly Color SheetColorNormal = new Color(0.6313726f, 0.8352941f, 0.7058824f);

		// Token: 0x04001413 RID: 5139
		private static readonly Color SheetColorRoyal = new Color(0.670588255f, 0.9137255f, 0.745098054f);

		// Token: 0x04001414 RID: 5140
		public static readonly Color SheetColorForPrisoner = new Color(1f, 0.7176471f, 0.129411772f);

		// Token: 0x04001415 RID: 5141
		private static readonly Color SheetColorMedical = new Color(0.3882353f, 0.623529434f, 0.8862745f);

		// Token: 0x04001416 RID: 5142
		private static readonly Color SheetColorMedicalForPrisoner = new Color(0.654902f, 0.3764706f, 0.152941182f);

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x001347C4 File Offset: 0x00132BC4
		// (set) Token: 0x060023DF RID: 9183 RVA: 0x001347E0 File Offset: 0x00132BE0
		public bool ForPrisoners
		{
			get
			{
				return this.forPrisonersInt;
			}
			set
			{
				if (value != this.forPrisonersInt && this.def.building.bed_humanlike)
				{
					if (Current.ProgramState != ProgramState.Playing && Scribe.mode != LoadSaveMode.Inactive)
					{
						Log.Error("Tried to set ForPrisoners while game mode was " + Current.ProgramState, false);
					}
					else
					{
						this.RemoveAllOwners();
						this.forPrisonersInt = value;
						this.Notify_ColorChanged();
						this.NotifyRoomBedTypeChanged();
					}
				}
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x060023E0 RID: 9184 RVA: 0x00134864 File Offset: 0x00132C64
		// (set) Token: 0x060023E1 RID: 9185 RVA: 0x00134880 File Offset: 0x00132C80
		public bool Medical
		{
			get
			{
				return this.medicalInt;
			}
			set
			{
				if (value != this.medicalInt && this.def.building.bed_humanlike)
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

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x060023E2 RID: 9186 RVA: 0x001348F8 File Offset: 0x00132CF8
		public bool AnyUnownedSleepingSlot
		{
			get
			{
				bool result;
				if (this.Medical)
				{
					Log.Warning("Tried to check for unowned sleeping slot on medical bed " + this, false);
					result = false;
				}
				else
				{
					result = (this.owners.Count < this.SleepingSlotsCount);
				}
				return result;
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x060023E3 RID: 9187 RVA: 0x00134944 File Offset: 0x00132D44
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

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x060023E4 RID: 9188 RVA: 0x00134988 File Offset: 0x00132D88
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
				yield break;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x060023E5 RID: 9189 RVA: 0x001349B4 File Offset: 0x00132DB4
		public override Color DrawColor
		{
			get
			{
				Color result;
				if (this.def.MadeFromStuff)
				{
					result = base.DrawColor;
				}
				else
				{
					result = this.DrawColorTwo;
				}
				return result;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x060023E6 RID: 9190 RVA: 0x001349EC File Offset: 0x00132DEC
		public override Color DrawColorTwo
		{
			get
			{
				Color result;
				if (!this.def.building.bed_humanlike)
				{
					result = base.DrawColorTwo;
				}
				else
				{
					bool forPrisoners = this.ForPrisoners;
					bool medical = this.Medical;
					if (forPrisoners && medical)
					{
						result = Building_Bed.SheetColorMedicalForPrisoner;
					}
					else if (forPrisoners)
					{
						result = Building_Bed.SheetColorForPrisoner;
					}
					else if (medical)
					{
						result = Building_Bed.SheetColorMedical;
					}
					else if (this.def == ThingDefOf.RoyalBed)
					{
						result = Building_Bed.SheetColorRoyal;
					}
					else
					{
						result = Building_Bed.SheetColorNormal;
					}
				}
				return result;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x060023E7 RID: 9191 RVA: 0x00134A8C File Offset: 0x00132E8C
		public int SleepingSlotsCount
		{
			get
			{
				return BedUtility.GetSleepingSlotsCount(this.def.size);
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x060023E8 RID: 9192 RVA: 0x00134AB4 File Offset: 0x00132EB4
		public IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				IEnumerable<Pawn> result;
				if (!base.Spawned)
				{
					result = Enumerable.Empty<Pawn>();
				}
				else
				{
					result = base.Map.mapPawns.FreeColonists;
				}
				return result;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x060023E9 RID: 9193 RVA: 0x00134AF0 File Offset: 0x00132EF0
		public IEnumerable<Pawn> AssignedPawns
		{
			get
			{
				return this.owners;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x060023EA RID: 9194 RVA: 0x00134B0C File Offset: 0x00132F0C
		public int MaxAssignedPawnsCount
		{
			get
			{
				return this.SleepingSlotsCount;
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x060023EB RID: 9195 RVA: 0x00134B28 File Offset: 0x00132F28
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
					for (int i = 0; i < this.owners.Count; i++)
					{
						if (this.owners[i].Faction == Faction.OfPlayer || this.owners[i].HostFaction == Faction.OfPlayer)
						{
							return true;
						}
					}
					result = false;
				}
				return result;
			}
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x00134BB0 File Offset: 0x00132FB0
		public void TryAssignPawn(Pawn owner)
		{
			owner.ownership.ClaimBedIfNonMedical(this);
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x00134BBF File Offset: 0x00132FBF
		public void TryUnassignPawn(Pawn pawn)
		{
			if (this.owners.Contains(pawn))
			{
				pawn.ownership.UnclaimBed();
			}
		}

		// Token: 0x060023EE RID: 9198 RVA: 0x00134BE0 File Offset: 0x00132FE0
		public bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.OwnedBed != null;
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x00134C08 File Offset: 0x00133008
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
				if (this.def.building.bed_defaultMedical)
				{
					this.Medical = true;
				}
			}
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x00134C80 File Offset: 0x00133080
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			this.RemoveAllOwners();
			this.ForPrisoners = false;
			this.Medical = false;
			this.alreadySetDefaultMed = false;
			Room room = this.GetRoom(RegionType.Set_Passable);
			base.DeSpawn(mode);
			if (room != null)
			{
				room.Notify_RoomShapeOrContainedBedsChanged();
			}
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x00134CC4 File Offset: 0x001330C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forPrisonersInt, "forPrisoners", false, false);
			Scribe_Values.Look<bool>(ref this.medicalInt, "medical", false, false);
			Scribe_Values.Look<bool>(ref this.alreadySetDefaultMed, "alreadySetDefaultMed", false, false);
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x00134D04 File Offset: 0x00133104
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Room room = this.GetRoom(RegionType.Set_Passable);
			if (room != null && Building_Bed.RoomCanBePrisonCell(room))
			{
				room.DrawFieldEdges();
			}
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x00134D38 File Offset: 0x00133138
		public static bool RoomCanBePrisonCell(Room r)
		{
			return !r.TouchesMapEdge && !r.IsHuge && r.RegionType == RegionType.Normal;
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x00134D70 File Offset: 0x00133170
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			if (this.def.building.bed_humanlike && base.Faction == Faction.OfPlayer)
			{
				Command_Toggle pris = new Command_Toggle();
				pris.defaultLabel = "CommandBedSetForPrisonersLabel".Translate();
				pris.defaultDesc = "CommandBedSetForPrisonersDesc".Translate();
				pris.icon = ContentFinder<Texture2D>.Get("UI/Commands/ForPrisoners", true);
				pris.isActive = (() => this.ForPrisoners);
				pris.toggleAction = delegate()
				{
					this.ToggleForPrisonersByInterface();
				};
				if (!Building_Bed.RoomCanBePrisonCell(this.GetRoom(RegionType.Set_Passable)) && !this.ForPrisoners)
				{
					pris.Disable("CommandBedSetForPrisonersFailOutdoors".Translate());
				}
				pris.hotKey = KeyBindingDefOf.Misc3;
				pris.turnOffSound = null;
				pris.turnOnSound = null;
				yield return pris;
				yield return new Command_Toggle
				{
					defaultLabel = "CommandBedSetAsMedicalLabel".Translate(),
					defaultDesc = "CommandBedSetAsMedicalDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/AsMedical", true),
					isActive = (() => this.Medical),
					toggleAction = delegate()
					{
						this.Medical = !this.Medical;
					},
					hotKey = KeyBindingDefOf.Misc2
				};
				if (!this.ForPrisoners && !this.Medical)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandBedSetOwnerLabel".Translate(),
						icon = ContentFinder<Texture2D>.Get("UI/Commands/AssignOwner", true),
						defaultDesc = "CommandBedSetOwnerDesc".Translate(),
						action = delegate()
						{
							Find.WindowStack.Add(new Dialog_AssignBuildingOwner(this));
						},
						hotKey = KeyBindingDefOf.Misc3
					};
				}
			}
			yield break;
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x00134D9C File Offset: 0x0013319C
		private void ToggleForPrisonersByInterface()
		{
			if (Building_Bed.lastPrisonerSetChangeFrame != Time.frameCount)
			{
				Building_Bed.lastPrisonerSetChangeFrame = Time.frameCount;
				bool newForPrisoners = !this.ForPrisoners;
				SoundDef soundDef = (!newForPrisoners) ? SoundDefOf.Checkbox_TurnedOff : SoundDefOf.Checkbox_TurnedOn;
				soundDef.PlayOneShotOnCamera(null);
				List<Building_Bed> bedsToAffect = new List<Building_Bed>();
				foreach (Building_Bed building_Bed in Find.Selector.SelectedObjects.OfType<Building_Bed>())
				{
					if (building_Bed.ForPrisoners != newForPrisoners)
					{
						Room room = building_Bed.GetRoom(RegionType.Set_Passable);
						if (room == null || !Building_Bed.RoomCanBePrisonCell(room))
						{
							if (!bedsToAffect.Contains(building_Bed))
							{
								bedsToAffect.Add(building_Bed);
							}
						}
						else
						{
							foreach (Building_Bed item in room.ContainedBeds)
							{
								if (!bedsToAffect.Contains(item))
								{
									bedsToAffect.Add(item);
								}
							}
						}
					}
				}
				Action action = delegate()
				{
					List<Room> list = new List<Room>();
					foreach (Building_Bed building_Bed3 in bedsToAffect)
					{
						Room room2 = building_Bed3.GetRoom(RegionType.Set_Passable);
						building_Bed3.ForPrisoners = (newForPrisoners && !room2.TouchesMapEdge);
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
					foreach (Room room3 in list)
					{
						room3.Notify_RoomShapeOrContainedBedsChanged();
					}
				};
				if ((from b in bedsToAffect
				where b.owners.Any<Pawn>() && b != this
				select b).Count<Building_Bed>() == 0)
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
					foreach (Building_Bed building_Bed2 in bedsToAffect)
					{
						if ((newForPrisoners && !building_Bed2.ForPrisoners) || (!newForPrisoners && building_Bed2.ForPrisoners))
						{
							for (int i = 0; i < building_Bed2.owners.Count; i++)
							{
								stringBuilder.AppendLine();
								stringBuilder.Append(building_Bed2.owners[i].LabelShort);
							}
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append("AreYouSure".Translate());
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), action, false, null));
				}
			}
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x001350A4 File Offset: 0x001334A4
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (this.def.building.bed_humanlike)
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
						stringBuilder.AppendLine("Owner".Translate() + ": " + "Nobody".Translate());
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

		// Token: 0x060023F7 RID: 9207 RVA: 0x0013528C File Offset: 0x0013368C
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			if (myPawn.RaceProps.Humanlike && !this.ForPrisoners && this.Medical && !myPawn.Drafted && base.Faction == Faction.OfPlayer && RestUtility.CanUseBedEver(myPawn, this.def))
			{
				if (!HealthAIUtility.ShouldSeekMedicalRest(myPawn) && !HealthAIUtility.ShouldSeekMedicalRestUrgent(myPawn))
				{
					yield return new FloatMenuOption("UseMedicalBed".Translate() + " (" + "NotInjured".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				else
				{
					Action sleep = delegate()
					{
						if (!this.ForPrisoners && this.Medical && myPawn.CanReserveAndReach(this.$this, PathEndMode.ClosestTouch, Danger.Deadly, this.SleepingSlotsCount, -1, null, true))
						{
							if (myPawn.CurJobDef == JobDefOf.LayDown && myPawn.CurJob.GetTarget(TargetIndex.A).Thing == this.$this)
							{
								myPawn.CurJob.restUntilHealed = true;
							}
							else
							{
								Job job = new Job(JobDefOf.LayDown, this.$this);
								job.restUntilHealed = true;
								myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							}
							myPawn.mindState.ResetLastDisturbanceTick();
						}
					};
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("UseMedicalBed".Translate(), sleep, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, this, (!this.AnyUnoccupiedSleepingSlot) ? "SomeoneElseSleeping" : "ReservedBy");
				}
			}
			yield break;
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x001352C0 File Offset: 0x001336C0
		public override void DrawGUIOverlay()
		{
			if (!this.Medical)
			{
				if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest && this.PlayerCanSeeOwners)
				{
					Color defaultThingLabelColor = GenMapUI.DefaultThingLabelColor;
					if (!this.owners.Any<Pawn>())
					{
						GenMapUI.DrawThingLabel(this, "Unowned".Translate(), defaultThingLabelColor);
					}
					else if (this.owners.Count == 1)
					{
						if (!this.owners[0].InBed() || this.owners[0].CurrentBed() != this)
						{
							GenMapUI.DrawThingLabel(this, this.owners[0].LabelShort, defaultThingLabelColor);
						}
					}
					else
					{
						for (int i = 0; i < this.owners.Count; i++)
						{
							if (!this.owners[i].InBed() || this.owners[i].CurrentBed() != this || !(this.owners[i].Position == this.GetSleepingSlotPos(i)))
							{
								Vector3 multiOwnersLabelScreenPosFor = this.GetMultiOwnersLabelScreenPosFor(i);
								GenMapUI.DrawThingLabel(multiOwnersLabelScreenPosFor, this.owners[i].LabelShort, defaultThingLabelColor);
							}
						}
					}
				}
			}
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x00135424 File Offset: 0x00133824
		public Pawn GetCurOccupant(int slotIndex)
		{
			Pawn result;
			if (!base.Spawned)
			{
				result = null;
			}
			else
			{
				IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
				List<Thing> list = base.Map.thingGrid.ThingsListAt(sleepingSlotPos);
				int i = 0;
				while (i < list.Count)
				{
					Pawn pawn = list[i] as Pawn;
					if (pawn != null)
					{
						if (pawn.CurJob != null)
						{
							if (pawn.GetPosture() == PawnPosture.LayingInBed)
							{
								return pawn;
							}
						}
					}
					IL_73:
					i++;
					continue;
					goto IL_73;
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x001354BC File Offset: 0x001338BC
		public int GetCurOccupantSlotIndex(Pawn curOccupant)
		{
			for (int i = 0; i < this.SleepingSlotsCount; i++)
			{
				if (this.GetCurOccupant(i) == curOccupant)
				{
					return i;
				}
			}
			Log.Error("Could not find pawn " + curOccupant + " on any of sleeping slots.", false);
			return 0;
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x00135518 File Offset: 0x00133918
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

		// Token: 0x060023FC RID: 9212 RVA: 0x00135568 File Offset: 0x00133968
		public IntVec3 GetSleepingSlotPos(int index)
		{
			return BedUtility.GetSleepingSlotPos(index, base.Position, base.Rotation, this.def.size);
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x0013559A File Offset: 0x0013399A
		public void SortOwners()
		{
			this.owners.SortBy((Pawn x) => x.thingIDNumber);
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x001355C8 File Offset: 0x001339C8
		private void RemoveAllOwners()
		{
			for (int i = this.owners.Count - 1; i >= 0; i--)
			{
				this.owners[i].ownership.UnclaimBed();
			}
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x0013560C File Offset: 0x00133A0C
		private void NotifyRoomBedTypeChanged()
		{
			Room room = this.GetRoom(RegionType.Set_Passable);
			if (room != null)
			{
				room.Notify_BedTypeChanged();
			}
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x00135630 File Offset: 0x00133A30
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

		// Token: 0x06002401 RID: 9217 RVA: 0x00135664 File Offset: 0x00133A64
		private Vector3 GetMultiOwnersLabelScreenPosFor(int slotIndex)
		{
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			Vector3 drawPos = this.DrawPos;
			if (base.Rotation.IsHorizontal)
			{
				drawPos.z = (float)sleepingSlotPos.z + 0.6f;
			}
			else
			{
				drawPos.x = (float)sleepingSlotPos.x + 0.5f;
				drawPos.z += -0.4f;
			}
			Vector2 v = drawPos.MapToUIPosition();
			if (!base.Rotation.IsHorizontal && this.SleepingSlotsCount == 2)
			{
				v = this.AdjustOwnerLabelPosToAvoidOverlapping(v, slotIndex);
			}
			return v;
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x00135724 File Offset: 0x00133B24
		private Vector3 AdjustOwnerLabelPosToAvoidOverlapping(Vector3 screenPos, int slotIndex)
		{
			Text.Font = GameFont.Tiny;
			float num = Text.CalcSize(this.owners[slotIndex].LabelShort).x + 1f;
			Vector2 vector = this.DrawPos.MapToUIPosition();
			float num2 = Mathf.Abs(screenPos.x - vector.x);
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			if (num > num2 * 2f)
			{
				float num3;
				if (slotIndex == 0)
				{
					num3 = (float)this.GetSleepingSlotPos(1).x;
				}
				else
				{
					num3 = (float)this.GetSleepingSlotPos(0).x;
				}
				if ((float)sleepingSlotPos.x < num3)
				{
					screenPos.x -= (num - num2 * 2f) / 2f;
				}
				else
				{
					screenPos.x += (num - num2 * 2f) / 2f;
				}
			}
			return screenPos;
		}
	}
}
