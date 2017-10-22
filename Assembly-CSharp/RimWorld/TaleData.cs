using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public abstract class TaleData : IExposable
	{
		public abstract void ExposeData();

		public virtual IEnumerable<Rule> GetRules(string prefix)
		{
			Log.Error(base.GetType() + " cannot do GetRules with a prefix.");
			yield break;
		}

		public virtual IEnumerable<Rule> GetRules()
		{
			Log.Error(base.GetType() + " cannot do GetRules without a prefix.");
			yield break;
		}
	}
}
