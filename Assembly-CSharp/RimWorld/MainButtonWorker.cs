using System;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	// Token: 0x020002AF RID: 687
	public abstract class MainButtonWorker
	{
		// Token: 0x04000684 RID: 1668
		public MainButtonDef def;

		// Token: 0x04000685 RID: 1669
		private const float CompactModeMargin = 2f;

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x000675B8 File Offset: 0x000659B8
		public virtual float ButtonBarPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06000B7D RID: 2941
		public abstract void Activate();

		// Token: 0x06000B7E RID: 2942 RVA: 0x000675D4 File Offset: 0x000659D4
		public virtual void InterfaceTryActivate()
		{
			if (!TutorSystem.TutorialMode || !this.def.canBeTutorDenied || Find.MainTabsRoot.OpenTab == this.def || TutorSystem.AllowAction("MainTab-" + this.def.defName + "-Open"))
			{
				this.Activate();
			}
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00067648 File Offset: 0x00065A48
		public virtual void DoButton(Rect rect)
		{
			Text.Font = GameFont.Small;
			Profiler.BeginSample("lab");
			string text = this.def.LabelCap;
			Profiler.EndSample();
			float num = this.def.LabelCapWidth;
			if (num > rect.width - 2f)
			{
				text = this.def.ShortenedLabelCap;
				num = this.def.ShortenedLabelCapWidth;
			}
			if ((!this.def.validWithoutMap || this.def == MainButtonDefOf.World) && Find.CurrentMap == null)
			{
				Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
				if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
				{
					Event.current.Use();
				}
			}
			else
			{
				Profiler.BeginSample("ButtonTextSubtle");
				bool flag = num > 0.85f * rect.width - 1f;
				Rect rect2 = rect;
				string label = text;
				float textLeftMargin = (!flag) ? -1f : 2f;
				if (Widgets.ButtonTextSubtle(rect2, label, this.ButtonBarPercent, textLeftMargin, SoundDefOf.Mouseover_Category, default(Vector2)))
				{
					this.InterfaceTryActivate();
				}
				Profiler.EndSample();
				if (Find.MainTabsRoot.OpenTab != this.def && !Find.WindowStack.NonImmediateDialogWindowOpen)
				{
					UIHighlighter.HighlightOpportunity(rect, this.def.cachedHighlightTagClosed);
				}
				if (!this.def.description.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect, this.def.description);
				}
			}
		}
	}
}
