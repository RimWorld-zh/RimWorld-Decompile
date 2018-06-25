using System;

namespace Verse
{
	// Token: 0x02000D49 RID: 3401
	public class NameSingle : Name
	{
		// Token: 0x0400327E RID: 12926
		private string nameInt;

		// Token: 0x0400327F RID: 12927
		private bool numerical;

		// Token: 0x06004AF8 RID: 19192 RVA: 0x00271CB0 File Offset: 0x002700B0
		public NameSingle()
		{
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x00271CB9 File Offset: 0x002700B9
		public NameSingle(string name, bool numerical = false)
		{
			this.nameInt = name;
			this.numerical = numerical;
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06004AFA RID: 19194 RVA: 0x00271CD0 File Offset: 0x002700D0
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06004AFB RID: 19195 RVA: 0x00271CEC File Offset: 0x002700EC
		public override string ToStringFull
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06004AFC RID: 19196 RVA: 0x00271D08 File Offset: 0x00270108
		public override string ToStringShort
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06004AFD RID: 19197 RVA: 0x00271D24 File Offset: 0x00270124
		public override bool IsValid
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06004AFE RID: 19198 RVA: 0x00271D48 File Offset: 0x00270148
		public override bool Numerical
		{
			get
			{
				return this.numerical;
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06004AFF RID: 19199 RVA: 0x00271D64 File Offset: 0x00270164
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
		// (get) Token: 0x06004B00 RID: 19200 RVA: 0x00271E04 File Offset: 0x00270204
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
		// (get) Token: 0x06004B01 RID: 19201 RVA: 0x00271E90 File Offset: 0x00270290
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

		// Token: 0x06004B02 RID: 19202 RVA: 0x00271EDD File Offset: 0x002702DD
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Values.Look<bool>(ref this.numerical, "numerical", false, false);
		}

		// Token: 0x06004B03 RID: 19203 RVA: 0x00271F04 File Offset: 0x00270304
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

		// Token: 0x06004B04 RID: 19204 RVA: 0x00271F70 File Offset: 0x00270370
		public override string ToString()
		{
			return this.nameInt;
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x00271F8C File Offset: 0x0027038C
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

		// Token: 0x06004B06 RID: 19206 RVA: 0x00271FD8 File Offset: 0x002703D8
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.nameInt.GetHashCode(), 1384661390);
		}
	}
}
