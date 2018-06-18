using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007CD RID: 1997
	public class Designator_Dropdown : Designator
	{
		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002C31 RID: 11313 RVA: 0x00175524 File Offset: 0x00173924
		public override string Label
		{
			get
			{
				return this.activeDesignator.Label + ((!this.activeDesignatorSet) ? "..." : "");
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002C32 RID: 11314 RVA: 0x00175564 File Offset: 0x00173964
		public override string Desc
		{
			get
			{
				return this.activeDesignator.Desc;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002C33 RID: 11315 RVA: 0x00175584 File Offset: 0x00173984
		public override Color IconDrawColor
		{
			get
			{
				return this.activeDesignator.IconDrawColor;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002C34 RID: 11316 RVA: 0x001755A4 File Offset: 0x001739A4
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

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002C35 RID: 11317 RVA: 0x001755F8 File Offset: 0x001739F8
		public List<Designator> Elements
		{
			get
			{
				return this.elements;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002C36 RID: 11318 RVA: 0x00175614 File Offset: 0x00173A14
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return this.activeDesignator.PanelReadoutTitleExtraRightMargin;
			}
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x00175634 File Offset: 0x00173A34
		public void Add(Designator des)
		{
			this.elements.Add(des);
			if (this.activeDesignator == null)
			{
				this.SetActiveDesignator(des, false);
			}
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x00175658 File Offset: 0x00173A58
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

		// Token: 0x06002C39 RID: 11321 RVA: 0x001756C2 File Offset: 0x00173AC2
		public override void DrawMouseAttachments()
		{
			this.activeDesignator.DrawMouseAttachments();
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x001756D0 File Offset: 0x00173AD0
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

		// Token: 0x06002C3B RID: 11323 RVA: 0x001757B8 File Offset: 0x00173BB8
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			return this.activeDesignator.CanDesignateCell(loc);
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x001757D9 File Offset: 0x00173BD9
		public override void SelectedUpdate()
		{
			this.activeDesignator.SelectedUpdate();
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x001757E7 File Offset: 0x00173BE7
		public override void DrawPanelReadout(ref float curY, float width)
		{
			this.activeDesignator.DrawPanelReadout(ref curY, width);
		}

		// Token: 0x0400179F RID: 6047
		private List<Designator> elements = new List<Designator>();

		// Token: 0x040017A0 RID: 6048
		private Designator activeDesignator = null;

		// Token: 0x040017A1 RID: 6049
		private bool activeDesignatorSet = false;
	}
}
