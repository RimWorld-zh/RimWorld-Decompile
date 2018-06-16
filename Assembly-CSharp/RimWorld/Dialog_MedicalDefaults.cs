using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000805 RID: 2053
	public class Dialog_MedicalDefaults : Window
	{
		// Token: 0x06002DD1 RID: 11729 RVA: 0x00181C85 File Offset: 0x00180085
		public Dialog_MedicalDefaults()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06002DD2 RID: 11730 RVA: 0x00181CB4 File Offset: 0x001800B4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(346f, 350f);
			}
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x00181CD8 File Offset: 0x001800D8
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, 170f, 28f);
			Rect rect2 = new Rect(170f, 0f, 140f, 28f);
			Widgets.Label(rect, "MedGroupColonist".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForColonyHumanlike);
			rect.y += 34f;
			rect2.y += 34f;
			Widgets.Label(rect, "MedGroupImprisonedColonist".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForColonyPrisoner);
			rect.y += 34f;
			rect2.y += 34f;
			Widgets.Label(rect, "MedGroupColonyAnimal".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForColonyAnimal);
			rect.y += 52f;
			rect2.y += 52f;
			Widgets.Label(rect, "MedGroupNeutralAnimal".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForNeutralAnimal);
			rect.y += 34f;
			rect2.y += 34f;
			Widgets.Label(rect, "MedGroupNeutralFaction".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForNeutralFaction);
			rect.y += 52f;
			rect2.y += 52f;
			Widgets.Label(rect, "MedGroupHostileFaction".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForHostileFaction);
		}

		// Token: 0x0400184E RID: 6222
		private const float MedicalCareStartX = 170f;

		// Token: 0x0400184F RID: 6223
		private const float VerticalGap = 6f;

		// Token: 0x04001850 RID: 6224
		private const float VerticalBigGap = 24f;
	}
}
