using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse
{
	public class Pawn_EquipmentTracker : IThingHolder, IExposable
	{
		private Pawn pawn;

		private ThingOwner<ThingWithComps> equipment;

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
						Log.Error("Tried to set non-primary equipment as primary.");
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

		public CompEquippable PrimaryEq
		{
			get
			{
				return (this.Primary == null) ? null : this.Primary.GetComp<CompEquippable>();
			}
		}

		public List<ThingWithComps> AllEquipmentListForReading
		{
			get
			{
				return this.equipment.InnerListForReading;
			}
		}

		public IEnumerable<Verb> AllEquipmentVerbs
		{
			get
			{
				List<ThingWithComps> list = this.AllEquipmentListForReading;
				int j = 0;
				List<Verb> verbs;
				int i;
				while (true)
				{
					if (j < list.Count)
					{
						ThingWithComps eq = list[j];
						verbs = eq.GetComp<CompEquippable>().AllVerbs;
						i = 0;
						if (i < verbs.Count)
							break;
						j++;
						continue;
					}
					yield break;
				}
				yield return verbs[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		public Pawn_EquipmentTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.equipment = new ThingOwner<ThingWithComps>(this);
		}

		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<ThingWithComps>>(ref this.equipment, "equipment", new object[1]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					ThingWithComps thingWithComps = allEquipmentListForReading[i];
					foreach (Verb allVerb in thingWithComps.GetComp<CompEquippable>().AllVerbs)
					{
						allVerb.caster = this.pawn;
						allVerb.ownerEquipment = thingWithComps;
					}
				}
			}
		}

		public void EquipmentTrackerTick()
		{
			List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
			for (int i = 0; i < allEquipmentListForReading.Count; i++)
			{
				ThingWithComps thingWithComps = allEquipmentListForReading[i];
				thingWithComps.GetComp<CompEquippable>().verbTracker.VerbsTick();
			}
		}

		public bool HasAnything()
		{
			return this.equipment.Any;
		}

		public void MakeRoomFor(ThingWithComps eq)
		{
			if (eq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				ThingWithComps thingWithComps = default(ThingWithComps);
				if (this.TryDropEquipment(this.Primary, out thingWithComps, this.pawn.Position, true))
				{
					if (thingWithComps != null)
					{
						thingWithComps.SetForbidden(false, true);
					}
				}
				else
				{
					Log.Error(this.pawn + " couldn't make room for equipment " + eq);
				}
			}
		}

		public void Remove(ThingWithComps eq)
		{
			this.equipment.Remove(eq);
		}

		public bool TryDropEquipment(ThingWithComps eq, out ThingWithComps resultingEq, IntVec3 pos, bool forbid = true)
		{
			if (!pos.IsValid)
			{
				Log.Error(this.pawn + " tried to drop " + eq + " at invalid cell.");
				resultingEq = null;
				return false;
			}
			if (this.equipment.TryDrop((Thing)eq, pos, this.pawn.MapHeld, ThingPlaceMode.Near, out resultingEq, (Action<ThingWithComps, int>)null))
			{
				if (resultingEq != null)
				{
					resultingEq.SetForbidden(forbid, false);
				}
				return true;
			}
			return false;
		}

		public void DropAllEquipment(IntVec3 pos, bool forbid = true)
		{
			for (int num = this.equipment.Count - 1; num >= 0; num--)
			{
				ThingWithComps thingWithComps = default(ThingWithComps);
				this.TryDropEquipment(this.equipment[num], out thingWithComps, pos, forbid);
			}
		}

		public bool TryTransferEquipmentToContainer(ThingWithComps eq, ThingOwner container)
		{
			return this.equipment.TryTransferToContainer(eq, container, true);
		}

		public void DestroyEquipment(ThingWithComps eq)
		{
			if (!this.equipment.Contains(eq))
			{
				Log.Warning("Tried to destroy equipment " + eq + " but it's not here.");
			}
			else
			{
				this.Remove(eq);
				eq.Destroy(DestroyMode.Vanish);
			}
		}

		public void DestroyAllEquipment(DestroyMode mode = DestroyMode.Vanish)
		{
			this.equipment.ClearAndDestroyContents(mode);
		}

		public bool Contains(Thing eq)
		{
			return this.equipment.Contains(eq);
		}

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

		public void AddEquipment(ThingWithComps newEq)
		{
			if (newEq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				Log.Error("Pawn " + this.pawn.LabelCap + " got primaryInt equipment " + newEq + " while already having primaryInt equipment " + this.Primary);
			}
			else
			{
				this.equipment.TryAdd(newEq, true);
			}
		}

		public IEnumerable<Gizmo> GetGizmos()
		{
			if (PawnAttackGizmoUtility.CanShowEquipmentGizmos())
			{
				List<ThingWithComps> list = this.AllEquipmentListForReading;
				for (int i = 0; i < list.Count; i++)
				{
					ThingWithComps eq = list[i];
					using (IEnumerator<Command> enumerator = eq.GetComp<CompEquippable>().GetVerbsCommands().GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							Command command = enumerator.Current;
							switch (i)
							{
							case 0:
								command.hotKey = KeyBindingDefOf.Misc1;
								break;
							case 1:
								command.hotKey = KeyBindingDefOf.Misc2;
								break;
							case 2:
								command.hotKey = KeyBindingDefOf.Misc3;
								break;
							}
							yield return (Gizmo)command;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			yield break;
			IL_0187:
			/*Error near IL_0188: Unexpected return in MoveNext()*/;
		}

		public void Notify_EquipmentAdded(ThingWithComps eq)
		{
			foreach (Verb allVerb in eq.GetComp<CompEquippable>().AllVerbs)
			{
				allVerb.caster = this.pawn;
				allVerb.Notify_PickedUp();
			}
		}

		public void Notify_EquipmentRemoved(ThingWithComps eq)
		{
			eq.GetComp<CompEquippable>().Notify_EquipmentLost();
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return this.equipment;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}
	}
}
