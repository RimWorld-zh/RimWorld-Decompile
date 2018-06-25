using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

		public static readonly Color SheetColorForPrisoner = new Color(1f, 0.7176471f, 0.129411772f);

		private static readonly Color SheetColorMedical = new Color(0.3882353f, 0.623529434f, 0.8862745f);

		private static readonly Color SheetColorMedicalForPrisoner = new Color(0.654902f, 0.3764706f, 0.152941182f);

		[CompilerGenerated]
		private static Func<Pawn, int> <>f__am$cache0;

		public Building_Bed()
		{
		}

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
				yield break;
			}
		}

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

		public int SleepingSlotsCount
		{
			get
			{
				return BedUtility.GetSleepingSlotsCount(this.def.size);
			}
		}

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

		public bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.OwnedBed != null;
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
				if (this.def.building.bed_defaultMedical)
				{
					this.Medical = true;
				}
			}
		}

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
			return BedUtility.GetSleepingSlotPos(index, base.Position, base.Rotation, this.def.size);
		}

		public void SortOwners()
		{
			this.owners.SortBy((Pawn x) => x.thingIDNumber);
		}

		private void RemoveAllOwners()
		{
			for (int i = this.owners.Count - 1; i >= 0; i--)
			{
				this.owners[i].ownership.UnclaimBed();
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

		// Note: this type is marked as 'beforefieldinit'.
		static Building_Bed()
		{
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <GetGizmos>__BaseCallProxy0()
		{
			return base.GetGizmos();
		}

		[CompilerGenerated]
		private static int <SortOwners>m__0(Pawn x)
		{
			return x.thingIDNumber;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal int <i>__1;

			internal Pawn <occupant>__2;

			internal Building_Bed $this;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					IL_71:
					i++;
					break;
				default:
					return false;
				}
				if (i >= base.SleepingSlotsCount)
				{
					this.$PC = -1;
				}
				else
				{
					occupant = base.GetCurOccupant(i);
					if (occupant != null)
					{
						this.$current = occupant;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_71;
				}
				return false;
			}

			Pawn IEnumerator<Pawn>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Pawn>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Building_Bed.<>c__Iterator0 <>c__Iterator = new Building_Bed.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator1 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <g>__1;

			internal Command_Toggle <pris>__2;

			internal Command_Toggle <med>__3;

			internal Command_Action <own>__4;

			internal Building_Bed $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator1()
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
					enumerator = base.<GetGizmos>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
				{
					Command_Toggle med = new Command_Toggle();
					med.defaultLabel = "CommandBedSetAsMedicalLabel".Translate();
					med.defaultDesc = "CommandBedSetAsMedicalDesc".Translate();
					med.icon = ContentFinder<Texture2D>.Get("UI/Commands/AsMedical", true);
					med.isActive = (() => base.Medical);
					med.toggleAction = delegate()
					{
						base.Medical = !base.Medical;
					};
					med.hotKey = KeyBindingDefOf.Misc2;
					this.$current = med;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				case 3u:
					if (!base.ForPrisoners && !base.Medical)
					{
						Command_Action own = new Command_Action();
						own.defaultLabel = "CommandBedSetOwnerLabel".Translate();
						own.icon = ContentFinder<Texture2D>.Get("UI/Commands/AssignOwner", true);
						own.defaultDesc = "CommandBedSetOwnerDesc".Translate();
						own.action = delegate()
						{
							Find.WindowStack.Add(new Dialog_AssignBuildingOwner(this));
						};
						own.hotKey = KeyBindingDefOf.Misc3;
						this.$current = own;
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						return true;
					}
					goto IL_34D;
				case 4u:
					goto IL_34D;
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
				if (this.def.building.bed_humanlike && base.Faction == Faction.OfPlayer)
				{
					pris = new Command_Toggle();
					pris.defaultLabel = "CommandBedSetForPrisonersLabel".Translate();
					pris.defaultDesc = "CommandBedSetForPrisonersDesc".Translate();
					pris.icon = ContentFinder<Texture2D>.Get("UI/Commands/ForPrisoners", true);
					pris.isActive = (() => base.ForPrisoners);
					pris.toggleAction = delegate()
					{
						base.ToggleForPrisonersByInterface();
					};
					if (!Building_Bed.RoomCanBePrisonCell(this.GetRoom(RegionType.Set_Passable)) && !base.ForPrisoners)
					{
						pris.Disable("CommandBedSetForPrisonersFailOutdoors".Translate());
					}
					pris.hotKey = KeyBindingDefOf.Misc3;
					pris.turnOffSound = null;
					pris.turnOnSound = null;
					this.$current = pris;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_34D:
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
				Building_Bed.<GetGizmos>c__Iterator1 <GetGizmos>c__Iterator = new Building_Bed.<GetGizmos>c__Iterator1();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}

			internal bool <>m__0()
			{
				return base.ForPrisoners;
			}

			internal void <>m__1()
			{
				base.ToggleForPrisonersByInterface();
			}

			internal bool <>m__2()
			{
				return base.Medical;
			}

			internal void <>m__3()
			{
				base.Medical = !base.Medical;
			}

			internal void <>m__4()
			{
				Find.WindowStack.Add(new Dialog_AssignBuildingOwner(this));
			}
		}

		[CompilerGenerated]
		private sealed class <ToggleForPrisonersByInterface>c__AnonStorey3
		{
			internal List<Building_Bed> bedsToAffect;

			internal bool newForPrisoners;

			internal Building_Bed $this;

			public <ToggleForPrisonersByInterface>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				List<Room> list = new List<Room>();
				foreach (Building_Bed building_Bed in this.bedsToAffect)
				{
					Room room = building_Bed.GetRoom(RegionType.Set_Passable);
					building_Bed.ForPrisoners = (this.newForPrisoners && !room.TouchesMapEdge);
					for (int i = 0; i < this.$this.SleepingSlotsCount; i++)
					{
						Pawn curOccupant = this.$this.GetCurOccupant(i);
						if (curOccupant != null)
						{
							curOccupant.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
						}
					}
					if (!list.Contains(room) && !room.TouchesMapEdge)
					{
						list.Add(room);
					}
				}
				foreach (Room room2 in list)
				{
					room2.Notify_RoomShapeOrContainedBedsChanged();
				}
			}

			internal bool <>m__1(Building_Bed b)
			{
				return b.owners.Any<Pawn>() && b != this.$this;
			}
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__Iterator2 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal Pawn myPawn;

			internal Action <sleep>__1;

			internal Building_Bed $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			private Building_Bed.<GetFloatMenuOptions>c__Iterator2.<GetFloatMenuOptions>c__AnonStorey4 $locvar0;

			[DebuggerHidden]
			public <GetFloatMenuOptions>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (myPawn.RaceProps.Humanlike && !base.ForPrisoners && base.Medical && !myPawn.Drafted && base.Faction == Faction.OfPlayer && RestUtility.CanUseBedEver(myPawn, this.def))
					{
						if (!HealthAIUtility.ShouldSeekMedicalRest(myPawn) && !HealthAIUtility.ShouldSeekMedicalRestUrgent(myPawn))
						{
							this.$current = new FloatMenuOption("UseMedicalBed".Translate() + " (" + "NotInjured".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
						}
						else
						{
							sleep = delegate()
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
							this.$current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("UseMedicalBed".Translate(), sleep, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, this, (!base.AnyUnoccupiedSleepingSlot) ? "SomeoneElseSleeping" : "ReservedBy");
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
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
				this.$disposing = true;
				this.$PC = -1;
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
				Building_Bed.<GetFloatMenuOptions>c__Iterator2 <GetFloatMenuOptions>c__Iterator = new Building_Bed.<GetFloatMenuOptions>c__Iterator2();
				<GetFloatMenuOptions>c__Iterator.$this = this;
				<GetFloatMenuOptions>c__Iterator.myPawn = myPawn;
				return <GetFloatMenuOptions>c__Iterator;
			}

			private sealed class <GetFloatMenuOptions>c__AnonStorey4
			{
				internal Pawn myPawn;

				internal Building_Bed.<GetFloatMenuOptions>c__Iterator2 <>f__ref$2;

				public <GetFloatMenuOptions>c__AnonStorey4()
				{
				}

				internal void <>m__0()
				{
					if (!this.<>f__ref$2.$this.ForPrisoners && this.<>f__ref$2.$this.Medical && this.myPawn.CanReserveAndReach(this.<>f__ref$2.$this, PathEndMode.ClosestTouch, Danger.Deadly, this.<>f__ref$2.$this.SleepingSlotsCount, -1, null, true))
					{
						if (this.myPawn.CurJobDef == JobDefOf.LayDown && this.myPawn.CurJob.GetTarget(TargetIndex.A).Thing == this.<>f__ref$2.$this)
						{
							this.myPawn.CurJob.restUntilHealed = true;
						}
						else
						{
							Job job = new Job(JobDefOf.LayDown, this.<>f__ref$2.$this);
							job.restUntilHealed = true;
							this.myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
						}
						this.myPawn.mindState.ResetLastDisturbanceTick();
					}
				}
			}
		}
	}
}
