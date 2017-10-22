namespace Verse
{
	public class NameSingle : Name
	{
		private string nameInt;

		private bool numerical;

		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		public override string ToStringFull
		{
			get
			{
				return this.nameInt;
			}
		}

		public override string ToStringShort
		{
			get
			{
				return this.nameInt;
			}
		}

		public override bool IsValid
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		public override bool Numerical
		{
			get
			{
				return this.numerical;
			}
		}

		private int FirstDigitPosition
		{
			get
			{
				int result;
				int num;
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
					for (num = this.nameInt.Length - 2; num >= 0; num--)
					{
						if (!char.IsDigit(this.nameInt[num]))
							goto IL_0076;
					}
					result = 0;
				}
				goto IL_0092;
				IL_0092:
				return result;
				IL_0076:
				result = num + 1;
				goto IL_0092;
			}
		}

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
						result = ((num > 0) ? this.nameInt.Substring(0, num) : "");
					}
				}
				return result;
			}
		}

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
					result = ((firstDigitPosition >= 0) ? int.Parse(this.nameInt.Substring(firstDigitPosition)) : 0);
				}
				return result;
			}
		}

		public NameSingle()
		{
		}

		public NameSingle(string name, bool numerical = false)
		{
			this.nameInt = name;
			this.numerical = numerical;
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.nameInt, "name", (string)null, false);
			Scribe_Values.Look<bool>(ref this.numerical, "numerical", false, false);
		}

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
				result = ((byte)((nameTriple != null && nameTriple.Nick == this.nameInt) ? 1 : 0) != 0);
			}
			return result;
		}

		public override string ToString()
		{
			return this.nameInt;
		}

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

		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.nameInt.GetHashCode(), 1384661390);
		}
	}
}
