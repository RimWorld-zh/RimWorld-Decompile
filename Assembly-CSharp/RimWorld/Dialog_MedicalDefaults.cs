using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000803 RID: 2051
	public class Dialog_MedicalDefaults : Window
	{
		// Token: 0x04001850 RID: 6224
		private const float MedicalCareStartX = 170f;

		// Token: 0x04001851 RID: 6225
		private const float VerticalGap = 6f;

		// Token: 0x04001852 RID: 6226
		private const float VerticalBigGap = 24f;

		// Token: 0x06002DCF RID: 11727 RVA: 0x001822A5 File Offset: 0x001806A5
		public Dialog_MedicalDefaults()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06002DD0 RID: 11728 RVA: 0x001822D4 File Offset: 0x001806D4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(346f, 350f);
			}
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x001822F8 File Offset: 0x001806F8
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
	}
}
