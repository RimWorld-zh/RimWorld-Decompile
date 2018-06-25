using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02000972 RID: 2418
	public static class DevModePermanentlyDisabledUtility
	{
		// Token: 0x04002328 RID: 9000
		private static bool initialized;

		// Token: 0x04002329 RID: 9001
		private static bool disabled;

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x0600367C RID: 13948 RVA: 0x001D1344 File Offset: 0x001CF744
		public static bool Disabled
		{
			get
			{
				if (!DevModePermanentlyDisabledUtility.initialized)
				{
					DevModePermanentlyDisabledUtility.initialized = true;
					DevModePermanentlyDisabledUtility.disabled = File.Exists(GenFilePaths.DevModePermanentlyDisabledFilePath);
				}
				return DevModePermanentlyDisabledUtility.disabled;
			}
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x001D1380 File Offset: 0x001CF780
		public static void Disable()
		{
			try
			{
				File.Create(GenFilePaths.DevModePermanentlyDisabledFilePath).Dispose();
			}
			catch (Exception arg)
			{
				Log.Error("Could not permanently disable dev mode: " + arg, false);
				return;
			}
			DevModePermanentlyDisabledUtility.disabled = true;
			Prefs.DevMode = false;
		}
	}
}
