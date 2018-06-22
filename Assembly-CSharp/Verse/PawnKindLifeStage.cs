using System;

namespace Verse
{
	// Token: 0x02000B5B RID: 2907
	public class PawnKindLifeStage
	{
		// Token: 0x06003F89 RID: 16265 RVA: 0x00217A32 File Offset: 0x00215E32
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelMale = this.labelMale;
			this.untranslatedLabelFemale = this.labelFemale;
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x00217A5C File Offset: 0x00215E5C
		public void ResolveReferences()
		{
			if (this.bodyGraphicData != null && this.bodyGraphicData.graphicClass == null)
			{
				this.bodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleGraphicData != null && this.femaleGraphicData.graphicClass == null)
			{
				this.femaleGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.dessicatedBodyGraphicData != null && this.dessicatedBodyGraphicData.graphicClass == null)
			{
				this.dessicatedBodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
		}

		// Token: 0x04002A3C RID: 10812
		[MustTranslate]
		public string label = null;

		// Token: 0x04002A3D RID: 10813
		[MustTranslate]
		public string labelPlural = null;

		// Token: 0x04002A3E RID: 10814
		[MustTranslate]
		public string labelMale = null;

		// Token: 0x04002A3F RID: 10815
		[MustTranslate]
		public string labelMalePlural = null;

		// Token: 0x04002A40 RID: 10816
		[MustTranslate]
		public string labelFemale = null;

		// Token: 0x04002A41 RID: 10817
		[MustTranslate]
		public string labelFemalePlural = null;

		// Token: 0x04002A42 RID: 10818
		[Unsaved]
		[TranslationHandle(Priority = 200)]
		public string untranslatedLabel = null;

		// Token: 0x04002A43 RID: 10819
		[Unsaved]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabelMale = null;

		// Token: 0x04002A44 RID: 10820
		[Unsaved]
		[TranslationHandle]
		public string untranslatedLabelFemale = null;

		// Token: 0x04002A45 RID: 10821
		public GraphicData bodyGraphicData = null;

		// Token: 0x04002A46 RID: 10822
		public GraphicData femaleGraphicData = null;

		// Token: 0x04002A47 RID: 10823
		public GraphicData dessicatedBodyGraphicData = null;

		// Token: 0x04002A48 RID: 10824
		public BodyPartToDrop butcherBodyPart = null;
	}
}
