using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class GenGameEnd
	{
		[CompilerGenerated]
		private static Action <>f__am$cache0;

		public static void EndGameDialogMessage(string msg, bool allowKeepPlaying = true)
		{
			GenGameEnd.EndGameDialogMessage(msg, allowKeepPlaying, Color.clear);
		}

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

		[CompilerGenerated]
		private static void <EndGameDialogMessage>m__0()
		{
			GenScene.GoToMainMenu();
		}
	}
}
