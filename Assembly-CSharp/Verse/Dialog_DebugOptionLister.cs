using System;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E2C RID: 3628
	public abstract class Dialog_DebugOptionLister : Dialog_OptionLister
	{
		// Token: 0x040038D7 RID: 14551
		private const float DebugOptionsGap = 7f;

		// Token: 0x06005605 RID: 22021 RVA: 0x002BBA04 File Offset: 0x002B9E04
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

		// Token: 0x06005606 RID: 22022 RVA: 0x002BBA94 File Offset: 0x002B9E94
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

		// Token: 0x06005607 RID: 22023 RVA: 0x002BBB30 File Offset: 0x002B9F30
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

		// Token: 0x06005608 RID: 22024 RVA: 0x002BBB60 File Offset: 0x002B9F60
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

		// Token: 0x06005609 RID: 22025 RVA: 0x002BBBFC File Offset: 0x002B9FFC
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

		// Token: 0x0600560A RID: 22026 RVA: 0x002BBC6D File Offset: 0x002BA06D
		protected void DoLabel(string label)
		{
			Text.Font = GameFont.Small;
			this.listing.Label(label, -1f, null);
			this.totalOptionsHeight += Text.CalcHeight(label, 300f) + 2f;
		}

		// Token: 0x0600560B RID: 22027 RVA: 0x002BBCA6 File Offset: 0x002BA0A6
		protected void DoGap()
		{
			this.listing.Gap(7f);
			this.totalOptionsHeight += 7f;
		}
	}
}
