using System;

namespace Verse
{
	// Token: 0x02000D4A RID: 3402
	public class NameSingle : Name
	{
		// Token: 0x06004AE0 RID: 19168 RVA: 0x00270628 File Offset: 0x0026EA28
		public NameSingle()
		{
		}

		// Token: 0x06004AE1 RID: 19169 RVA: 0x00270631 File Offset: 0x0026EA31
		public NameSingle(string name, bool numerical = false)
		{
			this.nameInt = name;
			this.numerical = numerical;
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06004AE2 RID: 19170 RVA: 0x00270648 File Offset: 0x0026EA48
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06004AE3 RID: 19171 RVA: 0x00270664 File Offset: 0x0026EA64
		public override string ToStringFull
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06004AE4 RID: 19172 RVA: 0x00270680 File Offset: 0x0026EA80
		public override string ToStringShort
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06004AE5 RID: 19173 RVA: 0x0027069C File Offset: 0x0026EA9C
		public override bool IsValid
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06004AE6 RID: 19174 RVA: 0x002706C0 File Offset: 0x0026EAC0
		public override bool Numerical
		{
			get
			{
				return this.numerical;
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06004AE7 RID: 19175 RVA: 0x002706DC File Offset: 0x0026EADC
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

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06004AE8 RID: 19176 RVA: 0x0027077C File Offset: 0x0026EB7C
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

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06004AE9 RID: 19177 RVA: 0x00270808 File Offset: 0x0026EC08
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

		// Token: 0x06004AEA RID: 19178 RVA: 0x00270855 File Offset: 0x0026EC55
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Values.Look<bool>(ref this.numerical, "numerical", false, false);
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x0027087C File Offset: 0x0026EC7C
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

		// Token: 0x06004AEC RID: 19180 RVA: 0x002708E8 File Offset: 0x0026ECE8
		public override string ToString()
		{
			return this.nameInt;
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x00270904 File Offset: 0x0026ED04
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

		// Token: 0x06004AEE RID: 19182 RVA: 0x00270950 File Offset: 0x0026ED50
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.nameInt.GetHashCode(), 1384661390);
		}

		// Token: 0x04003273 RID: 12915
		private string nameInt;

		// Token: 0x04003274 RID: 12916
		private bool numerical;
	}
}
