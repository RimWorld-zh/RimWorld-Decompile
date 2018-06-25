using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000838 RID: 2104
	public class Page_SelectStorytellerInGame : Page
	{
		// Token: 0x040019B8 RID: 6584
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();

		// Token: 0x06002FA1 RID: 12193 RVA: 0x00197D7E File Offset: 0x0019617E
		public Page_SelectStorytellerInGame()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06002FA2 RID: 12194 RVA: 0x00197DA0 File Offset: 0x001961A0
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x00197DC0 File Offset: 0x001961C0
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
	}
}
