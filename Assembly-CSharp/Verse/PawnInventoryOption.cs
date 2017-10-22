using System;
using System.Collections.Generic;

namespace Verse
{
	public class PawnInventoryOption
	{
		public ThingDef thingDef;

		public IntRange countRange = IntRange.one;

		public float choiceChance = 1f;

		public float skipChance;

		public List<PawnInventoryOption> subOptionsTakeAll;

		public List<PawnInventoryOption> subOptionsChooseOne;

		public IEnumerable<Thing> GenerateThings()
		{
			if (!(Rand.Value < this.skipChance))
			{
				if (this.thingDef != null && this.countRange.max > 0)
				{
					Thing thing = ThingMaker.MakeThing(this.thingDef, null);
					thing.stackCount = this.countRange.RandomInRange;
					yield return thing;
				}
				if (this.subOptionsTakeAll != null)
				{
					List<PawnInventoryOption>.Enumerator enumerator = this.subOptionsTakeAll.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							PawnInventoryOption opt = enumerator.Current;
							foreach (Thing item in opt.GenerateThings())
							{
								yield return item;
							}
						}
					}
					finally
					{
						((IDisposable)(object)enumerator).Dispose();
					}
				}
				if (this.subOptionsChooseOne != null)
				{
					PawnInventoryOption chosen = this.subOptionsChooseOne.RandomElementByWeight((Func<PawnInventoryOption, float>)((PawnInventoryOption o) => o.choiceChance));
					foreach (Thing item2 in chosen.GenerateThings())
					{
						yield return item2;
					}
				}
			}
		}
	}
}
