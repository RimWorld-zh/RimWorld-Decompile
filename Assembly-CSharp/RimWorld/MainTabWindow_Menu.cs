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
		// Token: 0x06003141 RID: 12609 RVA: 0x001AB822 File Offset: 0x001A9C22
		public MainTabWindow_Menu()
		{
			this.forcePause = true;
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06003142 RID: 12610 RVA: 0x001AB834 File Offset: 0x001A9C34
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(450f, 390f);
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06003143 RID: 12611 RVA: 0x001AB858 File Offset: 0x001A9C58
		public override MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Right;
			}
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x001AB86E File Offset: 0x001A9C6E
		public override void PreOpen()
		{
			base.PreOpen();
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			this.anyGameFiles = GenFilePaths.AllSavedGameFiles.Any<FileInfo>();
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x001AB891 File Offset: 0x001A9C91
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			VersionControl.DrawInfoInCorner();
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x001AB89F File Offset: 0x001A9C9F
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			MainMenuDrawer.DoMainMenuControls(rect, this.anyGameFiles);
		}

		// Token: 0x04001AA1 RID: 6817
		private bool anyGameFiles;
	}
}
