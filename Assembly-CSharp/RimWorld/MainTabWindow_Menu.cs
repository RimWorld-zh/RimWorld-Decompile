using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000875 RID: 2165
	public class MainTabWindow_Menu : MainTabWindow
	{
		// Token: 0x0600313F RID: 12607 RVA: 0x001AB75A File Offset: 0x001A9B5A
		public MainTabWindow_Menu()
		{
			this.forcePause = true;
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06003140 RID: 12608 RVA: 0x001AB76C File Offset: 0x001A9B6C
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(450f, 390f);
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06003141 RID: 12609 RVA: 0x001AB790 File Offset: 0x001A9B90
		public override MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Right;
			}
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x001AB7A6 File Offset: 0x001A9BA6
		public override void PreOpen()
		{
			base.PreOpen();
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			this.anyGameFiles = GenFilePaths.AllSavedGameFiles.Any<FileInfo>();
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x001AB7C9 File Offset: 0x001A9BC9
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			VersionControl.DrawInfoInCorner();
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x001AB7D7 File Offset: 0x001A9BD7
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			MainMenuDrawer.DoMainMenuControls(rect, this.anyGameFiles);
		}

		// Token: 0x04001AA1 RID: 6817
		private bool anyGameFiles;
	}
}
