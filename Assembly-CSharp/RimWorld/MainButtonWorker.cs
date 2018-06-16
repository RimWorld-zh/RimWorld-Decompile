using System;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	// Token: 0x020002AD RID: 685
	public abstract class MainButtonWorker
	{
		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x00067400 File Offset: 0x00065800
		public virtual float ButtonBarPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06000B7B RID: 2939
		public abstract void Activate();

		// Token: 0x06000B7C RID: 2940 RVA: 0x0006741C File Offset: 0x0006581C
		public virtual void InterfaceTryActivate()
		{
			if (!TutorSystem.TutorialMode || !this.def.canBeTutorDenied || Find.MainTabsRoot.OpenTab == this.def || TutorSystem.AllowAction("MainTab-" + this.def.defName + "-Open"))
			{
				this.Activate();
			}
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x00067490 File Offset: 0x00065890
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

		// Token: 0x04000685 RID: 1669
		public MainButtonDef def;

		// Token: 0x04000686 RID: 1670
		private const float CompactModeMargin = 2f;
	}
}
