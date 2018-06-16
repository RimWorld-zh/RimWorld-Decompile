using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000642 RID: 1602
	public class ScenPart_GameStartDialog : ScenPart
	{
		// Token: 0x06002126 RID: 8486 RVA: 0x0011A004 File Offset: 0x00118404
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 5f);
			this.text = Widgets.TextArea(scenPartRect, this.text, false);
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x0011A038 File Offset: 0x00118438
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.textKey, "textKey", null, false);
			Scribe_Defs.Look<SoundDef>(ref this.closeSound, "closeSound");
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x0011A078 File Offset: 0x00118478
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

		// Token: 0x040012E6 RID: 4838
		private string text;

		// Token: 0x040012E7 RID: 4839
		private string textKey;

		// Token: 0x040012E8 RID: 4840
		private SoundDef closeSound;
	}
}
