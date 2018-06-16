using System;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F6A RID: 3946
	public class ShaderParameter
	{
		// Token: 0x06005F38 RID: 24376 RVA: 0x00307DC4 File Offset: 0x003061C4
		public void Apply(Material mat)
		{
			ShaderParameter.Type type = this.type;
			if (type != ShaderParameter.Type.Float)
			{
				if (type != ShaderParameter.Type.Vector)
				{
					if (type == ShaderParameter.Type.Texture)
					{
						if (this.valueTex == null)
						{
							Log.ErrorOnce(string.Format("Texture for {0} is not yet loaded; file may be invalid, or main thread may not have loaded it yet", this.name), 27929440, false);
						}
						mat.SetTexture(this.name, this.valueTex);
					}
				}
				else
				{
					mat.SetVector(this.name, this.value);
				}
			}
			else
			{
				mat.SetFloat(this.name, this.value.x);
			}
		}

		// Token: 0x06005F39 RID: 24377 RVA: 0x00307E68 File Offset: 0x00306268
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ShaderParameter: " + xmlRoot.OuterXml, false);
			}
			else
			{
				this.name = xmlRoot.Name;
				string valstr = xmlRoot.FirstChild.Value;
				if (!valstr.NullOrEmpty() && valstr[0] == '(')
				{
					this.value = ParseHelper.FromStringVector4Adaptive(valstr);
					this.type = ShaderParameter.Type.Vector;
				}
				else if (!valstr.NullOrEmpty() && valstr[0] == '/')
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						this.valueTex = ContentFinder<Texture2D>.Get(valstr.TrimStart(new char[]
						{
							'/'
						}), true);
					});
					this.type = ShaderParameter.Type.Texture;
				}
				else
				{
					this.value = Vector4.one * (float)ParseHelper.FromString(valstr, typeof(float));
					this.type = ShaderParameter.Type.Float;
				}
			}
		}

		// Token: 0x04003EA1 RID: 16033
		[NoTranslate]
		private string name;

		// Token: 0x04003EA2 RID: 16034
		private Vector4 value;

		// Token: 0x04003EA3 RID: 16035
		private Texture2D valueTex;

		// Token: 0x04003EA4 RID: 16036
		private ShaderParameter.Type type;

		// Token: 0x02000F6B RID: 3947
		private enum Type
		{
			// Token: 0x04003EA6 RID: 16038
			Float,
			// Token: 0x04003EA7 RID: 16039
			Vector,
			// Token: 0x04003EA8 RID: 16040
			Matrix,
			// Token: 0x04003EA9 RID: 16041
			Texture
		}
	}
}
