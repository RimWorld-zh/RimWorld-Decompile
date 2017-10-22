using RimWorld;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class FloatMenuMap : FloatMenu
	{
		private Vector3 clickPos;

		public FloatMenuMap(List<FloatMenuOption> options, string title, Vector3 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		public override void DoWindowContents(Rect inRect)
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				Find.WindowStack.TryRemove(this, true);
			}
			else
			{
				List<FloatMenuOption> curOpts = FloatMenuMakerMap.ChoicesAtFor(this.clickPos, pawn);
				for (int i = 0; i < base.options.Count; i++)
				{
					if (!base.options[i].Disabled && !FloatMenuMap.StillValid(base.options[i], curOpts))
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
			if (opt.revalidateClickTarget == null)
			{
				for (int i = 0; i < curOpts.Count; i++)
				{
					if (FloatMenuMap.OptionsMatch(opt, curOpts[i]))
						goto IL_0027;
				}
			}
			else
			{
				if (!opt.revalidateClickTarget.Spawned)
				{
					result = false;
					goto IL_00c4;
				}
				List<FloatMenuOption> list = FloatMenuMakerMap.ChoicesAtFor(opt.revalidateClickTarget.Position.ToVector3Shifted(), Find.Selector.SingleSelectedThing as Pawn);
				for (int j = 0; j < list.Count; j++)
				{
					if (FloatMenuMap.OptionsMatch(opt, list[j]))
						goto IL_00a1;
				}
			}
			result = false;
			goto IL_00c4;
			IL_00c4:
			return result;
			IL_0027:
			result = true;
			goto IL_00c4;
			IL_00a1:
			result = true;
			goto IL_00c4;
		}

		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return (byte)((a.Label == b.Label) ? 1 : 0) != 0;
		}
	}
}
