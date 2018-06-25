using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Verse;

namespace RimWorld
{
	public static class ExternalHistoryUtility
	{
		private static List<FileInfo> cachedFiles;

		private static int gameplayIDLength = 20;

		private static string gameplayIDAvailableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

		static ExternalHistoryUtility()
		{
			try
			{
				ExternalHistoryUtility.cachedFiles = GenFilePaths.AllExternalHistoryFiles.ToList<FileInfo>();
			}
			catch (Exception ex)
			{
				Log.Error("Could not get external history files: " + ex.Message, false);
			}
		}

		public static IEnumerable<FileInfo> Files
		{
			get
			{
				for (int i = 0; i < ExternalHistoryUtility.cachedFiles.Count; i++)
				{
					yield return ExternalHistoryUtility.cachedFiles[i];
				}
				yield break;
			}
		}

		public static ExternalHistory Load(string path)
		{
			ExternalHistory result = null;
			try
			{
				result = new ExternalHistory();
				Scribe.loader.InitLoading(path);
				try
				{
					Scribe_Deep.Look<ExternalHistory>(ref result, "externalHistory", new object[0]);
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
			}
			catch (Exception ex)
			{
				Log.Error("Could not load external history (" + path + "): " + ex.Message, false);
				return null;
			}
			return result;
		}

		public static string GetRandomGameplayID()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < ExternalHistoryUtility.gameplayIDLength; i++)
			{
				int index = Rand.Range(0, ExternalHistoryUtility.gameplayIDAvailableChars.Length);
				stringBuilder.Append(ExternalHistoryUtility.gameplayIDAvailableChars[index]);
			}
			return stringBuilder.ToString();
		}

		public static bool IsValidGameplayID(string ID)
		{
			bool result;
			if (ID.NullOrEmpty() || ID.Length != ExternalHistoryUtility.gameplayIDLength)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < ID.Length; i++)
				{
					bool flag = false;
					for (int j = 0; j < ExternalHistoryUtility.gameplayIDAvailableChars.Length; j++)
					{
						if (ID[i] == ExternalHistoryUtility.gameplayIDAvailableChars[j])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		public static string GetCurrentUploadDate()
		{
			return DateTime.UtcNow.ToString("yyMMdd");
		}

		public static int GetCurrentUploadTime()
		{
			return (int)(DateTime.UtcNow.TimeOfDay.TotalSeconds / 2.0);
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<FileInfo>, IEnumerator, IDisposable, IEnumerator<FileInfo>
		{
			internal int <i>__1;

			internal FileInfo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < ExternalHistoryUtility.cachedFiles.Count)
				{
					this.$current = ExternalHistoryUtility.cachedFiles[i];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			FileInfo IEnumerator<FileInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<System.IO.FileInfo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FileInfo> IEnumerable<FileInfo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new ExternalHistoryUtility.<>c__Iterator0();
			}
		}
	}
}
