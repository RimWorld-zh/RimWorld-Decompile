using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000871 RID: 2161
	public class MainTabWindow_Menu : MainTabWindow
	{
		// Token: 0x0600313A RID: 12602 RVA: 0x001ABA0A File Offset: 0x001A9E0A
		public MainTabWindow_Menu()
		{
			this.forcePause = true;
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x001ABA1C File Offset: 0x001A9E1C
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(450f, 390f);
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x0600313C RID: 12604 RVA: 0x001ABA40 File Offset: 0x001A9E40
		public override MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Right;
			}
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x001ABA56 File Offset: 0x001A9E56
		public override void PreOpen()
		{
			base.PreOpen();
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			this.anyGameFiles = GenFilePaths.AllSavedGameFiles.Any<FileInfo>();
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x001ABA79 File Offset: 0x001A9E79
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			VersionControl.DrawInfoInCorner();
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x001ABA87 File Offset: 0x001A9E87
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			MainMenuDrawer.DoMainMenuControls(rect, this.anyGameFiles);
		}

		// Token: 0x04001A9F RID: 6815
		private bool anyGameFiles;
	}
}
