using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000873 RID: 2163
	public class MainTabWindow_Menu : MainTabWindow
	{
		// Token: 0x04001A9F RID: 6815
		private bool anyGameFiles;

		// Token: 0x0600313E RID: 12606 RVA: 0x001ABB5A File Offset: 0x001A9F5A
		public MainTabWindow_Menu()
		{
			this.forcePause = true;
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x0600313F RID: 12607 RVA: 0x001ABB6C File Offset: 0x001A9F6C
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(450f, 390f);
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06003140 RID: 12608 RVA: 0x001ABB90 File Offset: 0x001A9F90
		public override MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Right;
			}
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x001ABBA6 File Offset: 0x001A9FA6
		public override void PreOpen()
		{
			base.PreOpen();
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			this.anyGameFiles = GenFilePaths.AllSavedGameFiles.Any<FileInfo>();
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x001ABBC9 File Offset: 0x001A9FC9
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			VersionControl.DrawInfoInCorner();
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x001ABBD7 File Offset: 0x001A9FD7
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			MainMenuDrawer.DoMainMenuControls(rect, this.anyGameFiles);
		}
	}
}
