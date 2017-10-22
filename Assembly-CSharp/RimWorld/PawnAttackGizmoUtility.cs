using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class PawnAttackGizmoUtility
	{
		public static IEnumerable<Gizmo> GetAttackGizmos(Pawn pawn)
		{
			if (PawnAttackGizmoUtility.ShouldUseMeleeAttackGizmo(pawn))
			{
				yield return PawnAttackGizmoUtility.GetMeleeAttackGizmo(pawn);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!PawnAttackGizmoUtility.ShouldUseSquadAttackGizmo())
				yield break;
			yield return PawnAttackGizmoUtility.GetSquadAttackGizmo(pawn);
			/*Error: Unable to find new state assignment for yield return*/;
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
			string str = default(string);
			if ((object)FloatMenuUtility.GetAttackAction(pawn, LocalTargetInfo.Invalid, out str) == null)
			{
				command_Target.Disable(str.CapitalizeFirst() + ".");
			}
			command_Target.action = (Action<Thing>)delegate(Thing target)
			{
				IEnumerable<Pawn> enumerable = Find.Selector.SelectedObjects.Where((Func<object, bool>)delegate(object x)
				{
					Pawn pawn2 = x as Pawn;
					return pawn2 != null && pawn2.IsColonistPlayerControlled && pawn2.Drafted;
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
			string str = default(string);
			if ((object)FloatMenuUtility.GetMeleeAttackAction(pawn, LocalTargetInfo.Invalid, out str) == null)
			{
				command_Target.Disable(str.CapitalizeFirst() + ".");
			}
			command_Target.action = (Action<Thing>)delegate(Thing target)
			{
				IEnumerable<Pawn> enumerable = Find.Selector.SelectedObjects.Where((Func<object, bool>)delegate(object x)
				{
					Pawn pawn2 = x as Pawn;
					return pawn2 != null && pawn2.IsColonistPlayerControlled && pawn2.Drafted;
				}).Cast<Pawn>();
				foreach (Pawn item in enumerable)
				{
					string text = default(string);
					Action meleeAttackAction = FloatMenuUtility.GetMeleeAttackAction(item, target, out text);
					if ((object)meleeAttackAction != null)
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
			int num = 0;
			bool result;
			while (true)
			{
				if (num < selectedObjectsListForReading.Count)
				{
					Pawn pawn = selectedObjectsListForReading[num] as Pawn;
					if (pawn != null && pawn.IsColonistPlayerControlled && pawn.equipment != null && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
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

		private static bool AtLeastOneSelectedColonistHasNoWeapon()
		{
			List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < selectedObjectsListForReading.Count)
				{
					Pawn pawn = selectedObjectsListForReading[num] as Pawn;
					if (pawn != null && pawn.IsColonistPlayerControlled && (pawn.equipment == null || pawn.equipment.Primary == null))
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
						ThingDef thingDef2 = (pawn.equipment != null && pawn.equipment.Primary != null) ? pawn.equipment.Primary.def : null;
						if (!flag)
						{
							thingDef = thingDef2;
							flag = true;
						}
						else if (thingDef2 != thingDef)
							goto IL_00a9;
					}
				}
				result = false;
			}
			goto IL_00cb;
			IL_00cb:
			return result;
			IL_00a9:
			result = true;
			goto IL_00cb;
		}
	}
}
