using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;
using Verse.Profile;

namespace RimWorld
{
	public sealed class Autosaver
	{
		private int ticksSinceSave = 0;

		private const int NumAutosaves = 5;

		public const float MaxPermadeathModeAutosaveInterval = 1f;

		[CompilerGenerated]
		private static Func<string, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<string, DateTime> <>f__am$cache1;

		public Autosaver()
		{
		}

		private float AutosaveIntervalDays
		{
			get
			{
				float num = Prefs.AutosaveIntervalDays;
				if (Current.Game.Info.permadeathMode && num > 1f)
				{
					num = 1f;
				}
				return num;
			}
		}

		private int AutosaveIntervalTicks
		{
			get
			{
				return Mathf.RoundToInt(this.AutosaveIntervalDays * 60000f);
			}
		}

		public void AutosaverTick()
		{
			this.ticksSinceSave++;
			if (this.ticksSinceSave >= this.AutosaveIntervalTicks)
			{
				LongEventHandler.QueueLongEvent(new Action(this.DoAutosave), "Autosaving", false, null);
				this.ticksSinceSave = 0;
			}
		}

		public void DoAutosave()
		{
			string fileName;
			if (Current.Game.Info.permadeathMode)
			{
				fileName = Current.Game.Info.permadeathModeUniqueName;
			}
			else
			{
				fileName = this.NewAutosaveFileName();
			}
			GameDataSaveLoader.SaveGame(fileName);
		}

		private void DoMemoryCleanup()
		{
			Profiler.BeginSample("UnloadUnusedAssets");
			MemoryUtility.UnloadUnusedUnityAssets();
			Profiler.EndSample();
		}

		private string NewAutosaveFileName()
		{
			string text = (from name in this.AutoSaveNames()
			where !SaveGameFilesUtility.SavedGameNamedExists(name)
			select name).FirstOrDefault<string>();
			string result;
			if (text != null)
			{
				result = text;
			}
			else
			{
				string text2 = this.AutoSaveNames().MinBy((string name) => new FileInfo(GenFilePaths.FilePathForSavedGame(name)).LastWriteTime);
				result = text2;
			}
			return result;
		}

		private IEnumerable<string> AutoSaveNames()
		{
			for (int i = 1; i <= 5; i++)
			{
				yield return "Autosave-" + i;
			}
			yield break;
		}

		[CompilerGenerated]
		private static bool <NewAutosaveFileName>m__0(string name)
		{
			return !SaveGameFilesUtility.SavedGameNamedExists(name);
		}

		[CompilerGenerated]
		private static DateTime <NewAutosaveFileName>m__1(string name)
		{
			return new FileInfo(GenFilePaths.FilePathForSavedGame(name)).LastWriteTime;
		}

		[CompilerGenerated]
		private sealed class <AutoSaveNames>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal int <i>__1;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AutoSaveNames>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 1;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i <= 5)
				{
					this.$current = "Autosave-" + i;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new Autosaver.<AutoSaveNames>c__Iterator0();
			}
		}
	}
}
