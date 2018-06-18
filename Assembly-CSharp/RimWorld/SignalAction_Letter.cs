using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C0 RID: 1728
	public class SignalAction_Letter : SignalAction
	{
		// Token: 0x0600252C RID: 9516 RVA: 0x0013EA1E File Offset: 0x0013CE1E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<Letter>(ref this.letter, "letter", new object[0]);
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x0013EA40 File Offset: 0x0013CE40
		protected override void DoAction(object[] args)
		{
			Pawn pawn = null;
			if (args != null && args.Any<object>())
			{
				pawn = (args[0] as Pawn);
			}
			if (pawn != null)
			{
				ChoiceLetter choiceLetter = this.letter as ChoiceLetter;
				if (choiceLetter != null)
				{
					choiceLetter.text = string.Format(choiceLetter.text, pawn.LabelShort).AdjustedFor(pawn);
				}
				if (!this.letter.lookTargets.IsValid())
				{
					this.letter.lookTargets = pawn;
				}
			}
			Find.LetterStack.ReceiveLetter(this.letter, null);
		}

		// Token: 0x0400148D RID: 5261
		public Letter letter;
	}
}
