using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class TrainableDef : Def
	{
		public float difficulty = -1f;

		public float minBodySize = 0f;

		public List<TrainableDef> prerequisites = null;

		[NoTranslate]
		public List<string> tags = new List<string>();

		public bool defaultTrainable = false;

		public TrainableIntelligenceDef requiredTrainableIntelligence;

		public int steps = 1;

		public float listPriority = 0f;

		public string icon;

		[Unsaved]
		public int indent = 0;

		[Unsaved]
		private Texture2D iconTex;

		public Texture2D Icon
		{
			get
			{
				if ((Object)this.iconTex == (Object)null)
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		public bool MatchesTag(string tag)
		{
			bool result;
			if (tag == base.defName)
			{
				result = true;
			}
			else
			{
				for (int i = 0; i < this.tags.Count; i++)
				{
					if (this.tags[i] == tag)
						goto IL_0038;
				}
				result = false;
			}
			goto IL_005c;
			IL_0038:
			result = true;
			goto IL_005c;
			IL_005c:
			return result;
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
			if (!(this.difficulty < 0.0))
				yield break;
			yield return "difficulty not set";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_00f5:
			/*Error near IL_00f6: Unexpected return in MoveNext()*/;
		}
	}
}
