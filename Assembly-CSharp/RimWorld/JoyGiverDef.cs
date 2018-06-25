using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002AB RID: 683
	public class JoyGiverDef : Def
	{
		// Token: 0x0400065B RID: 1627
		public Type giverClass = null;

		// Token: 0x0400065C RID: 1628
		public float baseChance = 0f;

		// Token: 0x0400065D RID: 1629
		public List<ThingDef> thingDefs = null;

		// Token: 0x0400065E RID: 1630
		public JobDef jobDef;

		// Token: 0x0400065F RID: 1631
		public bool desireSit = true;

		// Token: 0x04000660 RID: 1632
		public float pctPawnsEverDo = 1f;

		// Token: 0x04000661 RID: 1633
		public bool unroofedOnly = false;

		// Token: 0x04000662 RID: 1634
		public JoyKindDef joyKind;

		// Token: 0x04000663 RID: 1635
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x04000664 RID: 1636
		public bool canDoWhileInBed;

		// Token: 0x04000665 RID: 1637
		private JoyGiver workerInt = null;

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x00066F64 File Offset: 0x00065364
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

		// Token: 0x06000B6B RID: 2923 RVA: 0x00066FB0 File Offset: 0x000653B0
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
	}
}
