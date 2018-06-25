using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02000972 RID: 2418
	public static class DevModePermanentlyDisabledUtility
	{
		// Token: 0x04002321 RID: 8993
		private static bool initialized;

		// Token: 0x04002322 RID: 8994
		private static bool disabled;

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x0600367C RID: 13948 RVA: 0x001D1070 File Offset: 0x001CF470
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

		// Token: 0x0600367D RID: 13949 RVA: 0x001D10AC File Offset: 0x001CF4AC
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
