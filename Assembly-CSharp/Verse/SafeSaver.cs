using System;
using System.IO;
using System.Threading;

namespace Verse
{
	// Token: 0x02000D8D RID: 3469
	public static class SafeSaver
	{
		// Token: 0x06004DA5 RID: 19877 RVA: 0x00289238 File Offset: 0x00287638
		private static string GetFileFullPath(string path)
		{
			return Path.GetFullPath(path);
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x00289254 File Offset: 0x00287654
		private static string GetNewFileFullPath(string path)
		{
			return Path.GetFullPath(path + SafeSaver.NewFileSuffix);
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x0028927C File Offset: 0x0028767C
		private static string GetOldFileFullPath(string path)
		{
			return Path.GetFullPath(path + SafeSaver.OldFileSuffix);
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x002892A4 File Offset: 0x002876A4
		public static void Save(string path, string documentElementName, Action saveAction)
		{
			try
			{
				SafeSaver.CleanSafeSaverFiles(path);
				if (!File.Exists(SafeSaver.GetFileFullPath(path)))
				{
					SafeSaver.DoSave(SafeSaver.GetFileFullPath(path), documentElementName, saveAction);
				}
				else
				{
					SafeSaver.DoSave(SafeSaver.GetNewFileFullPath(path), documentElementName, saveAction);
					try
					{
						SafeSaver.SafeMove(SafeSaver.GetFileFullPath(path), SafeSaver.GetOldFileFullPath(path));
					}
					catch (Exception ex)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Could not move file from \"",
							SafeSaver.GetFileFullPath(path),
							"\" to \"",
							SafeSaver.GetOldFileFullPath(path),
							"\": ",
							ex
						}), false);
						throw;
					}
					try
					{
						SafeSaver.SafeMove(SafeSaver.GetNewFileFullPath(path), SafeSaver.GetFileFullPath(path));
					}
					catch (Exception ex2)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Could not move file from \"",
							SafeSaver.GetNewFileFullPath(path),
							"\" to \"",
							SafeSaver.GetFileFullPath(path),
							"\": ",
							ex2
						}), false);
						SafeSaver.RemoveFileIfExists(SafeSaver.GetFileFullPath(path), false);
						SafeSaver.RemoveFileIfExists(SafeSaver.GetNewFileFullPath(path), false);
						try
						{
							SafeSaver.SafeMove(SafeSaver.GetOldFileFullPath(path), SafeSaver.GetFileFullPath(path));
						}
						catch (Exception ex3)
						{
							Log.Warning(string.Concat(new object[]
							{
								"Could not move file from \"",
								SafeSaver.GetOldFileFullPath(path),
								"\" back to \"",
								SafeSaver.GetFileFullPath(path),
								"\": ",
								ex3
							}), false);
						}
						throw;
					}
					SafeSaver.RemoveFileIfExists(SafeSaver.GetOldFileFullPath(path), true);
				}
			}
			catch (Exception ex4)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(new object[]
				{
					SafeSaver.GetFileFullPath(path),
					ex4.ToString()
				}));
				throw;
			}
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x002894B8 File Offset: 0x002878B8
		private static void CleanSafeSaverFiles(string path)
		{
			SafeSaver.RemoveFileIfExists(SafeSaver.GetOldFileFullPath(path), true);
			SafeSaver.RemoveFileIfExists(SafeSaver.GetNewFileFullPath(path), true);
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x002894D4 File Offset: 0x002878D4
		private static void DoSave(string fullPath, string documentElementName, Action saveAction)
		{
			try
			{
				Scribe.saver.InitSaving(fullPath, documentElementName);
				saveAction();
				Scribe.saver.FinalizeSaving();
			}
			catch (Exception ex)
			{
				Log.Warning(string.Concat(new object[]
				{
					"An exception was thrown during saving to \"",
					fullPath,
					"\": ",
					ex
				}), false);
				Scribe.saver.ForceStop();
				SafeSaver.RemoveFileIfExists(fullPath, false);
				throw;
			}
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x00289554 File Offset: 0x00287954
		private static void RemoveFileIfExists(string path, bool rethrow)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Could not remove file \"",
					path,
					"\": ",
					ex
				}), false);
				if (rethrow)
				{
					throw;
				}
			}
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x002895C4 File Offset: 0x002879C4
		private static void SafeMove(string from, string to)
		{
			Exception ex = null;
			for (int i = 0; i < 50; i++)
			{
				try
				{
					File.Move(from, to);
					return;
				}
				catch (Exception ex2)
				{
					if (ex == null)
					{
						ex = ex2;
					}
				}
				Thread.Sleep(1);
			}
			throw ex;
		}

		// Token: 0x040033D2 RID: 13266
		private static readonly string NewFileSuffix = ".new";

		// Token: 0x040033D3 RID: 13267
		private static readonly string OldFileSuffix = ".old";
	}
}
