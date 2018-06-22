using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000836 RID: 2102
	public class Page_SelectStorytellerInGame : Page
	{
		// Token: 0x06002F9D RID: 12189 RVA: 0x00197C2E File Offset: 0x0019602E
		public Page_SelectStorytellerInGame()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06002F9E RID: 12190 RVA: 0x00197C50 File Offset: 0x00196050
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x00197C70 File Offset: 0x00196070
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			Storyteller storyteller = Current.Game.storyteller;
			StorytellerDef def = Current.Game.storyteller.def;
			StorytellerUI.DrawStorytellerSelectionInterface(mainRect, ref storyteller.def, ref storyteller.difficulty, this.selectedStorytellerInfoListing);
			if (storyteller.def != def)
			{
				storyteller.Notify_DefChanged();
			}
		}

		// Token: 0x040019B8 RID: 6584
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
