using RimWorld;
using System.Xml;

namespace Verse
{
	public class SkillRequirement
	{
		public SkillDef skill;

		public int minLevel;

		public string Summary
		{
			get
			{
				return (this.skill != null) ? string.Format("{0} ({1})", this.skill.LabelCap, this.minLevel) : "";
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
			return (this.skill != null) ? (this.skill.defName + "-" + this.minLevel) : "null-skill-requirement";
		}
	}
}
