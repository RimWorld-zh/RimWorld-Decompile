using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class FleshTypeDef : Def
	{
		public class Wound
		{
			public string texture;

			public Color color = Color.white;

			public Material GetMaterial()
			{
				return MaterialPool.MatFrom(this.texture, ShaderDatabase.Cutout, this.color);
			}
		}

		public ThoughtDef ateDirect = null;

		public ThoughtDef ateAsIngredient = null;

		public ThingCategoryDef corpseCategory = null;

		public bool requiresBedForSurgery = true;

		public List<Wound> wounds = null;

		private List<Material> woundsResolved = null;

		public Material ChooseWoundOverlay()
		{
			Material result;
			if (this.wounds == null)
			{
				result = null;
			}
			else
			{
				if (this.woundsResolved == null)
				{
					this.woundsResolved = (from wound in this.wounds
					select wound.GetMaterial()).ToList();
				}
				result = this.woundsResolved.RandomElement();
			}
			return result;
		}
	}
}
