using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Dropdown : Designator
	{
		private List<Designator> elements = new List<Designator>();

		private Designator activeDesignator = null;

		private bool activeDesignatorSet = false;

		public Designator_Dropdown()
		{
		}

		public override string Label
		{
			get
			{
				return this.activeDesignator.Label + ((!this.activeDesignatorSet) ? "..." : "");
			}
		}

		public override string Desc
		{
			get
			{
				return this.activeDesignator.Desc;
			}
		}

		public override Color IconDrawColor
		{
			get
			{
				return this.activeDesignator.IconDrawColor;
			}
		}

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

		public List<Designator> Elements
		{
			get
			{
				return this.elements;
			}
		}

		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return this.activeDesignator.PanelReadoutTitleExtraRightMargin;
			}
		}

		public void Add(Designator des)
		{
			this.elements.Add(des);
			if (this.activeDesignator == null)
			{
				this.SetActiveDesignator(des, false);
			}
		}

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

		public override void DrawMouseAttachments()
		{
			this.activeDesignator.DrawMouseAttachments();
		}

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

		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			return this.activeDesignator.CanDesignateCell(loc);
		}

		public override void SelectedUpdate()
		{
			this.activeDesignator.SelectedUpdate();
		}

		public override void DrawPanelReadout(ref float curY, float width)
		{
			this.activeDesignator.DrawPanelReadout(ref curY, width);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private void <ProcessInput>__BaseCallProxy0(Event ev)
		{
			base.ProcessInput(ev);
		}

		[CompilerGenerated]
		private sealed class <ProcessInput>c__AnonStorey0
		{
			internal Event ev;

			internal Designator_Dropdown $this;

			public <ProcessInput>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.$this.activeDesignatorSet = true;
			}
		}

		[CompilerGenerated]
		private sealed class <ProcessInput>c__AnonStorey1
		{
			internal Designator des;

			internal Designator_Dropdown.<ProcessInput>c__AnonStorey0 <>f__ref$0;

			public <ProcessInput>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.<>f__ref$0.$this.<ProcessInput>__BaseCallProxy0(this.<>f__ref$0.ev);
				Find.DesignatorManager.Select(this.des);
				this.<>f__ref$0.$this.SetActiveDesignator(this.des, true);
			}
		}
	}
}
