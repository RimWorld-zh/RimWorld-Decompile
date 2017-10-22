using RimWorld;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class StuffProperties
	{
		public string stuffAdjective = (string)null;

		public float commonality = 1f;

		public List<StuffCategoryDef> categories = new List<StuffCategoryDef>();

		public bool smeltable = false;

		public List<StatModifier> statOffsets = null;

		public List<StatModifier> statFactors = null;

		public Color color = new Color(0.8f, 0.8f, 0.8f);

		public EffecterDef constructEffect = null;

		public StuffAppearanceDef appearance = StuffAppearanceDefOf.Smooth;

		public bool allowColorGenerators = false;

		public SoundDef soundImpactStuff = null;

		public SoundDef soundMeleeHitSharp = null;

		public SoundDef soundMeleeHitBlunt = null;

		public bool CanMake(ThingDef t)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < t.stuffCategories.Count)
				{
					for (int i = 0; i < this.categories.Count; i++)
					{
						if (t.stuffCategories[num] == this.categories[i])
							goto IL_002e;
					}
					num++;
					continue;
				}
				result = false;
				break;
				IL_002e:
				result = true;
				break;
			}
			return result;
		}
	}
}
