using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02000974 RID: 2420
	public static class DevModePermanentlyDisabledUtility
	{
		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x0600367F RID: 13951 RVA: 0x001D0D48 File Offset: 0x001CF148
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

		// Token: 0x06003680 RID: 13952 RVA: 0x001D0D84 File Offset: 0x001CF184
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

		// Token: 0x04002322 RID: 8994
		private static bool initialized;

		// Token: 0x04002323 RID: 8995
		private static bool disabled;
	}
}
