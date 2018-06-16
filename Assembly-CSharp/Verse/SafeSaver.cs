using System;
using System.IO;
using System.Threading;

namespace Verse
{
	// Token: 0x02000D91 RID: 3473
	public static class SafeSaver
	{
		// Token: 0x06004D92 RID: 19858 RVA: 0x00287CA8 File Offset: 0x002860A8
		private static string GetFileFullPath(string path)
		{
			return Path.GetFullPath(path);
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x00287CC4 File Offset: 0x002860C4
		private static string GetNewFileFullPath(string path)
		{
			return Path.GetFullPath(path + SafeSaver.NewFileSuffix);
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x00287CEC File Offset: 0x002860EC
		private static string GetOldFileFullPath(string path)
		{
			return Path.GetFullPath(path + SafeSaver.OldFileSuffix);
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x00287D14 File Offset: 0x00286114
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

		// Token: 0x06004D96 RID: 19862 RVA: 0x00287F28 File Offset: 0x00286328
		private static void CleanSafeSaverFiles(string path)
		{
			SafeSaver.RemoveFileIfExists(SafeSaver.GetOldFileFullPath(path), true);
			SafeSaver.RemoveFileIfExists(SafeSaver.GetNewFileFullPath(path), true);
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x00287F44 File Offset: 0x00286344
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

		// Token: 0x06004D98 RID: 19864 RVA: 0x00287FC4 File Offset: 0x002863C4
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

		// Token: 0x06004D99 RID: 19865 RVA: 0x00288034 File Offset: 0x00286434
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

		// Token: 0x040033C9 RID: 13257
		private static readonly string NewFileSuffix = ".new";

		// Token: 0x040033CA RID: 13258
		private static readonly string OldFileSuffix = ".old";
	}
}
