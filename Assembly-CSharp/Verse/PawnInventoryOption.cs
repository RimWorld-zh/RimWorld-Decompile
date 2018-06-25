using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B5C RID: 2908
	public class PawnInventoryOption
	{
		// Token: 0x04002A39 RID: 10809
		public ThingDef thingDef;

		// Token: 0x04002A3A RID: 10810
		public IntRange countRange = IntRange.one;

		// Token: 0x04002A3B RID: 10811
		public float choiceChance = 1f;

		// Token: 0x04002A3C RID: 10812
		public float skipChance;

		// Token: 0x04002A3D RID: 10813
		public List<PawnInventoryOption> subOptionsTakeAll = null;

		// Token: 0x04002A3E RID: 10814
		public List<PawnInventoryOption> subOptionsChooseOne = null;

		// Token: 0x06003F89 RID: 16265 RVA: 0x002178F0 File Offset: 0x00215CF0
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
