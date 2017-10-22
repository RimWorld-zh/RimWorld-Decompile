using System.Xml;

namespace Verse
{
	public static class Scribe_Defs
	{
		public static void Look<T>(ref T value, string label) where T : Def, new()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				string text = (value != null) ? ((Def)(object)value).defName : "null";
				Scribe_Values.Look<string>(ref text, label, "null", false);
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				value = ScribeExtractor.DefFromNode<T>((XmlNode)Scribe.loader.curXmlParent[label]);
			}
		}
	}
}
