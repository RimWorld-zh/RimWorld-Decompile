using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002DC RID: 732
	public class StorytellerDef : Def
	{
		// Token: 0x0400076C RID: 1900
		public int listOrder = 9999;

		// Token: 0x0400076D RID: 1901
		public bool listVisible = true;

		// Token: 0x0400076E RID: 1902
		public bool tutorialMode = false;

		// Token: 0x0400076F RID: 1903
		public bool disableAdaptiveTraining = false;

		// Token: 0x04000770 RID: 1904
		public bool disableAlerts = false;

		// Token: 0x04000771 RID: 1905
		public bool disablePermadeath = false;

		// Token: 0x04000772 RID: 1906
		public DifficultyDef forcedDifficulty = null;

		// Token: 0x04000773 RID: 1907
		[NoTranslate]
		private string portraitLarge;

		// Token: 0x04000774 RID: 1908
		[NoTranslate]
		private string portraitTiny;

		// Token: 0x04000775 RID: 1909
		public List<StorytellerCompProperties> comps = new List<StorytellerCompProperties>();

		// Token: 0x04000776 RID: 1910
		public float desiredPopulationMin = 3f;

		// Token: 0x04000777 RID: 1911
		public float desiredPopulationMax = 10f;

		// Token: 0x04000778 RID: 1912
		public float desiredPopulationCritical = 13f;

		// Token: 0x04000779 RID: 1913
		public SimpleCurve populationIntentFromPopCurve;

		// Token: 0x0400077A RID: 1914
		public SimpleCurve populationIntentFromTimeCurve;

		// Token: 0x0400077B RID: 1915
		[Unsaved]
		public Texture2D portraitLargeTex;

		// Token: 0x0400077C RID: 1916
		[Unsaved]
		public Texture2D portraitTinyTex;

		// Token: 0x06000C10 RID: 3088 RVA: 0x0006B130 File Offset: 0x00069530
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

		// Token: 0x06000C11 RID: 3089 RVA: 0x0006B188 File Offset: 0x00069588
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
