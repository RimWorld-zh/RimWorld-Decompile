using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000664 RID: 1636
	public static class TaleRecorder
	{
		// Token: 0x0600223B RID: 8763 RVA: 0x001225E0 File Offset: 0x001209E0
		public static void RecordTale(TaleDef def, params object[] args)
		{
			bool flag = Rand.Value < def.ignoreChance;
			if (Rand.Value >= def.ignoreChance || DebugViewSettings.logTaleRecording)
			{
				if (def.colonistOnly)
				{
					bool flag2 = false;
					bool flag3 = false;
					for (int i = 0; i < args.Length; i++)
					{
						Pawn pawn = args[i] as Pawn;
						if (pawn != null)
						{
							flag2 = true;
							if (pawn.Faction == Faction.OfPlayer)
							{
								flag3 = true;
							}
						}
					}
					if (flag2 && !flag3)
					{
						return;
					}
				}
				Tale tale = TaleFactory.MakeRawTale(def, args);
				if (tale != null)
				{
					if (DebugViewSettings.logTaleRecording)
					{
						string format = "Tale {0} from {1}, targets {2}:\n{3}";
						object[] array = new object[4];
						array[0] = ((!flag) ? "recorded" : "ignored");
						array[1] = def;
						array[2] = (from arg in args
						select arg.ToStringSafe<object>()).ToCommaList(false);
						array[3] = TaleTextGenerator.GenerateTextFromTale(TextGenerationPurpose.ArtDescription, tale, Rand.Int, RulePackDefOf.ArtDescription_Sculpture);
						Log.Message(string.Format(format, array), false);
					}
					if (!flag)
					{
						Find.TaleManager.Add(tale);
						for (int j = 0; j < args.Length; j++)
						{
							Pawn pawn2 = args[j] as Pawn;
							if (pawn2 != null)
							{
								if (!pawn2.Dead && pawn2.needs.mood != null)
								{
									pawn2.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
								}
								pawn2.records.AccumulateStoryEvent(StoryEventDefOf.TaleCreated);
							}
						}
					}
				}
			}
		}
	}
}
