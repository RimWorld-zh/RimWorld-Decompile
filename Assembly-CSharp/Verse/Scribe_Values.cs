using System;

namespace Verse
{
	// Token: 0x02000DAD RID: 3501
	public static class Scribe_Values
	{
		// Token: 0x06004E19 RID: 19993 RVA: 0x0028D314 File Offset: 0x0028B714
		public static void Look<T>(ref T value, string label, T defaultValue = default(T), bool forceSave = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (typeof(T) == typeof(TargetInfo))
				{
					Log.Error("Saving a TargetInfo " + label + " with Scribe_Values. TargetInfos must be saved with Scribe_TargetInfo.", false);
				}
				else if (typeof(Thing).IsAssignableFrom(typeof(T)))
				{
					Log.Error("Using Scribe_Values with a Thing reference " + label + ". Use Scribe_References or Scribe_Deep instead.", false);
				}
				else if (typeof(IExposable).IsAssignableFrom(typeof(T)))
				{
					Log.Error("Using Scribe_Values with a IExposable reference " + label + ". Use Scribe_References or Scribe_Deep instead.", false);
				}
				else if (typeof(Def).IsAssignableFrom(typeof(T)))
				{
					Log.Error("Using Scribe_Values with a Def " + label + ". Use Scribe_Defs instead.", false);
				}
				else if (forceSave || (value == null && defaultValue != null) || (value != null && !value.Equals(defaultValue)))
				{
					Scribe.saver.WriteElement(label, value.ToString());
				}
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				value = ScribeExtractor.ValueFromNode<T>(Scribe.loader.curXmlParent[label], defaultValue);
			}
		}
	}
}
