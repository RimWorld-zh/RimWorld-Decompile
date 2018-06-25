using System;

namespace Verse
{
	// Token: 0x02000D8E RID: 3470
	public static class PreLoadUtility
	{
		// Token: 0x06004DA8 RID: 19880 RVA: 0x002892D8 File Offset: 0x002876D8
		public static void CheckVersionAndLoad(string path, ScribeMetaHeaderUtility.ScribeHeaderMode mode, Action loadAct)
		{
			try
			{
				Scribe.loader.InitLoadingMetaHeaderOnly(path);
				ScribeMetaHeaderUtility.LoadGameDataHeader(mode, false);
				Scribe.loader.FinalizeLoading();
			}
			catch (Exception ex)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Exception loading ",
					path,
					": ",
					ex
				}), false);
				Scribe.ForceStop();
			}
			if (!ScribeMetaHeaderUtility.TryCreateDialogsForVersionMismatchWarnings(loadAct))
			{
				loadAct();
			}
		}
	}
}
