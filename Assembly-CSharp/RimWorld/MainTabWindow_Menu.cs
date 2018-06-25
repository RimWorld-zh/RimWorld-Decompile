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
		// Token: 0x04001AA3 RID: 6819
		private bool anyGameFiles;

		// Token: 0x0600313D RID: 12605 RVA: 0x001ABDC2 File Offset: 0x001AA1C2
		public MainTabWindow_Menu()
		{
			this.forcePause = true;
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x0600313E RID: 12606 RVA: 0x001ABDD4 File Offset: 0x001AA1D4
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(450f, 390f);
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x0600313F RID: 12607 RVA: 0x001ABDF8 File Offset: 0x001AA1F8
		public override MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Right;
			}
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x001ABE0E File Offset: 0x001AA20E
		public override void PreOpen()
		{
			base.PreOpen();
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			this.anyGameFiles = GenFilePaths.AllSavedGameFiles.Any<FileInfo>();
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x001ABE31 File Offset: 0x001AA231
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			VersionControl.DrawInfoInCorner();
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x001ABE3F File Offset: 0x001AA23F
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			MainMenuDrawer.DoMainMenuControls(rect, this.anyGameFiles);
		}
	}
}
