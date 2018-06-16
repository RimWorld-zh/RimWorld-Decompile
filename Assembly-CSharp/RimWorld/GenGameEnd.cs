using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000980 RID: 2432
	public static class GenGameEnd
	{
		// Token: 0x060036AD RID: 13997 RVA: 0x001D290B File Offset: 0x001D0D0B
		public static void EndGameDialogMessage(string msg, bool allowKeepPlaying = true)
		{
			GenGameEnd.EndGameDialogMessage(msg, allowKeepPlaying, Color.clear);
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x001D291C File Offset: 0x001D0D1C
		public static void EndGameDialogMessage(string msg, bool allowKeepPlaying, Color screenFillColor)
		{
			DiaNode diaNode = new DiaNode(msg);
			if (allowKeepPlaying)
			{
				DiaOption diaOption = new DiaOption("GameOverKeepPlaying".Translate());
				diaOption.resolveTree = true;
				diaNode.options.Add(diaOption);
			}
			DiaOption diaOption2 = new DiaOption("GameOverMainMenu".Translate());
			diaOption2.action = delegate()
			{
				GenScene.GoToMainMenu();
			};
			diaOption2.resolveTree = true;
			diaNode.options.Add(diaOption2);
			Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(diaNode, true, false, null);
			dialog_NodeTree.screenFillColor = screenFillColor;
			dialog_NodeTree.silenceAmbientSound = !allowKeepPlaying;
			dialog_NodeTree.closeOnAccept = allowKeepPlaying;
			dialog_NodeTree.closeOnCancel = allowKeepPlaying;
			Find.WindowStack.Add(dialog_NodeTree);
			Find.Archive.Add(new ArchivedDialog(diaNode.text, null, null));
		}
	}
}
