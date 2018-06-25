using System;

namespace Verse
{
	// Token: 0x02000DA5 RID: 3493
	public static class Scribe_BodyParts
	{
		// Token: 0x06004E15 RID: 19989 RVA: 0x0028CDD4 File Offset: 0x0028B1D4
		public static void Look(ref BodyPartRecord part, string label, BodyPartRecord defaultValue = null)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (part != defaultValue)
				{
					if (Scribe.EnterNode(label))
					{
						try
						{
							if (part == null)
							{
								Scribe.saver.WriteAttribute("IsNull", "True");
							}
							else
							{
								string defName = part.body.defName;
								Scribe_Values.Look<string>(ref defName, "body", null, false);
								int index = part.Index;
								Scribe_Values.Look<int>(ref index, "index", 0, true);
							}
						}
						finally
						{
							Scribe.ExitNode();
						}
					}
				}
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				part = ScribeExtractor.BodyPartFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
			}
		}
	}
}
