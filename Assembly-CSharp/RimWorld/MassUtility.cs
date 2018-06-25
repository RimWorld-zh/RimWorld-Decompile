using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098D RID: 2445
	public static class MassUtility
	{
		// Token: 0x04002383 RID: 9091
		public const float MassCapacityPerBodySize = 35f;

		// Token: 0x060036F6 RID: 14070 RVA: 0x001D63C8 File Offset: 0x001D47C8
		public static float EncumbrancePercent(Pawn pawn)
		{
			return Mathf.Clamp01(MassUtility.UnboundedEncumbrancePercent(pawn));
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x001D63E8 File Offset: 0x001D47E8
		public static float UnboundedEncumbrancePercent(Pawn pawn)
		{
			return MassUtility.GearAndInventoryMass(pawn) / MassUtility.Capacity(pawn, null);
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x001D640C File Offset: 0x001D480C
		public static bool IsOverEncumbered(Pawn pawn)
		{
			return MassUtility.UnboundedEncumbrancePercent(pawn) > 1f;
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x001D6430 File Offset: 0x001D4830
		public static bool WillBeOverEncumberedAfterPickingUp(Pawn pawn, Thing thing, int count)
		{
			return MassUtility.FreeSpace(pawn) < (float)count * thing.GetStatValue(StatDefOf.Mass, true);
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x001D645C File Offset: 0x001D485C
		public static int CountToPickUpUntilOverEncumbered(Pawn pawn, Thing thing)
		{
			return Mathf.FloorToInt(MassUtility.FreeSpace(pawn) / thing.GetStatValue(StatDefOf.Mass, true));
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x001D648C File Offset: 0x001D488C
		public static float FreeSpace(Pawn pawn)
		{
			return Mathf.Max(MassUtility.Capacity(pawn, null) - MassUtility.GearAndInventoryMass(pawn), 0f);
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x001D64BC File Offset: 0x001D48BC
		public static float GearAndInventoryMass(Pawn pawn)
		{
			return MassUtility.GearMass(pawn) + MassUtility.InventoryMass(pawn);
		}

		// Token: 0x060036FD RID: 14077 RVA: 0x001D64E0 File Offset: 0x001D48E0
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

		// Token: 0x060036FE RID: 14078 RVA: 0x001D65B4 File Offset: 0x001D49B4
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

		// Token: 0x060036FF RID: 14079 RVA: 0x001D661C File Offset: 0x001D4A1C
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

		// Token: 0x06003700 RID: 14080 RVA: 0x001D6694 File Offset: 0x001D4A94
		public static bool CanEverCarryAnything(Pawn p)
		{
			return p.RaceProps.ToolUser || p.RaceProps.packAnimal;
		}
	}
}
