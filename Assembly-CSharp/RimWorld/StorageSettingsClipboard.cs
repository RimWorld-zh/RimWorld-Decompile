using System;
using System.Collections.Generic;
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
			yield return (Gizmo)new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/CopySettings", true),
				defaultLabel = "CommandCopyZoneSettingsLabel".Translate(),
				defaultDesc = "CommandCopyZoneSettingsDesc".Translate(),
				action = (Action)delegate
				{
					SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
					StorageSettingsClipboard.Copy(((_003CCopyPasteGizmosFor_003Ec__Iterator14F)/*Error near IL_0076: stateMachine*/).s);
				},
				hotKey = KeyBindingDefOf.Misc4
			};
			Command_Action paste = new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/PasteSettings", true),
				defaultLabel = "CommandPasteZoneSettingsLabel".Translate(),
				defaultDesc = "CommandPasteZoneSettingsDesc".Translate(),
				action = (Action)delegate
				{
					SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
					StorageSettingsClipboard.PasteInto(((_003CCopyPasteGizmosFor_003Ec__Iterator14F)/*Error near IL_0100: stateMachine*/).s);
				},
				hotKey = KeyBindingDefOf.Misc5
			};
			if (!StorageSettingsClipboard.HasCopiedSettings)
			{
				paste.Disable((string)null);
			}
			yield return (Gizmo)paste;
		}
	}
}
