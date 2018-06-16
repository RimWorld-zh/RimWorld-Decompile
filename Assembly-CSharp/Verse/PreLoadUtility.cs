using System;

namespace Verse
{
	// Token: 0x02000D90 RID: 3472
	public static class PreLoadUtility
	{
		// Token: 0x06004D91 RID: 19857 RVA: 0x00287C1C File Offset: 0x0028601C
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
