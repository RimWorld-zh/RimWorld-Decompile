using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class FleshTypeDef : Def
	{
		public ThoughtDef ateDirect = null;

		public ThoughtDef ateAsIngredient = null;

		public ThingCategoryDef corpseCategory = null;

		public bool requiresBedForSurgery = true;

		public List<FleshTypeDef.Wound> wounds = null;

		private List<Material> woundsResolved = null;

		[CompilerGenerated]
		private static Func<FleshTypeDef.Wound, Material> <>f__am$cache0;

		public FleshTypeDef()
		{
		}

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
					select wound.GetMaterial()).ToList<Material>();
				}
				result = this.woundsResolved.RandomElement<Material>();
			}
			return result;
		}

		[CompilerGenerated]
		private static Material <ChooseWoundOverlay>m__0(FleshTypeDef.Wound wound)
		{
			return wound.GetMaterial();
		}

		public class Wound
		{
			[NoTranslate]
			public string texture;

			public Color color = Color.white;

			public Wound()
			{
			}

			public Material GetMaterial()
			{
				return MaterialPool.MatFrom(this.texture, ShaderDatabase.Cutout, this.color);
			}
		}
	}
}
