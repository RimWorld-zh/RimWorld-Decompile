using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public static class PawnAttackGizmoUtility
	{
		[CompilerGenerated]
		private static Action<Thing> <>f__am$cache0;

		[CompilerGenerated]
		private static Action<Thing> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<object, bool> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<object, bool> <>f__am$cache3;

		public static IEnumerable<Gizmo> GetAttackGizmos(Pawn pawn)
		{
			if (PawnAttackGizmoUtility.ShouldUseMeleeAttackGizmo(pawn))
			{
				yield return PawnAttackGizmoUtility.GetMeleeAttackGizmo(pawn);
			}
			if (PawnAttackGizmoUtility.ShouldUseSquadAttackGizmo())
			{
				yield return PawnAttackGizmoUtility.GetSquadAttackGizmo(pawn);
			}
			yield break;
		}

		public static bool CanShowEquipmentGizmos()
		{
			return !PawnAttackGizmoUtility.AtLeastTwoSelectedColonistsHaveDifferentWeapons();
		}

		private static bool ShouldUseSquadAttackGizmo()
		{
			return PawnAttackGizmoUtility.AtLeastOneSelectedColonistHasRangedWeapon() && PawnAttackGizmoUtility.AtLeastTwoSelectedColonistsHaveDifferentWeapons();
		}

		private static Gizmo GetSquadAttackGizmo(Pawn pawn)
		{
			Command_Target command_Target = new Command_Target();
			command_Target.defaultLabel = "CommandSquadAttack".Translate();
			command_Target.defaultDesc = "CommandSquadAttackDesc".Translate();
			command_Target.targetingParams = TargetingParameters.ForAttackAny();
			command_Target.hotKey = KeyBindingDefOf.Misc1;
			command_Target.icon = TexCommand.SquadAttack;
			string str;
			if (FloatMenuUtility.GetAttackAction(pawn, LocalTargetInfo.Invalid, out str) == null)
			{
				command_Target.Disable(str.CapitalizeFirst() + ".");
			}
			command_Target.action = delegate(Thing target)
			{
				IEnumerable<Pawn> enumerable = Find.Selector.SelectedObjects.Where(delegate(object x)
				{
					Pawn pawn3 = x as Pawn;
					return pawn3 != null && pawn3.IsColonistPlayerControlled && pawn3.Drafted;
				}).Cast<Pawn>();
				foreach (Pawn pawn2 in enumerable)
				{
					string text;
					Action attackAction = FloatMenuUtility.GetAttackAction(pawn2, target, out text);
					if (attackAction != null)
					{
						attackAction();
					}
				}
			};
			return command_Target;
		}

		private static bool ShouldUseMeleeAttackGizmo(Pawn pawn)
		{
			return pawn.Drafted && (PawnAttackGizmoUtility.AtLeastOneSelectedColonistHasRangedWeapon() || PawnAttackGizmoUtility.AtLeastOneSelectedColonistHasNoWeapon() || PawnAttackGizmoUtility.AtLeastTwoSelectedColonistsHaveDifferentWeapons());
		}

		private static Gizmo GetMeleeAttackGizmo(Pawn pawn)
		{
			Command_Target command_Target = new Command_Target();
			command_Target.defaultLabel = "CommandMeleeAttack".Translate();
			command_Target.defaultDesc = "CommandMeleeAttackDesc".Translate();
			command_Target.targetingParams = TargetingParameters.ForAttackAny();
			command_Target.hotKey = KeyBindingDefOf.Misc2;
			command_Target.icon = TexCommand.AttackMelee;
			string str;
			if (FloatMenuUtility.GetMeleeAttackAction(pawn, LocalTargetInfo.Invalid, out str) == null)
			{
				command_Target.Disable(str.CapitalizeFirst() + ".");
			}
			command_Target.action = delegate(Thing target)
			{
				IEnumerable<Pawn> enumerable = Find.Selector.SelectedObjects.Where(delegate(object x)
				{
					Pawn pawn3 = x as Pawn;
					return pawn3 != null && pawn3.IsColonistPlayerControlled && pawn3.Drafted;
				}).Cast<Pawn>();
				foreach (Pawn pawn2 in enumerable)
				{
					string text;
					Action meleeAttackAction = FloatMenuUtility.GetMeleeAttackAction(pawn2, target, out text);
					if (meleeAttackAction != null)
					{
						meleeAttackAction();
					}
				}
			};
			return command_Target;
		}

		private static bool AtLeastOneSelectedColonistHasRangedWeapon()
		{
			List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
			for (int i = 0; i < selectedObjectsListForReading.Count; i++)
			{
				Pawn pawn = selectedObjectsListForReading[i] as Pawn;
				if (pawn != null && pawn.IsColonistPlayerControlled)
				{
					if (pawn.equipment != null && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool AtLeastOneSelectedColonistHasNoWeapon()
		{
			List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
			for (int i = 0; i < selectedObjectsListForReading.Count; i++)
			{
				Pawn pawn = selectedObjectsListForReading[i] as Pawn;
				if (pawn != null && pawn.IsColonistPlayerControlled)
				{
					if (pawn.equipment == null || pawn.equipment.Primary == null)
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool AtLeastTwoSelectedColonistsHaveDifferentWeapons()
		{
			bool result;
			if (Find.Selector.NumSelected <= 1)
			{
				result = false;
			}
			else
			{
				ThingDef thingDef = null;
				bool flag = false;
				List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
				for (int i = 0; i < selectedObjectsListForReading.Count; i++)
				{
					Pawn pawn = selectedObjectsListForReading[i] as Pawn;
					if (pawn != null && pawn.IsColonistPlayerControlled)
					{
						ThingDef thingDef2;
						if (pawn.equipment == null || pawn.equipment.Primary == null)
						{
							thingDef2 = null;
						}
						else
						{
							thingDef2 = pawn.equipment.Primary.def;
						}
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
				result = false;
			}
			return result;
		}

		[CompilerGenerated]
		private static void <GetSquadAttackGizmo>m__0(Thing target)
		{
			IEnumerable<Pawn> enumerable = Find.Selector.SelectedObjects.Where(delegate(object x)
			{
				Pawn pawn2 = x as Pawn;
				return pawn2 != null && pawn2.IsColonistPlayerControlled && pawn2.Drafted;
			}).Cast<Pawn>();
			foreach (Pawn pawn in enumerable)
			{
				string text;
				Action attackAction = FloatMenuUtility.GetAttackAction(pawn, target, out text);
				if (attackAction != null)
				{
					attackAction();
				}
			}
		}

		[CompilerGenerated]
		private static void <GetMeleeAttackGizmo>m__1(Thing target)
		{
			IEnumerable<Pawn> enumerable = Find.Selector.SelectedObjects.Where(delegate(object x)
			{
				Pawn pawn2 = x as Pawn;
				return pawn2 != null && pawn2.IsColonistPlayerControlled && pawn2.Drafted;
			}).Cast<Pawn>();
			foreach (Pawn pawn in enumerable)
			{
				string text;
				Action meleeAttackAction = FloatMenuUtility.GetMeleeAttackAction(pawn, target, out text);
				if (meleeAttackAction != null)
				{
					meleeAttackAction();
				}
			}
		}

		[CompilerGenerated]
		private static bool <GetSquadAttackGizmo>m__2(object x)
		{
			Pawn pawn = x as Pawn;
			return pawn != null && pawn.IsColonistPlayerControlled && pawn.Drafted;
		}

		[CompilerGenerated]
		private static bool <GetMeleeAttackGizmo>m__3(object x)
		{
			Pawn pawn = x as Pawn;
			return pawn != null && pawn.IsColonistPlayerControlled && pawn.Drafted;
		}

		[CompilerGenerated]
		private sealed class <GetAttackGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal Pawn pawn;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetAttackGizmos>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (PawnAttackGizmoUtility.ShouldUseMeleeAttackGizmo(pawn))
					{
						this.$current = PawnAttackGizmoUtility.GetMeleeAttackGizmo(pawn);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_8A;
				default:
					return false;
				}
				if (PawnAttackGizmoUtility.ShouldUseSquadAttackGizmo())
				{
					this.$current = PawnAttackGizmoUtility.GetSquadAttackGizmo(pawn);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_8A:
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
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PawnAttackGizmoUtility.<GetAttackGizmos>c__Iterator0 <GetAttackGizmos>c__Iterator = new PawnAttackGizmoUtility.<GetAttackGizmos>c__Iterator0();
				<GetAttackGizmos>c__Iterator.pawn = pawn;
				return <GetAttackGizmos>c__Iterator;
			}
		}
	}
}
