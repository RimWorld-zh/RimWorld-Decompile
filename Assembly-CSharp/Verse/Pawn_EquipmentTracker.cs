using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class Pawn_EquipmentTracker : IExposable, IThingHolder
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
				for (int j = 0; j < list.Count; j++)
				{
					ThingWithComps eq = list[j];
					List<Verb> verbs = eq.GetComp<CompEquippable>().AllVerbs;
					for (int i = 0; i < verbs.Count; i++)
					{
						yield return verbs[i];
					}
				}
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
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					ThingWithComps thingWithComps = allEquipmentListForReading[i];
					List<Verb>.Enumerator enumerator = thingWithComps.GetComp<CompEquippable>().AllVerbs.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Verb current = enumerator.Current;
							current.caster = this.pawn;
							current.ownerEquipment = thingWithComps;
						}
					}
					finally
					{
						((IDisposable)(object)enumerator).Dispose();
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
			if (this.ShouldUseSquadAttackGizmo())
			{
				yield return this.GetSquadAttackGizmo();
			}
			else
			{
				List<ThingWithComps> list = this.AllEquipmentListForReading;
				for (int i = 0; i < list.Count; i++)
				{
					ThingWithComps eq = list[i];
					foreach (Command verbsCommand in eq.GetComp<CompEquippable>().GetVerbsCommands())
					{
						switch (i)
						{
						case 0:
						{
							verbsCommand.hotKey = KeyBindingDefOf.Misc1;
							break;
						}
						case 1:
						{
							verbsCommand.hotKey = KeyBindingDefOf.Misc2;
							break;
						}
						case 2:
						{
							verbsCommand.hotKey = KeyBindingDefOf.Misc3;
							break;
						}
						}
						yield return (Gizmo)verbsCommand;
					}
				}
			}
		}

		public bool TryStartAttack(LocalTargetInfo targ)
		{
			if (this.pawn.stances.FullBodyBusy)
			{
				return false;
			}
			if (this.pawn.story != null && this.pawn.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				return false;
			}
			bool allowManualCastWeapons = !this.pawn.IsColonist;
			Verb verb = this.pawn.TryGetAttackVerb(allowManualCastWeapons);
			if (verb == null)
			{
				return false;
			}
			return verb.TryStartCastOn(targ, false, true);
		}

		private bool ShouldUseSquadAttackGizmo()
		{
			if (Find.Selector.NumSelected <= 1)
			{
				return false;
			}
			ThingDef thingDef = null;
			bool flag = false;
			List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
			for (int i = 0; i < selectedObjectsListForReading.Count; i++)
			{
				Pawn pawn = selectedObjectsListForReading[i] as Pawn;
				if (pawn != null && pawn.IsColonist)
				{
					ThingDef thingDef2 = (pawn.equipment.Primary != null) ? pawn.equipment.Primary.def : null;
					if (!flag)
					{
						thingDef = thingDef2;
						flag = true;
					}
					else if (thingDef2 != thingDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		private Gizmo GetSquadAttackGizmo()
		{
			Command_Target command_Target = new Command_Target();
			command_Target.defaultLabel = "CommandSquadAttack".Translate();
			command_Target.defaultDesc = "CommandSquadAttackDesc".Translate();
			command_Target.targetingParams = TargetingParameters.ForAttackAny();
			command_Target.hotKey = KeyBindingDefOf.Misc1;
			command_Target.icon = TexCommand.SquadAttack;
			string str = default(string);
			if ((object)FloatMenuUtility.GetAttackAction(this.pawn, LocalTargetInfo.Invalid, out str) == null)
			{
				command_Target.Disable(str.CapitalizeFirst() + ".");
			}
			command_Target.action = (Action<Thing>)delegate(Thing target)
			{
				IEnumerable<Pawn> enumerable = Find.Selector.SelectedObjects.Where((Func<object, bool>)delegate(object x)
				{
					Pawn pawn = x as Pawn;
					return pawn != null && pawn.IsColonistPlayerControlled && pawn.Drafted;
				}).Cast<Pawn>();
				foreach (Pawn item in enumerable)
				{
					string text = default(string);
					Action attackAction = FloatMenuUtility.GetAttackAction(item, target, out text);
					if ((object)attackAction != null)
					{
						attackAction();
					}
				}
			};
			return command_Target;
		}

		public void Notify_EquipmentAdded(ThingWithComps eq)
		{
			List<Verb>.Enumerator enumerator = eq.GetComp<CompEquippable>().AllVerbs.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Verb current = enumerator.Current;
					current.caster = this.pawn;
					current.Notify_PickedUp();
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
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
