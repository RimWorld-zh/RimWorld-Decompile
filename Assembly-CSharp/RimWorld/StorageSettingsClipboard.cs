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
		// Token: 0x040013F7 RID: 5111
		private static StorageSettings clipboard = new StorageSettings();

		// Token: 0x040013F8 RID: 5112
		private static bool copied = false;

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060023B3 RID: 9139 RVA: 0x001327DC File Offset: 0x00130BDC
		public static bool HasCopiedSettings
		{
			get
			{
				return StorageSettingsClipboard.copied;
			}
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x001327F6 File Offset: 0x00130BF6
		public static void Copy(StorageSettings s)
		{
			StorageSettingsClipboard.clipboard.CopyFrom(s);
			StorageSettingsClipboard.copied = true;
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x0013280A File Offset: 0x00130C0A
		public static void PasteInto(StorageSettings s)
		{
			s.CopyFrom(StorageSettingsClipboard.clipboard);
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x00132818 File Offset: 0x00130C18
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
