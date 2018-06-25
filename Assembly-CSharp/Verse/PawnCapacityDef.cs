using System;

namespace Verse
{
	// Token: 0x02000B58 RID: 2904
	public class PawnCapacityDef : Def
	{
		// Token: 0x04002A24 RID: 10788
		public int listOrder = 0;

		// Token: 0x04002A25 RID: 10789
		public Type workerClass = typeof(PawnCapacityWorker);

		// Token: 0x04002A26 RID: 10790
		[MustTranslate]
		public string labelMechanoids = "";

		// Token: 0x04002A27 RID: 10791
		[MustTranslate]
		public string labelAnimals = "";

		// Token: 0x04002A28 RID: 10792
		public bool showOnHumanlikes = true;

		// Token: 0x04002A29 RID: 10793
		public bool showOnAnimals = true;

		// Token: 0x04002A2A RID: 10794
		public bool showOnMechanoids = true;

		// Token: 0x04002A2B RID: 10795
		public bool lethalFlesh = false;

		// Token: 0x04002A2C RID: 10796
		public bool lethalMechanoids = false;

		// Token: 0x04002A2D RID: 10797
		public float minForCapable = 0f;

		// Token: 0x04002A2E RID: 10798
		public float minValue = 0f;

		// Token: 0x04002A2F RID: 10799
		public bool zeroIfCannotBeAwake = false;

		// Token: 0x04002A30 RID: 10800
		public bool showOnCaravanHealthTab = false;

		// Token: 0x04002A31 RID: 10801
		[Unsaved]
		private PawnCapacityWorker workerInt;

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06003F7B RID: 16251 RVA: 0x002175D8 File Offset: 0x002159D8
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

		// Token: 0x06003F7C RID: 16252 RVA: 0x00217614 File Offset: 0x00215A14
		public string GetLabelFor(Pawn pawn)
		{
			return this.GetLabelFor(pawn.RaceProps.IsFlesh, pawn.RaceProps.Humanlike);
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x00217648 File Offset: 0x00215A48
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
	}
}
