using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002A9 RID: 681
	public class JoyGiverDef : Def
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x00066E18 File Offset: 0x00065218
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

		// Token: 0x06000B68 RID: 2920 RVA: 0x00066E64 File Offset: 0x00065264
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

		// Token: 0x04000659 RID: 1625
		public Type giverClass = null;

		// Token: 0x0400065A RID: 1626
		public float baseChance = 0f;

		// Token: 0x0400065B RID: 1627
		public List<ThingDef> thingDefs = null;

		// Token: 0x0400065C RID: 1628
		public JobDef jobDef;

		// Token: 0x0400065D RID: 1629
		public bool desireSit = true;

		// Token: 0x0400065E RID: 1630
		public float pctPawnsEverDo = 1f;

		// Token: 0x0400065F RID: 1631
		public bool unroofedOnly = false;

		// Token: 0x04000660 RID: 1632
		public JoyKindDef joyKind;

		// Token: 0x04000661 RID: 1633
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x04000662 RID: 1634
		public bool canDoWhileInBed;

		// Token: 0x04000663 RID: 1635
		private JoyGiver workerInt = null;
	}
}
