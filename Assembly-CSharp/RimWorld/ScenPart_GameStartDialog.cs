using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000640 RID: 1600
	public class ScenPart_GameStartDialog : ScenPart
	{
		// Token: 0x040012E7 RID: 4839
		private string text;

		// Token: 0x040012E8 RID: 4840
		private string textKey;

		// Token: 0x040012E9 RID: 4841
		private SoundDef closeSound;

		// Token: 0x06002123 RID: 8483 RVA: 0x0011A534 File Offset: 0x00118934
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 5f);
			this.text = Widgets.TextArea(scenPartRect, this.text, false);
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x0011A568 File Offset: 0x00118968
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.textKey, "textKey", null, false);
			Scribe_Defs.Look<SoundDef>(ref this.closeSound, "closeSound");
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x0011A5A8 File Offset: 0x001189A8
		public override void PostGameStart()
		{
			if (Find.GameInitData.startedFromEntry)
			{
				Find.MusicManagerPlay.disabled = true;
				Find.WindowStack.Notify_GameStartDialogOpened();
				DiaNode diaNode = new DiaNode((!this.text.NullOrEmpty()) ? this.text : this.textKey.Translate());
				DiaOption diaOption = new DiaOption();
				diaOption.resolveTree = true;
				diaOption.clickSound = null;
				diaNode.options.Add(diaOption);
				Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(diaNode, false, false, null);
				dialog_NodeTree.soundClose = ((this.closeSound == null) ? SoundDefOf.GameStartSting : this.closeSound);
				dialog_NodeTree.closeAction = delegate()
				{
					Find.MusicManagerPlay.ForceSilenceFor(7f);
					Find.MusicManagerPlay.disabled = false;
					Find.WindowStack.Notify_GameStartDialogClosed();
					Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
					TutorSystem.Notify_Event("GameStartDialogClosed");
				};
				Find.WindowStack.Add(dialog_NodeTree);
				Find.Archive.Add(new ArchivedDialog(diaNode.text, null, null));
			}
		}
	}
}
