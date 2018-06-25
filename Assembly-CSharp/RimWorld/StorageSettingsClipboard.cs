using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000695 RID: 1685
	public static class StorageSettingsClipboard
	{
		// Token: 0x040013F3 RID: 5107
		private static StorageSettings clipboard = new StorageSettings();

		// Token: 0x040013F4 RID: 5108
		private static bool copied = false;

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060023B4 RID: 9140 RVA: 0x00132574 File Offset: 0x00130974
		public static bool HasCopiedSettings
		{
			get
			{
				return StorageSettingsClipboard.copied;
			}
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x0013258E File Offset: 0x0013098E
		public static void Copy(StorageSettings s)
		{
			StorageSettingsClipboard.clipboard.CopyFrom(s);
			StorageSettingsClipboard.copied = true;
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x001325A2 File Offset: 0x001309A2
		public static void PasteInto(StorageSettings s)
		{
			s.CopyFrom(StorageSettingsClipboard.clipboard);
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x001325B0 File Offset: 0x001309B0
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
	}
}
