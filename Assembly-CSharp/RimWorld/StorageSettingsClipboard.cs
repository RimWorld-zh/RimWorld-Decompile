using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000693 RID: 1683
	public static class StorageSettingsClipboard
	{
		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060023B0 RID: 9136 RVA: 0x00132424 File Offset: 0x00130824
		public static bool HasCopiedSettings
		{
			get
			{
				return StorageSettingsClipboard.copied;
			}
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x0013243E File Offset: 0x0013083E
		public static void Copy(StorageSettings s)
		{
			StorageSettingsClipboard.clipboard.CopyFrom(s);
			StorageSettingsClipboard.copied = true;
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x00132452 File Offset: 0x00130852
		public static void PasteInto(StorageSettings s)
		{
			s.CopyFrom(StorageSettingsClipboard.clipboard);
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x00132460 File Offset: 0x00130860
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

		// Token: 0x040013F3 RID: 5107
		private static StorageSettings clipboard = new StorageSettings();

		// Token: 0x040013F4 RID: 5108
		private static bool copied = false;
	}
}
