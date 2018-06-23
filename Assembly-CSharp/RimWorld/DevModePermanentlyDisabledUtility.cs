using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02000970 RID: 2416
	public static class DevModePermanentlyDisabledUtility
	{
		// Token: 0x04002320 RID: 8992
		private static bool initialized;

		// Token: 0x04002321 RID: 8993
		private static bool disabled;

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06003678 RID: 13944 RVA: 0x001D0F30 File Offset: 0x001CF330
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

		// Token: 0x06003679 RID: 13945 RVA: 0x001D0F6C File Offset: 0x001CF36C
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
