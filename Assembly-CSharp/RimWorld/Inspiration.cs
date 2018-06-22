using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200002B RID: 43
	public class Inspiration : IExposable
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000194 RID: 404 RVA: 0x000108C4 File Offset: 0x0000ECC4
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000195 RID: 405 RVA: 0x000108E0 File Offset: 0x0000ECE0
		public float AgeDays
		{
			get
			{
				return (float)this.age / 60000f;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00010904 File Offset: 0x0000ED04
		public virtual string InspectLine
		{
			get
			{
				int numTicks = (int)((this.def.baseDurationDays - this.AgeDays) * 60000f);
				return string.Concat(new string[]
				{
					this.def.baseInspectLine,
					" (",
					"ExpiresIn".Translate(),
					": ",
					numTicks.ToStringTicksToPeriod(),
					")"
				});
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0001097A File Offset: 0x0000ED7A
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<InspirationDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0001099F File Offset: 0x0000ED9F
		public virtual void InspirationTick()
		{
			this.age++;
			if (this.AgeDays >= this.def.baseDurationDays)
			{
				this.End();
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000109CC File Offset: 0x0000EDCC
		public virtual void PostStart()
		{
			this.SendBeginLetter();
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000109D5 File Offset: 0x0000EDD5
		public virtual void PostEnd()
		{
			this.AddEndMessage();
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000109DE File Offset: 0x0000EDDE
		protected void End()
		{
			this.pawn.mindState.inspirationHandler.EndInspiration(this);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000109F8 File Offset: 0x0000EDF8
		protected virtual void SendBeginLetter()
		{
			if (!this.def.beginLetter.NullOrEmpty())
			{
				if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					string text = string.Format(this.def.beginLetter.AdjustedFor(this.pawn, "PAWN"), this.pawn.LabelCap);
					Find.LetterStack.ReceiveLetter(this.def.beginLetterLabel, text, this.def.beginLetterDef, this.pawn, null, null);
				}
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00010A90 File Offset: 0x0000EE90
		protected virtual void AddEndMessage()
		{
			if (!this.def.endMessage.NullOrEmpty())
			{
				if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					string text = string.Format(this.def.endMessage.AdjustedFor(this.pawn, "PAWN"), this.pawn.LabelCap);
					Messages.Message(text, this.pawn, MessageTypeDefOf.NeutralEvent, true);
				}
			}
		}

		// Token: 0x040001A3 RID: 419
		public Pawn pawn;

		// Token: 0x040001A4 RID: 420
		public InspirationDef def;

		// Token: 0x040001A5 RID: 421
		private int age;
	}
}
