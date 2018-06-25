using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public class ScatterableDef : Def
	{
		[NoTranslate]
		public string texturePath;

		public float minSize;

		public float maxSize;

		public float selectionWeight = 100f;

		[NoTranslate]
		public string scatterType = "";

		public Material mat;

		public ScatterableDef()
		{
		}

		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				this.defName = "Scatterable_" + this.texturePath;
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.Transparent);
			});
		}

		[CompilerGenerated]
		private void <PostLoad>m__0()
		{
			this.mat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.Transparent);
		}
	}
}
