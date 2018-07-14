using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;

namespace Verse
{
	public class Pawn_EquipmentTracker : IThingHolder, IExposable
	{
		public Pawn pawn;

		private ThingOwner<ThingWithComps> equipment;

		public Pawn_EquipmentTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.equipment = new ThingOwner<ThingWithComps>(this);
		}

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

		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

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

		public void Remove(ThingWithComps eq)
		{
			this.equipment.Remove(eq);
		}

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

		public void DropAllEquipment(IntVec3 pos, bool forbid = true)
		{
			for (int i = this.equipment.Count - 1; i >= 0; i--)
			{
				ThingWithComps thingWithComps;
				this.TryDropEquipment(this.equipment[i], out thingWithComps, pos, forbid);
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
				Log.Warning("Tried to destroy equipment " + eq + " but it's not here.", false);
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

		public void Notify_EquipmentAdded(ThingWithComps eq)
		{
			foreach (Verb verb in eq.GetComp<CompEquippable>().AllVerbs)
			{
				verb.caster = this.pawn;
				verb.Notify_PickedUp();
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

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Verb>, IEnumerator, IDisposable, IEnumerator<Verb>
		{
			internal List<ThingWithComps> <list>__0;

			internal int <i>__1;

			internal ThingWithComps <eq>__2;

			internal List<Verb> <verbs>__2;

			internal int <j>__3;

			internal Pawn_EquipmentTracker $this;

			internal Verb $current;

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
					list = base.AllEquipmentListForReading;
					i = 0;
					goto IL_D9;
				case 1u:
					j++;
					break;
				default:
					return false;
				}
				IL_B4:
				if (j < verbs.Count)
				{
					this.$current = verbs[j];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				i++;
				IL_D9:
				if (i < list.Count)
				{
					eq = list[i];
					verbs = eq.GetComp<CompEquippable>().AllVerbs;
					j = 0;
					goto IL_B4;
				}
				this.$PC = -1;
				return false;
			}

			Verb IEnumerator<Verb>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Verb>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Verb> IEnumerable<Verb>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Pawn_EquipmentTracker.<>c__Iterator0 <>c__Iterator = new Pawn_EquipmentTracker.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator1 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal List<ThingWithComps> <list>__1;

			internal int <i>__2;

			internal ThingWithComps <eq>__3;

			internal IEnumerator<Command> $locvar0;

			internal Command <command>__4;

			internal Pawn_EquipmentTracker $this;

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
					if (!PawnAttackGizmoUtility.CanShowEquipmentGizmos())
					{
						goto IL_186;
					}
					list = base.AllEquipmentListForReading;
					i = 0;
					break;
				case 1u:
					Block_3:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							command = enumerator.Current;
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
							this.$current = command;
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
					i++;
					break;
				default:
					return false;
				}
				if (i < list.Count)
				{
					eq = list[i];
					enumerator = eq.GetComp<CompEquippable>().GetVerbsCommands().GetEnumerator();
					num = 4294967293u;
					goto Block_3;
				}
				IL_186:
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
				Pawn_EquipmentTracker.<GetGizmos>c__Iterator1 <GetGizmos>c__Iterator = new Pawn_EquipmentTracker.<GetGizmos>c__Iterator1();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}
		}
	}
}
