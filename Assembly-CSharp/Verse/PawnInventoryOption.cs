using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B5B RID: 2907
	public class PawnInventoryOption
	{
		// Token: 0x04002A32 RID: 10802
		public ThingDef thingDef;

		// Token: 0x04002A33 RID: 10803
		public IntRange countRange = IntRange.one;

		// Token: 0x04002A34 RID: 10804
		public float choiceChance = 1f;

		// Token: 0x04002A35 RID: 10805
		public float skipChance;

		// Token: 0x04002A36 RID: 10806
		public List<PawnInventoryOption> subOptionsTakeAll = null;

		// Token: 0x04002A37 RID: 10807
		public List<PawnInventoryOption> subOptionsChooseOne = null;

		// Token: 0x06003F89 RID: 16265 RVA: 0x00217610 File Offset: 0x00215A10
		public IEnumerable<Thing> GenerateThings()
		{
			if (Rand.Value < this.skipChance)
			{
				yield break;
			}
			if (this.thingDef != null && this.countRange.max > 0)
			{
				Thing thing = ThingMaker.MakeThing(this.thingDef, null);
				thing.stackCount = this.countRange.RandomInRange;
				yield return thing;
			}
			if (this.subOptionsTakeAll != null)
			{
				foreach (PawnInventoryOption opt in this.subOptionsTakeAll)
				{
					foreach (Thing subThing in opt.GenerateThings())
					{
						yield return subThing;
					}
				}
			}
			if (this.subOptionsChooseOne != null)
			{
				PawnInventoryOption chosen = this.subOptionsChooseOne.RandomElementByWeight((PawnInventoryOption o) => o.choiceChance);
				foreach (Thing subThing2 in chosen.GenerateThings())
				{
					yield return subThing2;
				}
			}
			yield break;
		}
	}
}
