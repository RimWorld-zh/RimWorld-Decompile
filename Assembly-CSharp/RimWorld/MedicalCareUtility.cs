using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200051E RID: 1310
	[StaticConstructorOnStartup]
	public static class MedicalCareUtility
	{
		// Token: 0x04000E19 RID: 3609
		private static Texture2D[] careTextures;

		// Token: 0x04000E1A RID: 3610
		public const float CareSetterHeight = 28f;

		// Token: 0x04000E1B RID: 3611
		public const float CareSetterWidth = 140f;

		// Token: 0x04000E1C RID: 3612
		private static bool medicalCarePainting = false;

		// Token: 0x04000E1E RID: 3614
		[CompilerGenerated]
		private static Func<Pawn, MedicalCareCategory> <>f__mg$cache0;

		// Token: 0x04000E1F RID: 3615
		[CompilerGenerated]
		private static Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>> <>f__mg$cache1;

		// Token: 0x060017E1 RID: 6113 RVA: 0x000D0DDD File Offset: 0x000CF1DD
		public static void Reset()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				MedicalCareUtility.careTextures = new Texture2D[5];
				MedicalCareUtility.careTextures[0] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoCare", true);
				MedicalCareUtility.careTextures[1] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoMeds", true);
				MedicalCareUtility.careTextures[2] = ThingDefOf.MedicineHerbal.uiIcon;
				MedicalCareUtility.careTextures[3] = ThingDefOf.MedicineIndustrial.uiIcon;
				MedicalCareUtility.careTextures[4] = ThingDefOf.MedicineUltratech.uiIcon;
			});
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x000D0E04 File Offset: 0x000CF204
		public static void MedicalCareSetter(Rect rect, ref MedicalCareCategory medCare)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width / 5f, rect.height);
			for (int i = 0; i < 5; i++)
			{
				MedicalCareCategory mc = (MedicalCareCategory)i;
				Widgets.DrawHighlightIfMouseover(rect2);
				GUI.DrawTexture(rect2, MedicalCareUtility.careTextures[i]);
				Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect2, false);
				if (draggableResult == Widgets.DraggableResult.Dragged)
				{
					MedicalCareUtility.medicalCarePainting = true;
				}
				if ((MedicalCareUtility.medicalCarePainting && Mouse.IsOver(rect2) && medCare != mc) || draggableResult.AnyPressed())
				{
					medCare = mc;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				if (medCare == mc)
				{
					Widgets.DrawBox(rect2, 3);
				}
				TooltipHandler.TipRegion(rect2, () => mc.GetLabel(), 632165 + i * 17);
				rect2.x += rect2.width;
			}
			if (!Input.GetMouseButton(0))
			{
				MedicalCareUtility.medicalCarePainting = false;
			}
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x000D0F20 File Offset: 0x000CF320
		public static string GetLabel(this MedicalCareCategory cat)
		{
			return ("MedicalCareCategory_" + cat).Translate();
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x000D0F4C File Offset: 0x000CF34C
		public static bool AllowsMedicine(this MedicalCareCategory cat, ThingDef meds)
		{
			bool result;
			switch (cat)
			{
			case MedicalCareCategory.NoCare:
				result = false;
				break;
			case MedicalCareCategory.NoMeds:
				result = false;
				break;
			case MedicalCareCategory.HerbalOrWorse:
				result = (meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.MedicineHerbal.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
				break;
			case MedicalCareCategory.NormalOrWorse:
				result = (meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.MedicineIndustrial.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
				break;
			case MedicalCareCategory.Best:
				result = true;
				break;
			default:
				throw new InvalidOperationException();
			}
			return result;
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x000D0FE4 File Offset: 0x000CF3E4
		public static void MedicalCareSelectButton(Rect rect, Pawn pawn)
		{
			if (MedicalCareUtility.<>f__mg$cache0 == null)
			{
				MedicalCareUtility.<>f__mg$cache0 = new Func<Pawn, MedicalCareCategory>(MedicalCareUtility.MedicalCareSelectButton_GetMedicalCare);
			}
			Func<Pawn, MedicalCareCategory> getPayload = MedicalCareUtility.<>f__mg$cache0;
			if (MedicalCareUtility.<>f__mg$cache1 == null)
			{
				MedicalCareUtility.<>f__mg$cache1 = new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>>(MedicalCareUtility.MedicalCareSelectButton_GenerateMenu);
			}
			Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>> menuGenerator = MedicalCareUtility.<>f__mg$cache1;
			Texture2D buttonIcon = MedicalCareUtility.careTextures[(int)pawn.playerSettings.medCare];
			Widgets.Dropdown<Pawn, MedicalCareCategory>(rect, pawn, getPayload, menuGenerator, null, buttonIcon, null, null, null, true);
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x000D1058 File Offset: 0x000CF458
		private static MedicalCareCategory MedicalCareSelectButton_GetMedicalCare(Pawn pawn)
		{
			return pawn.playerSettings.medCare;
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x000D1078 File Offset: 0x000CF478
		private static IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>> MedicalCareSelectButton_GenerateMenu(Pawn p)
		{
			for (int i = 0; i < 5; i++)
			{
				MedicalCareCategory mc = (MedicalCareCategory)i;
				yield return new Widgets.DropdownMenuElement<MedicalCareCategory>
				{
					option = new FloatMenuOption(mc.GetLabel(), delegate()
					{
						p.playerSettings.medCare = mc;
					}, MenuOptionPriority.Default, null, null, 0f, null, null),
					payload = mc
				};
			}
			yield break;
		}
	}
}
