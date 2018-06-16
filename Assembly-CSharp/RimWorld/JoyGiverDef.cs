using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002A9 RID: 681
	public class JoyGiverDef : Def
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x00066DB0 File Offset: 0x000651B0
		public JoyGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (JoyGiver)Activator.CreateInstance(this.giverClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00066DFC File Offset: 0x000651FC
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.jobDef != null && this.jobDef.joyKind != this.joyKind)
			{
				yield return string.Concat(new object[]
				{
					"jobDef ",
					this.jobDef,
					" has joyKind ",
					this.jobDef.joyKind,
					" which does not match our joyKind ",
					this.joyKind
				});
			}
			yield break;
		}

		// Token: 0x0400065A RID: 1626
		public Type giverClass = null;

		// Token: 0x0400065B RID: 1627
		public float baseChance = 0f;

		// Token: 0x0400065C RID: 1628
		public List<ThingDef> thingDefs = null;

		// Token: 0x0400065D RID: 1629
		public JobDef jobDef;

		// Token: 0x0400065E RID: 1630
		public bool desireSit = true;

		// Token: 0x0400065F RID: 1631
		public float pctPawnsEverDo = 1f;

		// Token: 0x04000660 RID: 1632
		public bool unroofedOnly = false;

		// Token: 0x04000661 RID: 1633
		public JoyKindDef joyKind;

		// Token: 0x04000662 RID: 1634
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x04000663 RID: 1635
		public bool canDoWhileInBed;

		// Token: 0x04000664 RID: 1636
		private JoyGiver workerInt = null;
	}
}
