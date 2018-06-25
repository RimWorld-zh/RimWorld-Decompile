using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E9 RID: 745
	public class TrainableDef : Def
	{
		// Token: 0x040007D2 RID: 2002
		public float difficulty = -1f;

		// Token: 0x040007D3 RID: 2003
		public float minBodySize = 0f;

		// Token: 0x040007D4 RID: 2004
		public List<TrainableDef> prerequisites = null;

		// Token: 0x040007D5 RID: 2005
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x040007D6 RID: 2006
		public bool defaultTrainable = false;

		// Token: 0x040007D7 RID: 2007
		public TrainabilityDef requiredTrainability;

		// Token: 0x040007D8 RID: 2008
		public int steps = 1;

		// Token: 0x040007D9 RID: 2009
		public float listPriority = 0f;

		// Token: 0x040007DA RID: 2010
		[NoTranslate]
		public string icon;

		// Token: 0x040007DB RID: 2011
		[Unsaved]
		public int indent = 0;

		// Token: 0x040007DC RID: 2012
		[Unsaved]
		private Texture2D iconTex;

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0006D104 File Offset: 0x0006B504
		public Texture2D Icon
		{
			get
			{
				if (this.iconTex == null)
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x0006D144 File Offset: 0x0006B544
		public bool MatchesTag(string tag)
		{
			bool result;
			if (tag == this.defName)
			{
				result = true;
			}
			else
			{
				for (int i = 0; i < this.tags.Count; i++)
				{
					if (this.tags[i] == tag)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x0006D1B0 File Offset: 0x0006B5B0
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.difficulty < 0f)
			{
				yield return "difficulty not set";
			}
			yield break;
		}
	}
}
