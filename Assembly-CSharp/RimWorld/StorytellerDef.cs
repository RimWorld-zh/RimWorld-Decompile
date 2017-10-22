using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class StorytellerDef : Def
	{
		public int listOrder = 9999;

		public bool listVisible = true;

		public bool tutorialMode = false;

		public bool disableAdaptiveTraining = false;

		public bool disableAlerts = false;

		public bool disablePermadeath = false;

		public DifficultyDef forcedDifficulty = null;

		[NoTranslate]
		private string portraitLarge;

		[NoTranslate]
		private string portraitTiny;

		public List<StorytellerCompProperties> comps = new List<StorytellerCompProperties>();

		public float desiredPopulationMin = 3f;

		public float desiredPopulationMax = 10f;

		public float desiredPopulationCritical = 13f;

		public SimpleCurve populationIntentFromPopCurve;

		public SimpleCurve populationIntentFromTimeCurve;

		[Unsaved]
		public Texture2D portraitLargeTex;

		[Unsaved]
		public Texture2D portraitTinyTex;

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished((Action)delegate
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

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e2 = enumerator.Current;
					yield return e2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				using (IEnumerator<string> enumerator2 = this.comps[i].ConfigErrors(this).GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						string e = enumerator2.Current;
						yield return e;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_019e:
			/*Error near IL_019f: Unexpected return in MoveNext()*/;
		}
	}
}
