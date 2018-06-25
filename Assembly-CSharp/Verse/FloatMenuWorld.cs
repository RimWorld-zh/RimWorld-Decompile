using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E5A RID: 3674
	public class FloatMenuWorld : FloatMenu
	{
		// Token: 0x04003961 RID: 14689
		private Vector2 clickPos;

		// Token: 0x060056A2 RID: 22178 RVA: 0x002CAB3B File Offset: 0x002C8F3B
		public FloatMenuWorld(List<FloatMenuOption> options, string title, Vector2 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x060056A3 RID: 22179 RVA: 0x002CAB50 File Offset: 0x002C8F50
		public override void DoWindowContents(Rect inRect)
		{
			Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
			if (caravan == null)
			{
				Find.WindowStack.TryRemove(this, true);
			}
			else
			{
				List<FloatMenuOption> curOpts = FloatMenuMakerWorld.ChoicesAtFor(this.clickPos, caravan);
				for (int i = 0; i < this.options.Count; i++)
				{
					if (!this.options[i].Disabled && !FloatMenuWorld.StillValid(this.options[i], curOpts))
					{
						this.options[i].Disabled = true;
					}
				}
				base.DoWindowContents(inRect);
			}
		}

		// Token: 0x060056A4 RID: 22180 RVA: 0x002CABF8 File Offset: 0x002C8FF8
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts)
		{
			if (opt.revalidateWorldClickTarget == null)
			{
				for (int i = 0; i < curOpts.Count; i++)
				{
					if (FloatMenuWorld.OptionsMatch(opt, curOpts[i]))
					{
						return true;
					}
				}
			}
			else
			{
				if (!opt.revalidateWorldClickTarget.Spawned)
				{
					return false;
				}
				Vector2 mousePos = opt.revalidateWorldClickTarget.ScreenPos();
				mousePos.y = (float)UI.screenHeight - mousePos.y;
				List<FloatMenuOption> list = FloatMenuMakerWorld.ChoicesAtFor(mousePos, (Caravan)Find.WorldSelector.SingleSelectedObject);
				for (int j = 0; j < list.Count; j++)
				{
					if (FloatMenuWorld.OptionsMatch(opt, list[j]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060056A5 RID: 22181 RVA: 0x002CACDC File Offset: 0x002C90DC
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}
	}
}
