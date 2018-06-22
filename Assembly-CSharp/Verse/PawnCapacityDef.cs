using System;

namespace Verse
{
	// Token: 0x02000B55 RID: 2901
	public class PawnCapacityDef : Def
	{
		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06003F78 RID: 16248 RVA: 0x0021721C File Offset: 0x0021561C
		public PawnCapacityWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnCapacityWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x00217258 File Offset: 0x00215658
		public string GetLabelFor(Pawn pawn)
		{
			return this.GetLabelFor(pawn.RaceProps.IsFlesh, pawn.RaceProps.Humanlike);
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x0021728C File Offset: 0x0021568C
		public string GetLabelFor(bool isFlesh, bool isHumanlike)
		{
			string label;
			if (isHumanlike)
			{
				label = this.label;
			}
			else if (isFlesh)
			{
				if (!this.labelAnimals.NullOrEmpty())
				{
					label = this.labelAnimals;
				}
				else
				{
					label = this.label;
				}
			}
			else if (!this.labelMechanoids.NullOrEmpty())
			{
				label = this.labelMechanoids;
			}
			else
			{
				label = this.label;
			}
			return label;
		}

		// Token: 0x04002A1D RID: 10781
		public int listOrder = 0;

		// Token: 0x04002A1E RID: 10782
		public Type workerClass = typeof(PawnCapacityWorker);

		// Token: 0x04002A1F RID: 10783
		[MustTranslate]
		public string labelMechanoids = "";

		// Token: 0x04002A20 RID: 10784
		[MustTranslate]
		public string labelAnimals = "";

		// Token: 0x04002A21 RID: 10785
		public bool showOnHumanlikes = true;

		// Token: 0x04002A22 RID: 10786
		public bool showOnAnimals = true;

		// Token: 0x04002A23 RID: 10787
		public bool showOnMechanoids = true;

		// Token: 0x04002A24 RID: 10788
		public bool lethalFlesh = false;

		// Token: 0x04002A25 RID: 10789
		public bool lethalMechanoids = false;

		// Token: 0x04002A26 RID: 10790
		public float minForCapable = 0f;

		// Token: 0x04002A27 RID: 10791
		public float minValue = 0f;

		// Token: 0x04002A28 RID: 10792
		public bool zeroIfCannotBeAwake = false;

		// Token: 0x04002A29 RID: 10793
		public bool showOnCaravanHealthTab = false;

		// Token: 0x04002A2A RID: 10794
		[Unsaved]
		private PawnCapacityWorker workerInt;
	}
}
