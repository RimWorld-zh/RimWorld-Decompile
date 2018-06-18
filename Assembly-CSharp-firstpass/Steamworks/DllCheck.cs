using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x02000030 RID: 48
	public class DllCheck
	{
		// Token: 0x060000B9 RID: 185
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x060000BA RID: 186
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern int GetModuleFileName(IntPtr hModule, StringBuilder strFullPath, int nSize);

		// Token: 0x060000BB RID: 187 RVA: 0x0000339C File Offset: 0x0000159C
		public static bool Test()
		{
			return true;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000033B4 File Offset: 0x000015B4
		private static bool CheckSteamAPIDLL()
		{
			string lpModuleName;
			int num;
			if (IntPtr.Size == 4)
			{
				lpModuleName = "steam_api.dll";
				num = 186560;
			}
			else
			{
				lpModuleName = "steam_api64.dll";
				num = 206760;
			}
			IntPtr moduleHandle = DllCheck.GetModuleHandle(lpModuleName);
			bool result;
			if (moduleHandle == IntPtr.Zero)
			{
				result = true;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				DllCheck.GetModuleFileName(moduleHandle, stringBuilder, stringBuilder.Capacity);
				string text = stringBuilder.ToString();
				if (File.Exists(text))
				{
					FileInfo fileInfo = new FileInfo(text);
					if (fileInfo.Length != (long)num)
					{
						return false;
					}
					if (FileVersionInfo.GetVersionInfo(text).FileVersion != "02.89.45.04")
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}
	}
}
