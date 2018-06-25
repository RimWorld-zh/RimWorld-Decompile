using System;

namespace Verse
{
	// Token: 0x02000B5E RID: 2910
	public class PawnKindLifeStage
	{
		// Token: 0x04002A43 RID: 10819
		[MustTranslate]
		public string label = null;

		// Token: 0x04002A44 RID: 10820
		[MustTranslate]
		public string labelPlural = null;

		// Token: 0x04002A45 RID: 10821
		[MustTranslate]
		public string labelMale = null;

		// Token: 0x04002A46 RID: 10822
		[MustTranslate]
		public string labelMalePlural = null;

		// Token: 0x04002A47 RID: 10823
		[MustTranslate]
		public string labelFemale = null;

		// Token: 0x04002A48 RID: 10824
		[MustTranslate]
		public string labelFemalePlural = null;

		// Token: 0x04002A49 RID: 10825
		[Unsaved]
		[TranslationHandle(Priority = 200)]
		public string untranslatedLabel = null;

		// Token: 0x04002A4A RID: 10826
		[Unsaved]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabelMale = null;

		// Token: 0x04002A4B RID: 10827
		[Unsaved]
		[TranslationHandle]
		public string untranslatedLabelFemale = null;

		// Token: 0x04002A4C RID: 10828
		public GraphicData bodyGraphicData = null;

		// Token: 0x04002A4D RID: 10829
		public GraphicData femaleGraphicData = null;

		// Token: 0x04002A4E RID: 10830
		public GraphicData dessicatedBodyGraphicData = null;

		// Token: 0x04002A4F RID: 10831
		public BodyPartToDrop butcherBodyPart = null;

		// Token: 0x06003F8C RID: 16268 RVA: 0x00217DEE File Offset: 0x002161EE
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelMale = this.labelMale;
			this.untranslatedLabelFemale = this.labelFemale;
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x00217E18 File Offset: 0x00216218
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
	}
}
