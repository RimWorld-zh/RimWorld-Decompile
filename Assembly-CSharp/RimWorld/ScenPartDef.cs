using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ScenPartDef : Def
	{
		public ScenPartCategory category = ScenPartCategory.Undefined;

		public Type scenPartClass = null;

		public float summaryPriority = -1f;

		public float selectionWeight = 1f;

		public int maxUses = 999999;

		public Type pageClass;

		public GameConditionDef gameCondition;

		public FloatRange durationRandomRange = new FloatRange(30f, 100f);

		public Type designatorType;

		public bool PlayerAddRemovable
		{
			get
			{
				return this.category != ScenPartCategory.Fixed;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.scenPartClass != null)
				yield break;
			yield return "scenPartClass is null";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_00f0:
			/*Error near IL_00f1: Unexpected return in MoveNext()*/;
		}
	}
}
