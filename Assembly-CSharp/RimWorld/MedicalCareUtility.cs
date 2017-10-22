using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class MedicalCareUtility
	{
		private static Texture2D[] careTextures;

		public const float CareSetterHeight = 28f;

		public const float CareSetterWidth = 140f;

		public static void Reset()
		{
			LongEventHandler.ExecuteWhenFinished((Action)delegate
			{
				MedicalCareUtility.careTextures = new Texture2D[5];
				MedicalCareUtility.careTextures[0] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoCare", true);
				MedicalCareUtility.careTextures[1] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoMeds", true);
				MedicalCareUtility.careTextures[2] = ThingDefOf.HerbalMedicine.uiIcon;
				MedicalCareUtility.careTextures[3] = ThingDefOf.Medicine.uiIcon;
				MedicalCareUtility.careTextures[4] = ThingDefOf.GlitterworldMedicine.uiIcon;
			});
		}

		public static void MedicalCareSetter(Rect rect, ref MedicalCareCategory medCare)
		{
			Rect rect2 = new Rect(rect.x, rect.y, (float)(rect.width / 5.0), rect.height);
			for (int i = 0; i < 5; i++)
			{
				MedicalCareCategory mc = (MedicalCareCategory)(byte)i;
				Widgets.DrawHighlightIfMouseover(rect2);
				GUI.DrawTexture(rect2, MedicalCareUtility.careTextures[i]);
				if (Widgets.ButtonInvisible(rect2, false))
				{
					medCare = mc;
					SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
				}
				if (medCare == mc)
				{
					Widgets.DrawBox(rect2, 3);
				}
				TooltipHandler.TipRegion(rect2, (Func<string>)(() => mc.GetLabel()), 632165 + i * 17);
				rect2.x += rect2.width;
			}
		}

		public static string GetLabel(this MedicalCareCategory cat)
		{
			return ("MedicalCareCategory_" + cat).Translate();
		}

		public static bool AllowsMedicine(this MedicalCareCategory cat, ThingDef meds)
		{
			bool result;
			switch (cat)
			{
			case MedicalCareCategory.NoCare:
			{
				result = false;
				break;
			}
			case MedicalCareCategory.NoMeds:
			{
				result = false;
				break;
			}
			case MedicalCareCategory.HerbalOrWorse:
			{
				result = (meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.HerbalMedicine.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
				break;
			}
			case MedicalCareCategory.NormalOrWorse:
			{
				result = (meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.Medicine.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
				break;
			}
			case MedicalCareCategory.Best:
			{
				result = true;
				break;
			}
			default:
			{
				throw new InvalidOperationException();
			}
			}
			return result;
		}
	}
}
