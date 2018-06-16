using System;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E2D RID: 3629
	public abstract class Dialog_DebugOptionLister : Dialog_OptionLister
	{
		// Token: 0x060055E7 RID: 21991 RVA: 0x002B9B34 File Offset: 0x002B7F34
		protected bool DebugAction(string label, Action action)
		{
			bool result = false;
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.listing.ButtonDebug(label))
			{
				this.Close(true);
				action();
				result = true;
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
			return result;
		}

		// Token: 0x060055E8 RID: 21992 RVA: 0x002B9BC4 File Offset: 0x002B7FC4
		protected void DebugToolMap(string label, Action toolAction)
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				if (!base.FilterAllows(label))
				{
					GUI.color = new Color(1f, 1f, 1f, 0.3f);
				}
				if (this.listing.ButtonDebug(label))
				{
					this.Close(true);
					DebugTools.curTool = new DebugTool(label, toolAction, null);
				}
				GUI.color = Color.white;
				if (Event.current.type == EventType.Layout)
				{
					this.totalOptionsHeight += 24f;
				}
			}
		}

		// Token: 0x060055E9 RID: 21993 RVA: 0x002B9C60 File Offset: 0x002B8060
		protected void DebugToolMapForPawns(string label, Action<Pawn> pawnAction)
		{
			this.DebugToolMap(label, delegate
			{
				if (UI.MouseCell().InBounds(Find.CurrentMap))
				{
					foreach (Pawn obj in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>().ToList<Pawn>())
					{
						pawnAction(obj);
					}
				}
			});
		}

		// Token: 0x060055EA RID: 21994 RVA: 0x002B9C90 File Offset: 0x002B8090
		protected void DebugToolWorld(string label, Action toolAction)
		{
			if (WorldRendererUtility.WorldRenderedNow)
			{
				if (!base.FilterAllows(label))
				{
					GUI.color = new Color(1f, 1f, 1f, 0.3f);
				}
				if (this.listing.ButtonDebug(label))
				{
					this.Close(true);
					DebugTools.curTool = new DebugTool(label, toolAction, null);
				}
				GUI.color = Color.white;
				if (Event.current.type == EventType.Layout)
				{
					this.totalOptionsHeight += 24f;
				}
			}
		}

		// Token: 0x060055EB RID: 21995 RVA: 0x002B9D2C File Offset: 0x002B812C
		protected void CheckboxLabeledDebug(string label, ref bool checkOn)
		{
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			this.listing.LabelCheckboxDebug(label, ref checkOn);
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x060055EC RID: 21996 RVA: 0x002B9D9D File Offset: 0x002B819D
		protected void DoLabel(string label)
		{
			Text.Font = GameFont.Small;
			this.listing.Label(label, -1f, null);
			this.totalOptionsHeight += Text.CalcHeight(label, 300f) + 2f;
		}

		// Token: 0x060055ED RID: 21997 RVA: 0x002B9DD6 File Offset: 0x002B81D6
		protected void DoGap()
		{
			this.listing.Gap(7f);
			this.totalOptionsHeight += 7f;
		}

		// Token: 0x040038C3 RID: 14531
		private const float DebugOptionsGap = 7f;
	}
}
