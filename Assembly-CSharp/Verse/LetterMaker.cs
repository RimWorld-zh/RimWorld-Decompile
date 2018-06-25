using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E72 RID: 3698
	public static class LetterMaker
	{
		// Token: 0x0600570F RID: 22287 RVA: 0x002CD3F0 File Offset: 0x002CB7F0
		public static Letter MakeLetter(LetterDef def)
		{
			Letter letter = (Letter)Activator.CreateInstance(def.letterClass);
			letter.def = def;
			letter.ID = Find.UniqueIDsManager.GetNextLetterID();
			return letter;
		}

		// Token: 0x06005710 RID: 22288 RVA: 0x002CD430 File Offset: 0x002CB830
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

		// Token: 0x06005711 RID: 22289 RVA: 0x002CD494 File Offset: 0x002CB894
		public static ChoiceLetter MakeLetter(string label, string text, LetterDef def, LookTargets lookTargets, Faction relatedFaction = null)
		{
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, def);
			choiceLetter.lookTargets = lookTargets;
			choiceLetter.relatedFaction = relatedFaction;
			return choiceLetter;
		}
	}
}
