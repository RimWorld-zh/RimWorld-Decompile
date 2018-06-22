using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200046C RID: 1132
	public static class PawnAttackGizmoUtility
	{
		// Token: 0x060013E3 RID: 5091 RVA: 0x000AD92C File Offset: 0x000ABD2C
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

		// Token: 0x060013E4 RID: 5092 RVA: 0x000AD958 File Offset: 0x000ABD58
		public static bool CanShowEquipmentGizmos()
		{
			return !PawnAttackGizmoUtility.AtLeastTwoSelectedColonistsHaveDifferentWeapons();
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x000AD978 File Offset: 0x000ABD78
		private static bool ShouldUseSquadAttackGizmo()
		{
			return PawnAttackGizmoUtility.AtLeastOneSelectedColonistHasRangedWeapon() && PawnAttackGizmoUtility.AtLeastTwoSelectedColonistsHaveDifferentWeapons();
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x000AD9A0 File Offset: 0x000ABDA0
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

		// Token: 0x060013E7 RID: 5095 RVA: 0x000ADA48 File Offset: 0x000ABE48
		private static bool ShouldUseMeleeAttackGizmo(Pawn pawn)
		{
			return pawn.Drafted && (PawnAttackGizmoUtility.AtLeastOneSelectedColonistHasRangedWeapon() || PawnAttackGizmoUtility.AtLeastOneSelectedColonistHasNoWeapon() || PawnAttackGizmoUtility.AtLeastTwoSelectedColonistsHaveDifferentWeapons());
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x000ADA8C File Offset: 0x000ABE8C
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

		// Token: 0x060013E9 RID: 5097 RVA: 0x000ADB34 File Offset: 0x000ABF34
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

		// Token: 0x060013EA RID: 5098 RVA: 0x000ADBD0 File Offset: 0x000ABFD0
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

		// Token: 0x060013EB RID: 5099 RVA: 0x000ADC50 File Offset: 0x000AC050
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
	}
}
