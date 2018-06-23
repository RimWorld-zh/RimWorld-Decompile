using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002DA RID: 730
	public class StorytellerDef : Def
	{
		// Token: 0x04000769 RID: 1897
		public int listOrder = 9999;

		// Token: 0x0400076A RID: 1898
		public bool listVisible = true;

		// Token: 0x0400076B RID: 1899
		public bool tutorialMode = false;

		// Token: 0x0400076C RID: 1900
		public bool disableAdaptiveTraining = false;

		// Token: 0x0400076D RID: 1901
		public bool disableAlerts = false;

		// Token: 0x0400076E RID: 1902
		public bool disablePermadeath = false;

		// Token: 0x0400076F RID: 1903
		public DifficultyDef forcedDifficulty = null;

		// Token: 0x04000770 RID: 1904
		[NoTranslate]
		private string portraitLarge;

		// Token: 0x04000771 RID: 1905
		[NoTranslate]
		private string portraitTiny;

		// Token: 0x04000772 RID: 1906
		public List<StorytellerCompProperties> comps = new List<StorytellerCompProperties>();

		// Token: 0x04000773 RID: 1907
		public float desiredPopulationMin = 3f;

		// Token: 0x04000774 RID: 1908
		public float desiredPopulationMax = 10f;

		// Token: 0x04000775 RID: 1909
		public float desiredPopulationCritical = 13f;

		// Token: 0x04000776 RID: 1910
		public SimpleCurve populationIntentFromPopCurve;

		// Token: 0x04000777 RID: 1911
		public SimpleCurve populationIntentFromTimeCurve;

		// Token: 0x04000778 RID: 1912
		[Unsaved]
		public Texture2D portraitLargeTex;

		// Token: 0x04000779 RID: 1913
		[Unsaved]
		public Texture2D portraitTinyTex;

		// Token: 0x06000C0D RID: 3085 RVA: 0x0006AFD8 File Offset: 0x000693D8
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.portraitTiny.NullOrEmpty())
				{
					this.portraitTinyTex = ContentFinder<Texture2D>.Get(this.portraitTiny, true);
					this.portraitLargeTex = ContentFinder<Texture2D>.Get(this.portraitLarge, true);
				}
			});
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].ResolveReferences(this);
			}
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0006B030 File Offset: 0x00069430
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (string e2 in this.comps[i].ConfigErrors(this))
				{
					yield return e2;
				}
			}
			yield break;
		}
	}
}
