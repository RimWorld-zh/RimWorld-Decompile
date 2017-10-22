using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public static class GenLabel
	{
		private class LabelElement
		{
			public Thing thingTemplate;

			public int count;
		}

		private struct LabelRequest : IEquatable<LabelRequest>
		{
			public Thing thing;

			public BuildableDef entDef;

			public ThingDef stuffDef;

			public int stackCount;

			public QualityCategory quality;

			public int health;

			public int maxHealth;

			public bool wornByCorpse;

			public override bool Equals(object obj)
			{
				if (!(obj is LabelRequest))
				{
					return false;
				}
				return this.Equals((LabelRequest)obj);
			}

			public bool Equals(LabelRequest other)
			{
				return this.thing == other.thing && this.entDef == other.entDef && this.stuffDef == other.stuffDef && this.stackCount == other.stackCount && this.quality == other.quality && this.health == other.health && this.maxHealth == other.maxHealth && this.wornByCorpse == other.wornByCorpse;
			}

			public override int GetHashCode()
			{
				int seed = 0;
				seed = Gen.HashCombine(seed, this.thing);
				seed = Gen.HashCombine(seed, this.entDef);
				seed = Gen.HashCombine(seed, this.stuffDef);
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef != null)
				{
					seed = Gen.HashCombineInt(seed, this.stackCount);
					QualityCategory qualityCategory = default(QualityCategory);
					if (this.thing != null && this.thing.TryGetQuality(out qualityCategory))
					{
						seed = Gen.HashCombineStruct(seed, this.quality);
					}
					if (thingDef.useHitPoints)
					{
						seed = Gen.HashCombineInt(seed, this.health);
						seed = Gen.HashCombineInt(seed, this.maxHealth);
					}
					seed = Gen.HashCombineInt(seed, this.wornByCorpse ? 1 : 0);
				}
				return seed;
			}

			public static bool operator ==(LabelRequest lhs, LabelRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			public static bool operator !=(LabelRequest lhs, LabelRequest rhs)
			{
				return !(lhs == rhs);
			}
		}

		private const int LabelDictionaryMaxCount = 2000;

		private static Dictionary<LabelRequest, string> labelDictionary = new Dictionary<LabelRequest, string>();

		private static List<LabelElement> tmpThingsLabelElements = new List<LabelElement>();

		public static void ClearCache()
		{
			GenLabel.labelDictionary.Clear();
		}

		public static string ThingLabel(BuildableDef entDef, ThingDef stuffDef, int stackCount = 1)
		{
			LabelRequest key = new LabelRequest
			{
				entDef = entDef,
				stuffDef = stuffDef,
				stackCount = stackCount
			};
			string text = default(string);
			if (!GenLabel.labelDictionary.TryGetValue(key, out text))
			{
				if (GenLabel.labelDictionary.Count > 2000)
				{
					GenLabel.labelDictionary.Clear();
				}
				text = GenLabel.NewThingLabel(entDef, stuffDef, stackCount);
				GenLabel.labelDictionary.Add(key, text);
			}
			return text;
		}

		private static string NewThingLabel(BuildableDef entDef, ThingDef stuffDef, int stackCount)
		{
			string text = (stuffDef != null) ? "ThingMadeOfStuffLabel".Translate(stuffDef.LabelAsStuff, entDef.label) : entDef.label;
			if (stackCount != 1)
			{
				text = text + " x" + stackCount.ToStringCached();
			}
			return text;
		}

		public static string ThingLabel(Thing t)
		{
			LabelRequest key = new LabelRequest
			{
				thing = t,
				entDef = t.def,
				stuffDef = t.Stuff,
				stackCount = t.stackCount
			};
			t.TryGetQuality(out key.quality);
			if (t.def.useHitPoints)
			{
				key.health = t.HitPoints;
				key.maxHealth = t.MaxHitPoints;
			}
			Apparel apparel = t as Apparel;
			if (apparel != null)
			{
				key.wornByCorpse = apparel.WornByCorpse;
			}
			string text = default(string);
			if (!GenLabel.labelDictionary.TryGetValue(key, out text))
			{
				if (GenLabel.labelDictionary.Count > 2000)
				{
					GenLabel.labelDictionary.Clear();
				}
				text = GenLabel.NewThingLabel(t);
				GenLabel.labelDictionary.Add(key, text);
			}
			return text;
		}

		private static string NewThingLabel(Thing t)
		{
			string text = GenLabel.ThingLabel(t.def, t.Stuff, 1);
			QualityCategory cat = default(QualityCategory);
			bool flag = t.TryGetQuality(out cat);
			int hitPoints = t.HitPoints;
			int maxHitPoints = t.MaxHitPoints;
			bool flag2 = t.def.useHitPoints && hitPoints < maxHitPoints && t.def.stackLimit == 1;
			Apparel apparel = t as Apparel;
			bool flag3 = apparel != null && apparel.WornByCorpse;
			if (flag || flag2 || flag3)
			{
				text += " (";
				if (flag)
				{
					text += cat.GetLabel();
				}
				if (flag2)
				{
					if (flag)
					{
						text += " ";
					}
					text += ((float)hitPoints / (float)maxHitPoints).ToStringPercent();
				}
				if (flag3)
				{
					text = text + " " + "WornByCorpseChar".Translate();
				}
				text += ")";
			}
			return text;
		}

		public static string ThingsLabel(List<Thing> things)
		{
			GenLabel.tmpThingsLabelElements.Clear();
			List<Thing>.Enumerator enumerator = things.GetEnumerator();
			try
			{
				Thing thing;
				while (enumerator.MoveNext())
				{
					thing = enumerator.Current;
					LabelElement labelElement = (from elem in GenLabel.tmpThingsLabelElements
					where thing.def.stackLimit > 1 && elem.thingTemplate.def == thing.def && elem.thingTemplate.Stuff == thing.Stuff
					select elem).FirstOrDefault();
					if (labelElement != null)
					{
						labelElement.count += thing.stackCount;
					}
					else
					{
						GenLabel.tmpThingsLabelElements.Add(new LabelElement
						{
							thingTemplate = thing,
							count = thing.stackCount
						});
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			GenLabel.tmpThingsLabelElements.Sort((Comparison<LabelElement>)delegate(LabelElement lhs, LabelElement rhs)
			{
				int num = TransferableComparer_Category.Compare(lhs.thingTemplate.def, rhs.thingTemplate.def);
				if (num != 0)
				{
					return num;
				}
				return lhs.thingTemplate.MarketValue.CompareTo(rhs.thingTemplate.MarketValue);
			});
			StringBuilder stringBuilder = new StringBuilder();
			List<LabelElement>.Enumerator enumerator2 = GenLabel.tmpThingsLabelElements.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					LabelElement current = enumerator2.Current;
					string str = string.Empty;
					if (current.thingTemplate.ParentHolder is Pawn_ApparelTracker)
					{
						str = " (" + "WornBy".Translate((current.thingTemplate.ParentHolder.ParentHolder as Pawn).LabelShort) + ")";
					}
					else if (current.thingTemplate.ParentHolder is Pawn_EquipmentTracker)
					{
						str = " (" + "EquippedBy".Translate((current.thingTemplate.ParentHolder.ParentHolder as Pawn).LabelShort) + ")";
					}
					if (current.count == 1)
					{
						stringBuilder.AppendLine("  " + current.thingTemplate.LabelCap + str);
					}
					else
					{
						stringBuilder.AppendLine("  " + GenLabel.ThingLabel(current.thingTemplate.def, current.thingTemplate.Stuff, current.count).CapitalizeFirst() + str);
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
			GenLabel.tmpThingsLabelElements.Clear();
			return stringBuilder.ToString();
		}

		public static string BestKindLabel(Pawn pawn, bool mustNoteGender = false, bool mustNoteLifeStage = false, bool plural = false)
		{
			bool flag = false;
			bool flag2 = false;
			string text = (string)null;
			switch (pawn.gender)
			{
			case Gender.None:
			{
				if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelPlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelPlural;
					flag2 = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.label != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.label;
					flag2 = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text);
					}
				}
				else if (plural && pawn.kindDef.labelPlural != null)
				{
					text = pawn.kindDef.labelPlural;
				}
				else
				{
					text = pawn.kindDef.label;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text);
					}
				}
				break;
			}
			case Gender.Male:
			{
				if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelMalePlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelMalePlural;
					flag2 = true;
					flag = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelMale != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelMale;
					flag2 = true;
					flag = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text);
					}
				}
				else if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelPlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelPlural;
					flag2 = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.label != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.label;
					flag2 = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text);
					}
				}
				else if (plural && pawn.kindDef.labelMalePlural != null)
				{
					text = pawn.kindDef.labelMalePlural;
					flag = true;
				}
				else if (pawn.kindDef.labelMale != null)
				{
					text = pawn.kindDef.labelMale;
					flag = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text);
					}
				}
				else if (plural && pawn.kindDef.labelPlural != null)
				{
					text = pawn.kindDef.labelPlural;
				}
				else
				{
					text = pawn.kindDef.label;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text);
					}
				}
				break;
			}
			case Gender.Female:
			{
				if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelFemalePlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelFemalePlural;
					flag2 = true;
					flag = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelFemale != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelFemale;
					flag2 = true;
					flag = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text);
					}
				}
				else if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelPlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelPlural;
					flag2 = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.label != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.label;
					flag2 = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text);
					}
				}
				else if (plural && pawn.kindDef.labelFemalePlural != null)
				{
					text = pawn.kindDef.labelFemalePlural;
					flag = true;
				}
				else if (pawn.kindDef.labelFemale != null)
				{
					text = pawn.kindDef.labelFemale;
					flag = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text);
					}
				}
				else if (plural && pawn.kindDef.labelPlural != null)
				{
					text = pawn.kindDef.labelPlural;
				}
				else
				{
					text = pawn.kindDef.label;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text);
					}
				}
				break;
			}
			}
			if ((mustNoteGender ? ((!flag) ? pawn.gender : Gender.None) : Gender.None) != 0)
			{
				text = "PawnMainDescGendered".Translate(pawn.gender.GetLabel(), text);
			}
			if (mustNoteLifeStage && !flag2 && pawn.ageTracker != null && pawn.ageTracker.CurLifeStage.visible)
			{
				text = "PawnMainDescLifestageWrap".Translate(text, pawn.ageTracker.CurLifeStage.Adjective);
			}
			return text;
		}
	}
}
