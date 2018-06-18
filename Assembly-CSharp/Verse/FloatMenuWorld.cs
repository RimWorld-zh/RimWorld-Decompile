using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E59 RID: 3673
	public class FloatMenuWorld : FloatMenu
	{
		// Token: 0x0600567E RID: 22142 RVA: 0x002C8DFF File Offset: 0x002C71FF
		public FloatMenuWorld(List<FloatMenuOption> options, string title, Vector2 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x0600567F RID: 22143 RVA: 0x002C8E14 File Offset: 0x002C7214
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

		// Token: 0x06005680 RID: 22144 RVA: 0x002C8EBC File Offset: 0x002C72BC
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

		// Token: 0x06005681 RID: 22145 RVA: 0x002C8FA0 File Offset: 0x002C73A0
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}

		// Token: 0x04003952 RID: 14674
		private Vector2 clickPos;
	}
}
