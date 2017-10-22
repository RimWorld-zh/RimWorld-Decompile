using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class ScatterableDef : Def
	{
		[NoTranslate]
		public string texturePath;

		public List<string> relevantTerrains = new List<string>();

		public float minSize;

		public float maxSize;

		public float selectionWeight = 100f;

		[NoTranslate]
		public string scatterType = "";

		public Material mat;

		public override void PostLoad()
		{
			base.PostLoad();
			if (base.defName == "UnnamedDef")
			{
				base.defName = "Scatterable_" + this.texturePath;
			}
			LongEventHandler.ExecuteWhenFinished((Action)delegate
			{
				this.mat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.Transparent);
			});
		}
	}
}
