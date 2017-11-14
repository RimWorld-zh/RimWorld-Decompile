using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StockGenerator_SingleDef : StockGenerator
	{
		private ThingDef thingDef;

		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			using (IEnumerator<Thing> enumerator = StockGeneratorUtility.TryMakeForStock(this.thingDef, base.RandomCountOf(this.thingDef)).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Thing th = enumerator.Current;
					yield return th;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00d4:
			/*Error near IL_00d5: Unexpected return in MoveNext()*/;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			using (IEnumerator<string> enumerator = base.ConfigErrors(parentDef).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.thingDef.tradeability == Tradeability.Stockable)
				yield break;
			yield return this.thingDef + " is not Stockable";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0108:
			/*Error near IL_0109: Unexpected return in MoveNext()*/;
		}
	}
}
