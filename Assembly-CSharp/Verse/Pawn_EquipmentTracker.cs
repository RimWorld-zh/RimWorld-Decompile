using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D59 RID: 3417
	public class Pawn_EquipmentTracker : IThingHolder, IExposable
	{
		// Token: 0x06004C36 RID: 19510 RVA: 0x0027B075 File Offset: 0x00279475
		public Pawn_EquipmentTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.equipment = new ThingOwner<ThingWithComps>(this);
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06004C37 RID: 19511 RVA: 0x0027B094 File Offset: 0x00279494
		// (set) Token: 0x06004C38 RID: 19512 RVA: 0x0027B0F8 File Offset: 0x002794F8
		public ThingWithComps Primary
		{
			get
			{
				for (int i = 0; i < this.equipment.Count; i++)
				{
					if (this.equipment[i].def.equipmentType == EquipmentType.Primary)
					{
						return this.equipment[i];
					}
				}
				return null;
			}
			private set
			{
				if (this.Primary != value)
				{
					if (value != null && value.def.equipmentType != EquipmentType.Primary)
					{
						Log.Error("Tried to set non-primary equipment as primary.", false);
					}
					else
					{
						if (this.Primary != null)
						{
							this.equipment.Remove(this.Primary);
						}
						if (value != null)
						{
							this.equipment.TryAdd(value, true);
						}
						if (this.pawn.drafter != null)
						{
							this.pawn.drafter.Notify_PrimaryWeaponChanged();
						}
					}
				}
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06004C39 RID: 19513 RVA: 0x0027B190 File Offset: 0x00279590
		public CompEquippable PrimaryEq
		{
			get
			{
				return (this.Primary == null) ? null : this.Primary.GetComp<CompEquippable>();
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06004C3A RID: 19514 RVA: 0x0027B1C4 File Offset: 0x002795C4
		public List<ThingWithComps> AllEquipmentListForReading
		{
			get
			{
				return this.equipment.InnerListForReading;
			}
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x06004C3B RID: 19515 RVA: 0x0027B1E4 File Offset: 0x002795E4
		public IEnumerable<Verb> AllEquipmentVerbs
		{
			get
			{
				List<ThingWithComps> list = this.AllEquipmentListForReading;
				for (int i = 0; i < list.Count; i++)
				{
					ThingWithComps eq = list[i];
					List<Verb> verbs = eq.GetComp<CompEquippable>().AllVerbs;
					for (int j = 0; j < verbs.Count; j++)
					{
						yield return verbs[j];
					}
				}
				yield break;
			}
		}

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x06004C3C RID: 19516 RVA: 0x0027B210 File Offset: 0x00279610
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06004C3D RID: 19517 RVA: 0x0027B22C File Offset: 0x0027962C
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<ThingWithComps>>(ref this.equipment, "equipment", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					ThingWithComps thingWithComps = allEquipmentListForReading[i];
					foreach (Verb verb in thingWithComps.GetComp<CompEquippable>().AllVerbs)
					{
						verb.caster = this.pawn;
						verb.ownerEquipment = thingWithComps;
					}
				}
			}
		}

		// Token: 0x06004C3E RID: 19518 RVA: 0x0027B2F0 File Offset: 0x002796F0
		public void EquipmentTrackerTick()
		{
			List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
			for (int i = 0; i < allEquipmentListForReading.Count; i++)
			{
				ThingWithComps thingWithComps = allEquipmentListForReading[i];
				thingWithComps.GetComp<CompEquippable>().verbTracker.VerbsTick();
			}
		}

		// Token: 0x06004C3F RID: 19519 RVA: 0x0027B338 File Offset: 0x00279738
		public bool HasAnything()
		{
			return this.equipment.Any;
		}

		// Token: 0x06004C40 RID: 19520 RVA: 0x0027B358 File Offset: 0x00279758
		public void MakeRoomFor(ThingWithComps eq)
		{
			if (eq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				ThingWithComps thingWithComps;
				if (this.TryDropEquipment(this.Primary, out thingWithComps, this.pawn.Position, true))
				{
					if (thingWithComps != null)
					{
						thingWithComps.SetForbidden(false, true);
					}
				}
				else
				{
					Log.Error(this.pawn + " couldn't make room for equipment " + eq, false);
				}
			}
		}

		// Token: 0x06004C41 RID: 19521 RVA: 0x0027B3CF File Offset: 0x002797CF
		public void Remove(ThingWithComps eq)
		{
			this.equipment.Remove(eq);
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x0027B3E0 File Offset: 0x002797E0
		public bool TryDropEquipment(ThingWithComps eq, out ThingWithComps resultingEq, IntVec3 pos, bool forbid = true)
		{
			bool result;
			if (!pos.IsValid)
			{
				Log.Error(string.Concat(new object[]
				{
					this.pawn,
					" tried to drop ",
					eq,
					" at invalid cell."
				}), false);
				resultingEq = null;
				result = false;
			}
			else if (this.equipment.TryDrop(eq, pos, this.pawn.MapHeld, ThingPlaceMode.Near, out resultingEq, null, null))
			{
				if (resultingEq != null)
				{
					resultingEq.SetForbidden(forbid, false);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004C43 RID: 19523 RVA: 0x0027B478 File Offset: 0x00279878
		public void DropAllEquipment(IntVec3 pos, bool forbid = true)
		{
			for (int i = this.equipment.Count - 1; i >= 0; i--)
			{
				ThingWithComps thingWithComps;
				this.TryDropEquipment(this.equipment[i], out thingWithComps, pos, forbid);
			}
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x0027B4C0 File Offset: 0x002798C0
		public bool TryTransferEquipmentToContainer(ThingWithComps eq, ThingOwner container)
		{
			return this.equipment.TryTransferToContainer(eq, container, true);
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x0027B4E3 File Offset: 0x002798E3
		public void DestroyEquipment(ThingWithComps eq)
		{
			if (!this.equipment.Contains(eq))
			{
				Log.Warning("Tried to destroy equipment " + eq + " but it's not here.", false);
			}
			else
			{
				this.Remove(eq);
				eq.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06004C46 RID: 19526 RVA: 0x0027B521 File Offset: 0x00279921
		public void DestroyAllEquipment(DestroyMode mode = DestroyMode.Vanish)
		{
			this.equipment.ClearAndDestroyContents(mode);
		}

		// Token: 0x06004C47 RID: 19527 RVA: 0x0027B530 File Offset: 0x00279930
		public bool Contains(Thing eq)
		{
			return this.equipment.Contains(eq);
		}

		// Token: 0x06004C48 RID: 19528 RVA: 0x0027B551 File Offset: 0x00279951
		internal void Notify_PrimaryDestroyed()
		{
			if (this.Primary != null)
			{
				this.Remove(this.Primary);
			}
			if (this.pawn.Spawned)
			{
				this.pawn.stances.CancelBusyStanceSoft();
			}
		}

		// Token: 0x06004C49 RID: 19529 RVA: 0x0027B58C File Offset: 0x0027998C
		public void AddEquipment(ThingWithComps newEq)
		{
			if (newEq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Pawn ",
					this.pawn.LabelCap,
					" got primaryInt equipment ",
					newEq,
					" while already having primaryInt equipment ",
					this.Primary
				}), false);
			}
			else
			{
				this.equipment.TryAdd(newEq, true);
			}
		}

		// Token: 0x06004C4A RID: 19530 RVA: 0x0027B610 File Offset: 0x00279A10
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (PawnAttackGizmoUtility.CanShowEquipmentGizmos())
			{
				List<ThingWithComps> list = this.AllEquipmentListForReading;
				for (int i = 0; i < list.Count; i++)
				{
					ThingWithComps eq = list[i];
					foreach (Command command in eq.GetComp<CompEquippable>().GetVerbsCommands())
					{
						if (i != 0)
						{
							if (i != 1)
							{
								if (i == 2)
								{
									command.hotKey = KeyBindingDefOf.Misc3;
								}
							}
							else
							{
								command.hotKey = KeyBindingDefOf.Misc2;
							}
						}
						else
						{
							command.hotKey = KeyBindingDefOf.Misc1;
						}
						yield return command;
					}
				}
			}
			yield break;
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x0027B63C File Offset: 0x00279A3C
		public void Notify_EquipmentAdded(ThingWithComps eq)
		{
			foreach (Verb verb in eq.GetComp<CompEquippable>().AllVerbs)
			{
				verb.caster = this.pawn;
				verb.Notify_PickedUp();
			}
		}

		// Token: 0x06004C4C RID: 19532 RVA: 0x0027B6AC File Offset: 0x00279AAC
		public void Notify_EquipmentRemoved(ThingWithComps eq)
		{
			eq.GetComp<CompEquippable>().Notify_EquipmentLost();
		}

		// Token: 0x06004C4D RID: 19533 RVA: 0x0027B6BC File Offset: 0x00279ABC
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.equipment;
		}

		// Token: 0x06004C4E RID: 19534 RVA: 0x0027B6D7 File Offset: 0x00279AD7
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x040032FC RID: 13052
		private Pawn pawn;

		// Token: 0x040032FD RID: 13053
		private ThingOwner<ThingWithComps> equipment;
	}
}
