using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098B RID: 2443
	public static class MassUtility
	{
		// Token: 0x060036F2 RID: 14066 RVA: 0x001D5FB4 File Offset: 0x001D43B4
		public static float EncumbrancePercent(Pawn pawn)
		{
			return Mathf.Clamp01(MassUtility.UnboundedEncumbrancePercent(pawn));
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x001D5FD4 File Offset: 0x001D43D4
		public static float UnboundedEncumbrancePercent(Pawn pawn)
		{
			return MassUtility.GearAndInventoryMass(pawn) / MassUtility.Capacity(pawn, null);
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x001D5FF8 File Offset: 0x001D43F8
		public static bool IsOverEncumbered(Pawn pawn)
		{
			return MassUtility.UnboundedEncumbrancePercent(pawn) > 1f;
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x001D601C File Offset: 0x001D441C
		public static bool WillBeOverEncumberedAfterPickingUp(Pawn pawn, Thing thing, int count)
		{
			return MassUtility.FreeSpace(pawn) < (float)count * thing.GetStatValue(StatDefOf.Mass, true);
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x001D6048 File Offset: 0x001D4448
		public static int CountToPickUpUntilOverEncumbered(Pawn pawn, Thing thing)
		{
			return Mathf.FloorToInt(MassUtility.FreeSpace(pawn) / thing.GetStatValue(StatDefOf.Mass, true));
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x001D6078 File Offset: 0x001D4478
		public static float FreeSpace(Pawn pawn)
		{
			return Mathf.Max(MassUtility.Capacity(pawn, null) - MassUtility.GearAndInventoryMass(pawn), 0f);
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x001D60A8 File Offset: 0x001D44A8
		public static float GearAndInventoryMass(Pawn pawn)
		{
			return MassUtility.GearMass(pawn) + MassUtility.InventoryMass(pawn);
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x001D60CC File Offset: 0x001D44CC
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

		// Token: 0x060036FA RID: 14074 RVA: 0x001D61A0 File Offset: 0x001D45A0
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

		// Token: 0x060036FB RID: 14075 RVA: 0x001D6208 File Offset: 0x001D4608
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

		// Token: 0x060036FC RID: 14076 RVA: 0x001D6280 File Offset: 0x001D4680
		public static bool CanEverCarryAnything(Pawn p)
		{
			return p.RaceProps.ToolUser || p.RaceProps.packAnimal;
		}

		// Token: 0x0400237B RID: 9083
		public const float MassCapacityPerBodySize = 35f;
	}
}
