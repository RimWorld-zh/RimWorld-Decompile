using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class StuffProperties
	{
		public string stuffAdjective = null;

		public float commonality = 1f;

		public List<StuffCategoryDef> categories = new List<StuffCategoryDef>();

		public bool smeltable = false;

		public List<StatModifier> statOffsets = null;

		public List<StatModifier> statFactors = null;

		public Color color = new Color(0.8f, 0.8f, 0.8f);

		public EffecterDef constructEffect = null;

		public StuffAppearanceDef appearance;

		public bool allowColorGenerators = false;

		public SoundDef soundImpactStuff = null;

		public SoundDef soundMeleeHitSharp = null;

		public SoundDef soundMeleeHitBlunt = null;

		public StuffProperties()
		{
		}

		public bool CanMake(BuildableDef t)
		{
			bool result;
			if (!t.MadeFromStuff)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < t.stuffCategories.Count; i++)
				{
					for (int j = 0; j < this.categories.Count; j++)
					{
						if (t.stuffCategories[i] == this.categories[j])
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		public void ResolveReferencesSpecial()
		{
			if (this.appearance == null)
			{
				this.appearance = StuffAppearanceDefOf.Smooth;
			}
		}
	}
}
