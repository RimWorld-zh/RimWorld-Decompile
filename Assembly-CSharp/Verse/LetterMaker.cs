using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E70 RID: 3696
	public static class LetterMaker
	{
		// Token: 0x0600570B RID: 22283 RVA: 0x002CD2C4 File Offset: 0x002CB6C4
		public static Letter MakeLetter(LetterDef def)
		{
			Letter letter = (Letter)Activator.CreateInstance(def.letterClass);
			letter.def = def;
			letter.ID = Find.UniqueIDsManager.GetNextLetterID();
			return letter;
		}

		// Token: 0x0600570C RID: 22284 RVA: 0x002CD304 File Offset: 0x002CB704
		public static ChoiceLetter MakeLetter(string label, string text, LetterDef def)
		{
			ChoiceLetter result;
			if (!typeof(ChoiceLetter).IsAssignableFrom(def.letterClass))
			{
				Log.Error(def + " is not a choice letter.", false);
				result = null;
			}
			else
			{
				ChoiceLetter choiceLetter = (ChoiceLetter)LetterMaker.MakeLetter(def);
				choiceLetter.label = label;
				choiceLetter.text = text;
				result = choiceLetter;
			}
			return result;
		}

		// Token: 0x0600570D RID: 22285 RVA: 0x002CD368 File Offset: 0x002CB768
		public static ChoiceLetter MakeLetter(string label, string text, LetterDef def, LookTargets lookTargets, Faction relatedFaction = null)
		{
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, def);
			choiceLetter.lookTargets = lookTargets;
			choiceLetter.relatedFaction = relatedFaction;
			return choiceLetter;
		}
	}
}
