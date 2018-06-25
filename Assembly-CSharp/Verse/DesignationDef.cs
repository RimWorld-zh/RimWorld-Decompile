using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public class DesignationDef : Def
	{
		[NoTranslate]
		public string texturePath;

		public TargetType targetType;

		public bool removeIfBuildingDespawned = false;

		public bool designateCancelable = true;

		[Unsaved]
		public Material iconMat;

		public DesignationDef()
		{
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.iconMat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.MetaOverlay);
			});
		}

		[CompilerGenerated]
		private void <ResolveReferences>m__0()
		{
			this.iconMat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.MetaOverlay);
		}
	}
}
