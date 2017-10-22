using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class FloatMenuWorld : FloatMenu
	{
		private Vector2 clickPos;

		public FloatMenuWorld(List<FloatMenuOption> options, string title, Vector2 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

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
				for (int i = 0; i < base.options.Count; i++)
				{
					if (!base.options[i].Disabled && !FloatMenuWorld.StillValid(base.options[i], curOpts))
					{
						base.options[i].Disabled = true;
					}
				}
				base.DoWindowContents(inRect);
			}
		}

		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts)
		{
			bool result;
			if (opt.revalidateWorldClickTarget == null)
			{
				for (int i = 0; i < curOpts.Count; i++)
				{
					if (FloatMenuWorld.OptionsMatch(opt, curOpts[i]))
						goto IL_0027;
				}
			}
			else
			{
				if (!opt.revalidateWorldClickTarget.Spawned)
				{
					result = false;
					goto IL_00d3;
				}
				Vector2 mousePos = opt.revalidateWorldClickTarget.ScreenPos();
				mousePos.y = (float)UI.screenHeight - mousePos.y;
				List<FloatMenuOption> list = FloatMenuMakerWorld.ChoicesAtFor(mousePos, (Caravan)Find.WorldSelector.SingleSelectedObject);
				for (int j = 0; j < list.Count; j++)
				{
					if (FloatMenuWorld.OptionsMatch(opt, list[j]))
						goto IL_00b0;
				}
			}
			result = false;
			goto IL_00d3;
			IL_00d3:
			return result;
			IL_0027:
			result = true;
			goto IL_00d3;
			IL_00b0:
			result = true;
			goto IL_00d3;
		}

		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return (byte)((a.Label == b.Label) ? 1 : 0) != 0;
		}
	}
}
