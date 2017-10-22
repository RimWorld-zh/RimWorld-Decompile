using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StockGenerator_MultiDef : StockGenerator
	{
		private List<ThingDef> thingDefs = new List<ThingDef>();

		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			ThingDef td = this.thingDefs.RandomElement();
			using (IEnumerator<Thing> enumerator = StockGeneratorUtility.TryMakeForStock(td, base.RandomCountOf(td)).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Thing th = enumerator.Current;
					yield return th;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00e4:
			/*Error near IL_00e5: Unexpected return in MoveNext()*/;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return this.thingDefs.Contains(thingDef);
		}

		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0(parentDef).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			int i = 0;
			while (true)
			{
				if (i < this.thingDefs.Count)
				{
					if (this.thingDefs[i].tradeability == Tradeability.Stockable)
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return this.thingDefs[i] + " is not Stockable";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0159:
			/*Error near IL_015a: Unexpected return in MoveNext()*/;
		}
	}
}
