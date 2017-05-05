using System;
using System.Collections.Generic;

namespace Verse
{
	public class StandardLetter : ChoiceLetter
	{
		protected override IEnumerable<DiaOption> Choices
		{
			get
			{
				StandardLetter.<>c__Iterator230 <>c__Iterator = new StandardLetter.<>c__Iterator230();
				<>c__Iterator.<>f__this = this;
				StandardLetter.<>c__Iterator230 expr_0E = <>c__Iterator;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}
	}
}
