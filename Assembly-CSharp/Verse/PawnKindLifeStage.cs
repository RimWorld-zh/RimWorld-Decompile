using System;

namespace Verse
{
	public class PawnKindLifeStage
	{
		[MustTranslate]
		public string label = null;

		[MustTranslate]
		public string labelPlural = null;

		[MustTranslate]
		public string labelMale = null;

		[MustTranslate]
		public string labelMalePlural = null;

		[MustTranslate]
		public string labelFemale = null;

		[MustTranslate]
		public string labelFemalePlural = null;

		[TranslationHandle(Priority = 200)]
		[Unsaved]
		public string untranslatedLabel = null;

		[TranslationHandle(Priority = 100)]
		[Unsaved]
		public string untranslatedLabelMale = null;

		[TranslationHandle]
		[Unsaved]
		public string untranslatedLabelFemale = null;

		public GraphicData bodyGraphicData = null;

		public GraphicData femaleGraphicData = null;

		public GraphicData dessicatedBodyGraphicData = null;

		public BodyPartToDrop butcherBodyPart = null;

		public PawnKindLifeStage()
		{
		}

		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelMale = this.labelMale;
			this.untranslatedLabelFemale = this.labelFemale;
		}

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
