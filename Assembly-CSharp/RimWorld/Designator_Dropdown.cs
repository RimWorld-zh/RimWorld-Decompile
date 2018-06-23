using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C9 RID: 1993
	public class Designator_Dropdown : Designator
	{
		// Token: 0x0400179D RID: 6045
		private List<Designator> elements = new List<Designator>();

		// Token: 0x0400179E RID: 6046
		private Designator activeDesignator = null;

		// Token: 0x0400179F RID: 6047
		private bool activeDesignatorSet = false;

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002C2A RID: 11306 RVA: 0x001756FC File Offset: 0x00173AFC
		public override string Label
		{
			get
			{
				return this.activeDesignator.Label + ((!this.activeDesignatorSet) ? "..." : "");
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002C2B RID: 11307 RVA: 0x0017573C File Offset: 0x00173B3C
		public override string Desc
		{
			get
			{
				return this.activeDesignator.Desc;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002C2C RID: 11308 RVA: 0x0017575C File Offset: 0x00173B5C
		public override Color IconDrawColor
		{
			get
			{
				return this.activeDesignator.IconDrawColor;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002C2D RID: 11309 RVA: 0x0017577C File Offset: 0x00173B7C
		public override bool Visible
		{
			get
			{
				for (int i = 0; i < this.elements.Count; i++)
				{
					if (this.elements[i].Visible)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002C2E RID: 11310 RVA: 0x001757D0 File Offset: 0x00173BD0
		public List<Designator> Elements
		{
			get
			{
				return this.elements;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002C2F RID: 11311 RVA: 0x001757EC File Offset: 0x00173BEC
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return this.activeDesignator.PanelReadoutTitleExtraRightMargin;
			}
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x0017580C File Offset: 0x00173C0C
		public void Add(Designator des)
		{
			this.elements.Add(des);
			if (this.activeDesignator == null)
			{
				this.SetActiveDesignator(des, false);
			}
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x00175830 File Offset: 0x00173C30
		public void SetActiveDesignator(Designator des, bool explicitySet = true)
		{
			this.activeDesignator = des;
			this.icon = des.icon;
			this.iconDrawScale = des.iconDrawScale;
			this.iconProportions = des.iconProportions;
			this.iconTexCoords = des.iconTexCoords;
			this.iconAngle = des.iconAngle;
			this.iconOffset = des.iconOffset;
			if (explicitySet)
			{
				this.activeDesignatorSet = true;
			}
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x0017589A File Offset: 0x00173C9A
		public override void DrawMouseAttachments()
		{
			this.activeDesignator.DrawMouseAttachments();
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x001758A8 File Offset: 0x00173CA8
		public override void ProcessInput(Event ev)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			for (int i = 0; i < this.elements.Count; i++)
			{
				Designator des = this.elements[i];
				if (des.Visible)
				{
					list.Add(new FloatMenuOption(des.LabelCap, delegate()
					{
						this.<ProcessInput>__BaseCallProxy0(ev);
						Find.DesignatorManager.Select(des);
						this.SetActiveDesignator(des, true);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
			}
			FloatMenu floatMenu = new FloatMenu(list);
			floatMenu.vanishIfMouseDistant = true;
			floatMenu.onCloseCallback = delegate()
			{
				this.activeDesignatorSet = true;
			};
			Find.WindowStack.Add(floatMenu);
			Find.DesignatorManager.Select(this.activeDesignator);
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x00175990 File Offset: 0x00173D90
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			return this.activeDesignator.CanDesignateCell(loc);
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x001759B1 File Offset: 0x00173DB1
		public override void SelectedUpdate()
		{
			this.activeDesignator.SelectedUpdate();
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x001759BF File Offset: 0x00173DBF
		public override void DrawPanelReadout(ref float curY, float width)
		{
			this.activeDesignator.DrawPanelReadout(ref curY, width);
		}
	}
}
