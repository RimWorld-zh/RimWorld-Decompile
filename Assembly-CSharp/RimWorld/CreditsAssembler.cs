using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CreditsAssembler
	{
		public static IEnumerable<CreditsEntry> AllCredits()
		{
			yield return (CreditsEntry)new CreditRecord_Space(200f);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
