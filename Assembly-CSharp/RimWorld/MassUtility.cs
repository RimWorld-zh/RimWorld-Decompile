using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098F RID: 2447
	public static class MassUtility
	{
		// Token: 0x060036F7 RID: 14071 RVA: 0x001D5CF0 File Offset: 0x001D40F0
		public static float EncumbrancePercent(Pawn pawn)
		{
			return Mathf.Clamp01(MassUtility.UnboundedEncumbrancePercent(pawn));
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x001D5D10 File Offset: 0x001D4110
		public static float UnboundedEncumbrancePercent(Pawn pawn)
		{
			return MassUtility.GearAndInventoryMass(pawn) / MassUtility.Capacity(pawn, null);
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x001D5D34 File Offset: 0x001D4134
		public static bool IsOverEncumbered(Pawn pawn)
		{
			return MassUtility.UnboundedEncumbrancePercent(pawn) > 1f;
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x001D5D58 File Offset: 0x001D4158
		public static bool WillBeOverEncumberedAfterPickingUp(Pawn pawn, Thing thing, int count)
		{
			return MassUtility.FreeSpace(pawn) < (float)count * thing.GetStatValue(StatDefOf.Mass, true);
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x001D5D84 File Offset: 0x001D4184
		public static int CountToPickUpUntilOverEncumbered(Pawn pawn, Thing thing)
		{
			return Mathf.FloorToInt(MassUtility.FreeSpace(pawn) / thing.GetStatValue(StatDefOf.Mass, true));
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x001D5DB4 File Offset: 0x001D41B4
		public static float FreeSpace(Pawn pawn)
		{
			return Mathf.Max(MassUtility.Capacity(pawn, null) - MassUtility.GearAndInventoryMass(pawn), 0f);
		}

		// Token: 0x060036FD RID: 14077 RVA: 0x001D5DE4 File Offset: 0x001D41E4
		public static float GearAndInventoryMass(Pawn pawn)
		{
			return MassUtility.GearMass(pawn) + MassUtility.InventoryMass(pawn);
		}

		// Token: 0x060036FE RID: 14078 RVA: 0x001D5E08 File Offset: 0x001D4208
		public static float GearMass(Pawn p)
		{
			float num = 0f;
			if (p.apparel != null)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					num += wornApparel[i].GetStatValue(StatDefOf.Mass, true);
				}
			}
			if (p.equipment != null)
			{
				foreach (ThingWithComps thing in p.equipment.AllEquipmentListForReading)
				{
					num += thing.GetStatValue(StatDefOf.Mass, true);
				}
			}
			return num;
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x001D5EDC File Offset: 0x001D42DC
		public static float InventoryMass(Pawn p)
		{
			float num = 0f;
			for (int i = 0; i < p.inventory.innerContainer.Count; i++)
			{
				Thing thing = p.inventory.innerContainer[i];
				num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Mass, true);
			}
			return num;
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x001D5F44 File Offset: 0x001D4344
		public static float Capacity(Pawn p, StringBuilder explanation = null)
		{
			float result;
			if (!MassUtility.CanEverCarryAnything(p))
			{
				result = 0f;
			}
			else
			{
				float num = p.BodySize * 35f;
				if (explanation != null)
				{
					if (explanation.Length > 0)
					{
						explanation.AppendLine();
					}
					explanation.Append("  - " + p.LabelShortCap + ": " + num.ToStringMassOffset());
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x001D5FBC File Offset: 0x001D43BC
		public static bool CanEverCarryAnything(Pawn p)
		{
			return p.RaceProps.ToolUser || p.RaceProps.packAnimal;
		}

		// Token: 0x0400237D RID: 9085
		public const float MassCapacityPerBodySize = 35f;
	}
}
