using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public static class StorageSettingsClipboard
	{
		private static StorageSettings clipboard = new StorageSettings();

		private static bool copied = false;

		public static bool HasCopiedSettings
		{
			get
			{
				return StorageSettingsClipboard.copied;
			}
		}

		public static void Copy(StorageSettings s)
		{
			StorageSettingsClipboard.clipboard.CopyFrom(s);
			StorageSettingsClipboard.copied = true;
		}

		public static void PasteInto(StorageSettings s)
		{
			s.CopyFrom(StorageSettingsClipboard.clipboard);
		}

		public static IEnumerable<Gizmo> CopyPasteGizmosFor(StorageSettings s)
		{
			yield return new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/CopySettings", true),
				defaultLabel = "CommandCopyZoneSettingsLabel".Translate(),
				defaultDesc = "CommandCopyZoneSettingsDesc".Translate(),
				action = delegate()
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					StorageSettingsClipboard.Copy(s);
				},
				hotKey = KeyBindingDefOf.Misc4
			};
			Command_Action paste = new Command_Action();
			paste.icon = ContentFinder<Texture2D>.Get("UI/Commands/PasteSettings", true);
			paste.defaultLabel = "CommandPasteZoneSettingsLabel".Translate();
			paste.defaultDesc = "CommandPasteZoneSettingsDesc".Translate();
			paste.action = delegate()
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				StorageSettingsClipboard.PasteInto(s);
			};
			paste.hotKey = KeyBindingDefOf.Misc5;
			if (!StorageSettingsClipboard.HasCopiedSettings)
			{
				paste.Disable(null);
			}
			yield return paste;
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static StorageSettingsClipboard()
		{
		}

		[CompilerGenerated]
		private sealed class <CopyPasteGizmosFor>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal Command_Action <copy>__1;

			internal StorageSettings s;

			internal Command_Action <paste>__2;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			private StorageSettingsClipboard.<CopyPasteGizmosFor>c__Iterator0.<CopyPasteGizmosFor>c__AnonStorey1 $locvar0;

			[DebuggerHidden]
			public <CopyPasteGizmosFor>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
				{
					Command_Action copy = new Command_Action();
					copy.icon = ContentFinder<Texture2D>.Get("UI/Commands/CopySettings", true);
					copy.defaultLabel = "CommandCopyZoneSettingsLabel".Translate();
					copy.defaultDesc = "CommandCopyZoneSettingsDesc".Translate();
					copy.action = delegate()
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						StorageSettingsClipboard.Copy(s);
					};
					copy.hotKey = KeyBindingDefOf.Misc4;
					this.$current = copy;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
					paste = new Command_Action();
					paste.icon = ContentFinder<Texture2D>.Get("UI/Commands/PasteSettings", true);
					paste.defaultLabel = "CommandPasteZoneSettingsLabel".Translate();
					paste.defaultDesc = "CommandPasteZoneSettingsDesc".Translate();
					paste.action = delegate()
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						StorageSettingsClipboard.PasteInto(<CopyPasteGizmosFor>c__AnonStorey.s);
					};
					paste.hotKey = KeyBindingDefOf.Misc5;
					if (!StorageSettingsClipboard.HasCopiedSettings)
					{
						paste.Disable(null);
					}
					this.$current = paste;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StorageSettingsClipboard.<CopyPasteGizmosFor>c__Iterator0 <CopyPasteGizmosFor>c__Iterator = new StorageSettingsClipboard.<CopyPasteGizmosFor>c__Iterator0();
				<CopyPasteGizmosFor>c__Iterator.s = s;
				return <CopyPasteGizmosFor>c__Iterator;
			}

			private sealed class <CopyPasteGizmosFor>c__AnonStorey1
			{
				internal StorageSettings s;

				public <CopyPasteGizmosFor>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					StorageSettingsClipboard.Copy(this.s);
				}

				internal void <>m__1()
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					StorageSettingsClipboard.PasteInto(this.s);
				}
			}
		}
	}
}
