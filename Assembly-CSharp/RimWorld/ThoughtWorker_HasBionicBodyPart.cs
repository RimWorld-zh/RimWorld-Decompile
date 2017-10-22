using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_HasBionicBodyPart : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			int num = 0;
			ThoughtState result;
			while (true)
			{
				if (num < hediffs.Count)
				{
					AddedBodyPartProps addedPartProps = hediffs[num].def.addedPartProps;
					if (addedPartProps != null && addedPartProps.isBionic)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
