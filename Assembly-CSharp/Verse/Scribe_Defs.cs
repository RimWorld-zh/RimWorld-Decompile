using System;

namespace Verse
{
	// Token: 0x02000DA9 RID: 3497
	public static class Scribe_Defs
	{
		// Token: 0x06004E07 RID: 19975 RVA: 0x0028CB94 File Offset: 0x0028AF94
		public static void Look<T>(ref T value, string label) where T : Def, new()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				string text;
				if (value == null)
				{
					text = "null";
				}
				else
				{
					text = value.defName;
				}
				Scribe_Values.Look<string>(ref text, label, "null", false);
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				value = ScribeExtractor.DefFromNode<T>(Scribe.loader.curXmlParent[label]);
			}
		}
	}
}
