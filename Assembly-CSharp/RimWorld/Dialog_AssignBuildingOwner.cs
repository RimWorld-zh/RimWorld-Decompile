using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Dialog_AssignBuildingOwner : Window
	{
		private const float EntryHeight = 35f;

		private IAssignableBuilding assignable;

		private Vector2 scrollPosition;

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(620f, 500f);
			}
		}

		public Dialog_AssignBuildingOwner(IAssignableBuilding assignable)
		{
			this.assignable = assignable;
			base.closeOnEscapeKey = true;
			base.doCloseButton = true;
			base.doCloseX = true;
			base.closeOnClickedOutside = true;
			base.absorbInputAroundWindow = true;
		}

		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect);
			outRect.yMin += 20f;
			outRect.yMax -= 40f;
			outRect.width -= 16f;
			Rect viewRect = new Rect(0f, 0f, (float)(outRect.width - 16.0), (float)((float)this.assignable.AssigningCandidates.Count() * 35.0 + 100.0));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			bool flag = false;
			foreach (Pawn assignedPawn in this.assignable.AssignedPawns)
			{
				flag = true;
				Rect rect = new Rect(0f, num, (float)(viewRect.width * 0.60000002384185791), 32f);
				Widgets.Label(rect, assignedPawn.LabelCap);
				rect.x = rect.xMax;
				rect.width = (float)(viewRect.width * 0.40000000596046448);
				if (Widgets.ButtonText(rect, "BuildingUnassign".Translate(), true, false, true))
				{
					this.assignable.TryUnassignPawn(assignedPawn);
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					return;
				}
				num = (float)(num + 35.0);
			}
			if (flag)
			{
				num = (float)(num + 15.0);
			}
			using (IEnumerator<Pawn> enumerator2 = this.assignable.AssigningCandidates.GetEnumerator())
			{
				Pawn current2;
				while (enumerator2.MoveNext())
				{
					current2 = enumerator2.Current;
					if (!this.assignable.AssignedPawns.Contains(current2))
					{
						Rect rect2 = new Rect(0f, num, (float)(viewRect.width * 0.60000002384185791), 32f);
						Widgets.Label(rect2, current2.LabelCap);
						rect2.x = rect2.xMax;
						rect2.width = (float)(viewRect.width * 0.40000000596046448);
						if (Widgets.ButtonText(rect2, "BuildingAssign".Translate(), true, false, true))
							goto IL_0217;
						num = (float)(num + 35.0);
					}
				}
				goto end_IL_0185;
				IL_0217:
				this.assignable.TryAssignPawn(current2);
				if (this.assignable.MaxAssignedPawnsCount == 1)
				{
					this.Close(true);
				}
				else
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
				return;
				end_IL_0185:;
			}
			Widgets.EndScrollView();
		}
	}
}
