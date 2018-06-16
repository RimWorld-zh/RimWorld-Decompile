using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000697 RID: 1687
	public static class StorageSettingsClipboard
	{
		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060023B6 RID: 9142 RVA: 0x00132264 File Offset: 0x00130664
		public static bool HasCopiedSettings
		{
			get
			{
				return StorageSettingsClipboard.copied;
			}
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x0013227E File Offset: 0x0013067E
		public static void Copy(StorageSettings s)
		{
			StorageSettingsClipboard.clipboard.CopyFrom(s);
			StorageSettingsClipboard.copied = true;
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x00132292 File Offset: 0x00130692
		public static void PasteInto(StorageSettings s)
		{
			s.CopyFrom(StorageSettingsClipboard.clipboard);
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x001322A0 File Offset: 0x001306A0
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

		// Token: 0x040013F5 RID: 5109
		private static StorageSettings clipboard = new StorageSettings();

		// Token: 0x040013F6 RID: 5110
		private static bool copied = false;
	}
}
