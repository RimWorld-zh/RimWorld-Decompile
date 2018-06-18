using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200086F RID: 2159
	public class MainTabWindow_Architect : MainTabWindow
	{
		// Token: 0x0600310B RID: 12555 RVA: 0x001AA107 File Offset: 0x001A8507
		public MainTabWindow_Architect()
		{
			this.CacheDesPanels();
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x0600310C RID: 12556 RVA: 0x001AA124 File Offset: 0x001A8524
		public float WinHeight
		{
			get
			{
				if (this.desPanelsCached == null)
				{
					this.CacheDesPanels();
				}
				return (float)Mathf.CeilToInt((float)this.desPanelsCached.Count / 2f) * 32f;
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x0600310D RID: 12557 RVA: 0x001AA168 File Offset: 0x001A8568
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(200f, this.WinHeight);
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x0600310E RID: 12558 RVA: 0x001AA190 File Offset: 0x001A8590
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x001AA1AA File Offset: 0x001A85AA
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x001AA1C3 File Offset: 0x001A85C3
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			if (this.selectedDesPanel != null && this.selectedDesPanel.def.showPowerGrid)
			{
				OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
			}
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x001AA1F3 File Offset: 0x001A85F3
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (this.selectedDesPanel != null)
			{
				this.selectedDesPanel.DesignationTabOnGUI();
			}
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x001AA214 File Offset: 0x001A8614
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			Text.Font = GameFont.Small;
			float num = inRect.width / 2f;
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < this.desPanelsCached.Count; i++)
			{
				Rect rect = new Rect(num2 * num, num3 * 32f, num, 32f);
				rect.height += 1f;
				if (num2 == 0f)
				{
					rect.width += 1f;
				}
				if (Widgets.ButtonTextSubtle(rect, this.desPanelsCached[i].def.LabelCap, 0f, 8f, SoundDefOf.Mouseover_Category, new Vector2(-1f, -1f)))
				{
					this.ClickedCategory(this.desPanelsCached[i]);
				}
				if (this.selectedDesPanel != this.desPanelsCached[i])
				{
					UIHighlighter.HighlightOpportunity(rect, this.desPanelsCached[i].def.cachedHighlightClosedTag);
				}
				num2 += 1f;
				if (num2 > 1f)
				{
					num2 = 0f;
					num3 += 1f;
				}
			}
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x001AA358 File Offset: 0x001A8758
		private void CacheDesPanels()
		{
			this.desPanelsCached = new List<ArchitectCategoryTab>();
			foreach (DesignationCategoryDef def in from dc in DefDatabase<DesignationCategoryDef>.AllDefs
			orderby dc.order descending
			select dc)
			{
				this.desPanelsCached.Add(new ArchitectCategoryTab(def));
			}
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x001AA3EC File Offset: 0x001A87EC
		protected void ClickedCategory(ArchitectCategoryTab Pan)
		{
			if (this.selectedDesPanel == Pan)
			{
				this.selectedDesPanel = null;
			}
			else
			{
				this.selectedDesPanel = Pan;
			}
			SoundDefOf.ArchitectCategorySelect.PlayOneShotOnCamera(null);
		}

		// Token: 0x04001A80 RID: 6784
		private List<ArchitectCategoryTab> desPanelsCached = null;

		// Token: 0x04001A81 RID: 6785
		public ArchitectCategoryTab selectedDesPanel = null;

		// Token: 0x04001A82 RID: 6786
		public const float WinWidth = 200f;

		// Token: 0x04001A83 RID: 6787
		private const float ButHeight = 32f;
	}
}
