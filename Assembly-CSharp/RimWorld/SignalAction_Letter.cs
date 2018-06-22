using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BC RID: 1724
	public class SignalAction_Letter : SignalAction
	{
		// Token: 0x06002524 RID: 9508 RVA: 0x0013EB66 File Offset: 0x0013CF66
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<Letter>(ref this.letter, "letter", new object[0]);
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x0013EB88 File Offset: 0x0013CF88
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
					choiceLetter.text = string.Format(choiceLetter.text, pawn.LabelShort).AdjustedFor(pawn, "PAWN");
				}
				if (!this.letter.lookTargets.IsValid())
				{
					this.letter.lookTargets = pawn;
				}
			}
			Find.LetterStack.ReceiveLetter(this.letter, null);
		}

		// Token: 0x0400148B RID: 5259
		public Letter letter;
	}
}
