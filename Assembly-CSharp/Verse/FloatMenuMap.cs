using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E55 RID: 3669
	public class FloatMenuMap : FloatMenu
	{
		// Token: 0x06005689 RID: 22153 RVA: 0x002CA133 File Offset: 0x002C8533
		public FloatMenuMap(List<FloatMenuOption> options, string title, Vector3 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x0600568A RID: 22154 RVA: 0x002CA148 File Offset: 0x002C8548
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

		// Token: 0x0600568B RID: 22155 RVA: 0x002CA208 File Offset: 0x002C8608
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

		// Token: 0x0600568C RID: 22156 RVA: 0x002CA2FC File Offset: 0x002C86FC
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}

		// Token: 0x0400393C RID: 14652
		private Vector3 clickPos;
	}
}
