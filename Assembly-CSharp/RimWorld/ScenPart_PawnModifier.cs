using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000638 RID: 1592
	public class ScenPart_PawnModifier : ScenPart
	{
		// Token: 0x040012CC RID: 4812
		protected float chance = 1f;

		// Token: 0x040012CD RID: 4813
		protected PawnGenerationContext context = PawnGenerationContext.All;

		// Token: 0x040012CE RID: 4814
		protected bool hideOffMap;

		// Token: 0x040012CF RID: 4815
		private string chanceBuf;

		// Token: 0x060020D7 RID: 8407 RVA: 0x00117B64 File Offset: 0x00115F64
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.chance, "chance", 0f, false);
			Scribe_Values.Look<PawnGenerationContext>(ref this.context, "context", PawnGenerationContext.All, false);
			Scribe_Values.Look<bool>(ref this.hideOffMap, "hideOffMap", false, false);
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x00117BB4 File Offset: 0x00115FB4
		protected void DoPawnModifierEditInterface(Rect rect)
		{
			Rect rect2 = rect.TopHalf();
			Rect rect3 = rect2.LeftPart(0.333f).Rounded();
			Rect rect4 = rect2.RightPart(0.666f).Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect3, "chance".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldPercent(rect4, ref this.chance, ref this.chanceBuf, 0f, 1f);
			Rect rect5 = rect.BottomHalf();
			Rect rect6 = rect5.LeftPart(0.333f).Rounded();
			Rect rect7 = rect5.RightPart(0.666f).Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect6, "context".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonText(rect7, this.context.ToStringHuman(), true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				IEnumerator enumerator = Enum.GetValues(typeof(PawnGenerationContext)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						PawnGenerationContext localCont2 = (PawnGenerationContext)obj;
						PawnGenerationContext localCont = localCont2;
						list.Add(new FloatMenuOption(localCont.ToStringHuman(), delegate()
						{
							this.context = localCont;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x00117D50 File Offset: 0x00116150
		public override void Randomize()
		{
			this.chance = GenMath.RoundedHundredth(Rand.Range(0.05f, 1f));
			this.context = PawnGenerationContextUtility.GetRandom();
			this.hideOffMap = false;
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x00117D80 File Offset: 0x00116180
		public override void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
			if (this.context.Includes(context))
			{
				if (!this.hideOffMap || context != PawnGenerationContext.PlayerStarter)
				{
					if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
					{
						this.ModifyNewPawn(pawn);
					}
				}
			}
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x00117DE4 File Offset: 0x001161E4
		public override void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
			if (this.context.Includes(context))
			{
				if (!this.hideOffMap || context != PawnGenerationContext.PlayerStarter)
				{
					if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
					{
						this.ModifyPawnPostGenerate(pawn, redressed);
					}
				}
			}
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x00117E48 File Offset: 0x00116248
		public override void PostMapGenerate(Map map)
		{
			if (Find.GameInitData != null)
			{
				if (this.hideOffMap && this.context.Includes(PawnGenerationContext.PlayerStarter))
				{
					foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
					{
						if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
						{
							this.ModifyHideOffMapStartingPawnPostMapGenerate(pawn);
						}
					}
				}
			}
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x00117EFC File Offset: 0x001162FC
		protected virtual void ModifyNewPawn(Pawn p)
		{
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x00117EFF File Offset: 0x001162FF
		protected virtual void ModifyPawnPostGenerate(Pawn p, bool redressed)
		{
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00117F02 File Offset: 0x00116302
		protected virtual void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
		}
	}
}
