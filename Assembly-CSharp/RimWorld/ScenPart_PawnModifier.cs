using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063A RID: 1594
	public class ScenPart_PawnModifier : ScenPart
	{
		// Token: 0x060020DB RID: 8411 RVA: 0x00117968 File Offset: 0x00115D68
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.chance, "chance", 0f, false);
			Scribe_Values.Look<PawnGenerationContext>(ref this.context, "context", PawnGenerationContext.All, false);
			Scribe_Values.Look<bool>(ref this.hideOffMap, "hideOffMap", false, false);
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x001179B8 File Offset: 0x00115DB8
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

		// Token: 0x060020DD RID: 8413 RVA: 0x00117B54 File Offset: 0x00115F54
		public override void Randomize()
		{
			this.chance = GenMath.RoundedHundredth(Rand.Range(0.05f, 1f));
			this.context = PawnGenerationContextUtility.GetRandom();
			this.hideOffMap = false;
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x00117B84 File Offset: 0x00115F84
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

		// Token: 0x060020DF RID: 8415 RVA: 0x00117BE8 File Offset: 0x00115FE8
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

		// Token: 0x060020E0 RID: 8416 RVA: 0x00117C4C File Offset: 0x0011604C
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

		// Token: 0x060020E1 RID: 8417 RVA: 0x00117D00 File Offset: 0x00116100
		protected virtual void ModifyNewPawn(Pawn p)
		{
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x00117D03 File Offset: 0x00116103
		protected virtual void ModifyPawnPostGenerate(Pawn p, bool redressed)
		{
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x00117D06 File Offset: 0x00116106
		protected virtual void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
		}

		// Token: 0x040012CF RID: 4815
		protected float chance = 1f;

		// Token: 0x040012D0 RID: 4816
		protected PawnGenerationContext context = PawnGenerationContext.All;

		// Token: 0x040012D1 RID: 4817
		protected bool hideOffMap;

		// Token: 0x040012D2 RID: 4818
		private string chanceBuf;
	}
}
