using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	public class SkillRequirement
	{
		public SkillDef skill;

		public int minLevel;

		public SkillRequirement()
		{
		}

		public string Summary
		{
			get
			{
				string result;
				if (this.skill == null)
				{
					result = "";
				}
				else
				{
					result = string.Format("{0} ({1})", this.skill.LabelCap, this.minLevel);
				}
				return result;
			}
		}

		public bool PawnSatisfies(Pawn pawn)
		{
			return pawn.skills != null && pawn.skills.GetSkill(this.skill).Level >= this.minLevel;
		}

		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name);
			this.minLevel = (int)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(int));
		}

		public override string ToString()
		{
			string result;
			if (this.skill == null)
			{
				result = "null-skill-requirement";
			}
			else
			{
				result = this.skill.defName + "-" + this.minLevel;
			}
			return result;
		}
	}
}
