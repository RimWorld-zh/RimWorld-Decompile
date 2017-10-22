using System.Collections.Generic;

namespace Verse
{
	public class StandardLetter : ChoiceLetter
	{
		protected override IEnumerable<DiaOption> Choices
		{
			get
			{
				yield return base.OK;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
