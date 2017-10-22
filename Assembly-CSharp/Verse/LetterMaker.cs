using RimWorld.Planet;
using System;

namespace Verse
{
	public static class LetterMaker
	{
		public static Letter MakeLetter(LetterDef def)
		{
			Letter letter = (Letter)Activator.CreateInstance(def.letterClass);
			letter.def = def;
			return letter;
		}

		public static ChoiceLetter MakeLetter(string label, string text, LetterDef def)
		{
			ChoiceLetter result;
			if (!typeof(ChoiceLetter).IsAssignableFrom(def.letterClass))
			{
				Log.Error(def + " is not a choice letter.");
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

		public static ChoiceLetter MakeLetter(string label, string text, LetterDef def, GlobalTargetInfo lookTarget)
		{
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, def);
			choiceLetter.lookTarget = lookTarget;
			return choiceLetter;
		}
	}
}
