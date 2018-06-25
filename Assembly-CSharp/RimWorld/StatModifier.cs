using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	public class StatModifier
	{
		public StatDef stat;

		public float value;

		public StatModifier()
		{
		}

		public string ValueToStringAsOffset
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Offset);
			}
		}

		public string ToStringAsFactor
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Factor);
			}
		}

		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "stat", xmlRoot.Name);
			this.value = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}

		public override string ToString()
		{
			string result;
			if (this.stat == null)
			{
				result = "(null stat)";
			}
			else
			{
				result = this.stat.defName + "-" + this.value.ToString();
			}
			return result;
		}
	}
}
