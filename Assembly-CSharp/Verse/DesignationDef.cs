using System;
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

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished((Action)delegate
			{
				this.iconMat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.MetaOverlay);
			});
		}
	}
}
