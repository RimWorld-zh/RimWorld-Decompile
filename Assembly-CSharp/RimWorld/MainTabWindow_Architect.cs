using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200086B RID: 2155
	public class MainTabWindow_Architect : MainTabWindow
	{
		// Token: 0x06003104 RID: 12548 RVA: 0x001AA2EF File Offset: 0x001A86EF
		public MainTabWindow_Architect()
		{
			this.CacheDesPanels();
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x001AA30C File Offset: 0x001A870C
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

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06003106 RID: 12550 RVA: 0x001AA350 File Offset: 0x001A8750
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(200f, this.WinHeight);
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06003107 RID: 12551 RVA: 0x001AA378 File Offset: 0x001A8778
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x001AA392 File Offset: 0x001A8792
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x001AA3AB File Offset: 0x001A87AB
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			if (this.selectedDesPanel != null && this.selectedDesPanel.def.showPowerGrid)
			{
				OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
			}
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x001AA3DB File Offset: 0x001A87DB
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (this.selectedDesPanel != null)
			{
				this.selectedDesPanel.DesignationTabOnGUI();
			}
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x001AA3FC File Offset: 0x001A87FC
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

		// Token: 0x0600310C RID: 12556 RVA: 0x001AA540 File Offset: 0x001A8940
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

		// Token: 0x0600310D RID: 12557 RVA: 0x001AA5D4 File Offset: 0x001A89D4
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

		// Token: 0x04001A7E RID: 6782
		private List<ArchitectCategoryTab> desPanelsCached = null;

		// Token: 0x04001A7F RID: 6783
		public ArchitectCategoryTab selectedDesPanel = null;

		// Token: 0x04001A80 RID: 6784
		public const float WinWidth = 200f;

		// Token: 0x04001A81 RID: 6785
		private const float ButHeight = 32f;
	}
}
