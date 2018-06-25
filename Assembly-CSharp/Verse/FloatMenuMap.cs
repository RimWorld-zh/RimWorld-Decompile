using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E57 RID: 3671
	public class FloatMenuMap : FloatMenu
	{
		// Token: 0x0400393C RID: 14652
		private Vector3 clickPos;

		// Token: 0x0600568D RID: 22157 RVA: 0x002CA25F File Offset: 0x002C865F
		public FloatMenuMap(List<FloatMenuOption> options, string title, Vector3 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x0600568E RID: 22158 RVA: 0x002CA274 File Offset: 0x002C8674
		public override void DoWindowContents(Rect inRect)
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				Find.WindowStack.TryRemove(this, true);
			}
			else
			{
				List<FloatMenuOption> list = FloatMenuMakerMap.ChoicesAtFor(this.clickPos, pawn);
				List<FloatMenuOption> list2 = list;
				Vector3 vector = this.clickPos;
				for (int i = 0; i < this.options.Count; i++)
				{
					if (!this.options[i].Disabled && !FloatMenuMap.StillValid(this.options[i], list, pawn, ref list2, ref vector))
					{
						this.options[i].Disabled = true;
					}
				}
				base.DoWindowContents(inRect);
			}
		}

		// Token: 0x0600568F RID: 22159 RVA: 0x002CA334 File Offset: 0x002C8734
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Pawn forPawn, ref List<FloatMenuOption> cachedChoices, ref Vector3 cachedChoicesForPos)
		{
			if (opt.revalidateClickTarget == null)
			{
				for (int i = 0; i < curOpts.Count; i++)
				{
					if (FloatMenuMap.OptionsMatch(opt, curOpts[i]))
					{
						return true;
					}
				}
			}
			else
			{
				if (!opt.revalidateClickTarget.Spawned)
				{
					return false;
				}
				Vector3 vector = opt.revalidateClickTarget.Position.ToVector3Shifted();
				List<FloatMenuOption> list;
				if (vector == cachedChoicesForPos)
				{
					list = cachedChoices;
				}
				else
				{
					cachedChoices = FloatMenuMakerMap.ChoicesAtFor(vector, forPawn);
					cachedChoicesForPos = vector;
					list = cachedChoices;
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (FloatMenuMap.OptionsMatch(opt, list[j]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005690 RID: 22160 RVA: 0x002CA428 File Offset: 0x002C8828
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}
	}
}
