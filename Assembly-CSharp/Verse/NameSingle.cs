using System;

namespace Verse
{
	// Token: 0x02000D4B RID: 3403
	public class NameSingle : Name
	{
		// Token: 0x06004AE2 RID: 19170 RVA: 0x00270650 File Offset: 0x0026EA50
		public NameSingle()
		{
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x00270659 File Offset: 0x0026EA59
		public NameSingle(string name, bool numerical = false)
		{
			this.nameInt = name;
			this.numerical = numerical;
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06004AE4 RID: 19172 RVA: 0x00270670 File Offset: 0x0026EA70
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06004AE5 RID: 19173 RVA: 0x0027068C File Offset: 0x0026EA8C
		public override string ToStringFull
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06004AE6 RID: 19174 RVA: 0x002706A8 File Offset: 0x0026EAA8
		public override string ToStringShort
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06004AE7 RID: 19175 RVA: 0x002706C4 File Offset: 0x0026EAC4
		public override bool IsValid
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06004AE8 RID: 19176 RVA: 0x002706E8 File Offset: 0x0026EAE8
		public override bool Numerical
		{
			get
			{
				return this.numerical;
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06004AE9 RID: 19177 RVA: 0x00270704 File Offset: 0x0026EB04
		private int FirstDigitPosition
		{
			get
			{
				int result;
				if (!this.numerical)
				{
					result = -1;
				}
				else if (this.nameInt.NullOrEmpty() || !char.IsDigit(this.nameInt[this.nameInt.Length - 1]))
				{
					result = -1;
				}
				else
				{
					for (int i = this.nameInt.Length - 2; i >= 0; i--)
					{
						if (!char.IsDigit(this.nameInt[i]))
						{
							return i + 1;
						}
					}
					result = 0;
				}
				return result;
			}
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06004AEA RID: 19178 RVA: 0x002707A4 File Offset: 0x0026EBA4
		public string NameWithoutNumber
		{
			get
			{
				string result;
				if (!this.numerical)
				{
					result = this.nameInt;
				}
				else
				{
					int firstDigitPosition = this.FirstDigitPosition;
					if (firstDigitPosition < 0)
					{
						result = this.nameInt;
					}
					else
					{
						int num = firstDigitPosition;
						if (num - 1 >= 0 && this.nameInt[num - 1] == ' ')
						{
							num--;
						}
						if (num <= 0)
						{
							result = "";
						}
						else
						{
							result = this.nameInt.Substring(0, num);
						}
					}
				}
				return result;
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06004AEB RID: 19179 RVA: 0x00270830 File Offset: 0x0026EC30
		public int Number
		{
			get
			{
				int result;
				if (!this.numerical)
				{
					result = 0;
				}
				else
				{
					int firstDigitPosition = this.FirstDigitPosition;
					if (firstDigitPosition < 0)
					{
						result = 0;
					}
					else
					{
						result = int.Parse(this.nameInt.Substring(firstDigitPosition));
					}
				}
				return result;
			}
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x0027087D File Offset: 0x0026EC7D
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Values.Look<bool>(ref this.numerical, "numerical", false, false);
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x002708A4 File Offset: 0x0026ECA4
		public override bool ConfusinglySimilarTo(Name other)
		{
			NameSingle nameSingle = other as NameSingle;
			bool result;
			if (nameSingle != null && nameSingle.nameInt == this.nameInt)
			{
				result = true;
			}
			else
			{
				NameTriple nameTriple = other as NameTriple;
				result = (nameTriple != null && nameTriple.Nick == this.nameInt);
			}
			return result;
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x00270910 File Offset: 0x0026ED10
		public override string ToString()
		{
			return this.nameInt;
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x0027092C File Offset: 0x0026ED2C
		public override bool Equals(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = false;
			}
			else if (!(obj is NameSingle))
			{
				result = false;
			}
			else
			{
				NameSingle nameSingle = (NameSingle)obj;
				result = (this.nameInt == nameSingle.nameInt);
			}
			return result;
		}

		// Token: 0x06004AF0 RID: 19184 RVA: 0x00270978 File Offset: 0x0026ED78
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.nameInt.GetHashCode(), 1384661390);
		}

		// Token: 0x04003275 RID: 12917
		private string nameInt;

		// Token: 0x04003276 RID: 12918
		private bool numerical;
	}
}
