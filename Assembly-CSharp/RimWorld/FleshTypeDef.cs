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
		public ThoughtDef ateDirect;

		public ThoughtDef ateAsIngredient;

		public ThingCategoryDef corpseCategory;

		public List<FleshTypeDef.Wound> wounds;

		private List<Material> woundsResolved;

		[CompilerGenerated]
		private static Func<FleshTypeDef.Wound, Material> <>f__am$cache0;

		public FleshTypeDef()
		{
		}

		public Material ChooseWoundOverlay()
		{
			if (this.wounds == null)
			{
				return null;
			}
			if (this.woundsResolved == null)
			{
				this.woundsResolved = (from wound in this.wounds
				select wound.GetMaterial()).ToList<Material>();
			}
			return this.woundsResolved.RandomElement<Material>();
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
