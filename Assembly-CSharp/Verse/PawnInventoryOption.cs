using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B5D RID: 2909
	public class PawnInventoryOption
	{
		// Token: 0x06003F84 RID: 16260 RVA: 0x00216E34 File Offset: 0x00215234
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

		// Token: 0x04002A34 RID: 10804
		public ThingDef thingDef;

		// Token: 0x04002A35 RID: 10805
		public IntRange countRange = IntRange.one;

		// Token: 0x04002A36 RID: 10806
		public float choiceChance = 1f;

		// Token: 0x04002A37 RID: 10807
		public float skipChance;

		// Token: 0x04002A38 RID: 10808
		public List<PawnInventoryOption> subOptionsTakeAll = null;

		// Token: 0x04002A39 RID: 10809
		public List<PawnInventoryOption> subOptionsChooseOne = null;
	}
}
