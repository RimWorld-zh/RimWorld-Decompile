using System;
using System.Collections.Generic;

namespace Verse
{
	public interface IVerbOwner
	{
		VerbTracker VerbTracker { get; }

		List<VerbProperties> VerbProperties { get; }

		List<Tool> Tools { get; }

		ImplementOwnerTypeDef ImplementOwnerTypeDef { get; }

		string UniqueVerbOwnerID();

		bool VerbsStillUsableBy(Pawn p);

		Thing ConstantCaster { get; }
	}
}
