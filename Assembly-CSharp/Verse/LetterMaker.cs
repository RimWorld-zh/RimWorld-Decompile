using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E71 RID: 3697
	public static class LetterMaker
	{
		// Token: 0x060056EB RID: 22251 RVA: 0x002CB6B4 File Offset: 0x002C9AB4
		public static Letter MakeLetter(LetterDef def)
		{
			Letter letter = (Letter)Activator.CreateInstance(def.letterClass);
			letter.def = def;
			letter.ID = Find.UniqueIDsManager.GetNextLetterID();
			return letter;
		}

		// Token: 0x060056EC RID: 22252 RVA: 0x002CB6F4 File Offset: 0x002C9AF4
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

		// Token: 0x060056ED RID: 22253 RVA: 0x002CB758 File Offset: 0x002C9B58
		public static ChoiceLetter MakeLetter(string label, string text, LetterDef def, LookTargets lookTargets, Faction relatedFaction = null)
		{
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, def);
			choiceLetter.lookTargets = lookTargets;
			choiceLetter.relatedFaction = relatedFaction;
			return choiceLetter;
		}
	}
}
