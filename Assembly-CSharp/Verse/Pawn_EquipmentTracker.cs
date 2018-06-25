using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D57 RID: 3415
	public class Pawn_EquipmentTracker : IThingHolder, IExposable
	{
		// Token: 0x04003305 RID: 13061
		private Pawn pawn;

		// Token: 0x04003306 RID: 13062
		private ThingOwner<ThingWithComps> equipment;

		// Token: 0x06004C4C RID: 19532 RVA: 0x0027C71D File Offset: 0x0027AB1D
		public Pawn_EquipmentTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.equipment = new ThingOwner<ThingWithComps>(this);
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06004C4D RID: 19533 RVA: 0x0027C73C File Offset: 0x0027AB3C
		// (set) Token: 0x06004C4E RID: 19534 RVA: 0x0027C7A0 File Offset: 0x0027ABA0
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
		// (get) Token: 0x06004C4F RID: 19535 RVA: 0x0027C838 File Offset: 0x0027AC38
		public CompEquippable PrimaryEq
		{
			get
			{
				return (this.Primary == null) ? null : this.Primary.GetComp<CompEquippable>();
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06004C50 RID: 19536 RVA: 0x0027C86C File Offset: 0x0027AC6C
		public List<ThingWithComps> AllEquipmentListForReading
		{
			get
			{
				return this.equipment.InnerListForReading;
			}
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x06004C51 RID: 19537 RVA: 0x0027C88C File Offset: 0x0027AC8C
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
		// (get) Token: 0x06004C52 RID: 19538 RVA: 0x0027C8B8 File Offset: 0x0027ACB8
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06004C53 RID: 19539 RVA: 0x0027C8D4 File Offset: 0x0027ACD4
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

		// Token: 0x06004C54 RID: 19540 RVA: 0x0027C998 File Offset: 0x0027AD98
		public void EquipmentTrackerTick()
		{
			List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
			for (int i = 0; i < allEquipmentListForReading.Count; i++)
			{
				ThingWithComps thingWithComps = allEquipmentListForReading[i];
				thingWithComps.GetComp<CompEquippable>().verbTracker.VerbsTick();
			}
		}

		// Token: 0x06004C55 RID: 19541 RVA: 0x0027C9E0 File Offset: 0x0027ADE0
		public bool HasAnything()
		{
			return this.equipment.Any;
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x0027CA00 File Offset: 0x0027AE00
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

		// Token: 0x06004C57 RID: 19543 RVA: 0x0027CA77 File Offset: 0x0027AE77
		public void Remove(ThingWithComps eq)
		{
			this.equipment.Remove(eq);
		}

		// Token: 0x06004C58 RID: 19544 RVA: 0x0027CA88 File Offset: 0x0027AE88
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

		// Token: 0x06004C59 RID: 19545 RVA: 0x0027CB20 File Offset: 0x0027AF20
		public void DropAllEquipment(IntVec3 pos, bool forbid = true)
		{
			for (int i = this.equipment.Count - 1; i >= 0; i--)
			{
				ThingWithComps thingWithComps;
				this.TryDropEquipment(this.equipment[i], out thingWithComps, pos, forbid);
			}
		}

		// Token: 0x06004C5A RID: 19546 RVA: 0x0027CB68 File Offset: 0x0027AF68
		public bool TryTransferEquipmentToContainer(ThingWithComps eq, ThingOwner container)
		{
			return this.equipment.TryTransferToContainer(eq, container, true);
		}

		// Token: 0x06004C5B RID: 19547 RVA: 0x0027CB8B File Offset: 0x0027AF8B
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

		// Token: 0x06004C5C RID: 19548 RVA: 0x0027CBC9 File Offset: 0x0027AFC9
		public void DestroyAllEquipment(DestroyMode mode = DestroyMode.Vanish)
		{
			this.equipment.ClearAndDestroyContents(mode);
		}

		// Token: 0x06004C5D RID: 19549 RVA: 0x0027CBD8 File Offset: 0x0027AFD8
		public bool Contains(Thing eq)
		{
			return this.equipment.Contains(eq);
		}

		// Token: 0x06004C5E RID: 19550 RVA: 0x0027CBF9 File Offset: 0x0027AFF9
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

		// Token: 0x06004C5F RID: 19551 RVA: 0x0027CC34 File Offset: 0x0027B034
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

		// Token: 0x06004C60 RID: 19552 RVA: 0x0027CCB8 File Offset: 0x0027B0B8
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

		// Token: 0x06004C61 RID: 19553 RVA: 0x0027CCE4 File Offset: 0x0027B0E4
		public void Notify_EquipmentAdded(ThingWithComps eq)
		{
			foreach (Verb verb in eq.GetComp<CompEquippable>().AllVerbs)
			{
				verb.caster = this.pawn;
				verb.Notify_PickedUp();
			}
		}

		// Token: 0x06004C62 RID: 19554 RVA: 0x0027CD54 File Offset: 0x0027B154
		public void Notify_EquipmentRemoved(ThingWithComps eq)
		{
			eq.GetComp<CompEquippable>().Notify_EquipmentLost();
		}

		// Token: 0x06004C63 RID: 19555 RVA: 0x0027CD64 File Offset: 0x0027B164
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.equipment;
		}

		// Token: 0x06004C64 RID: 19556 RVA: 0x0027CD7F File Offset: 0x0027B17F
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}
	}
}
