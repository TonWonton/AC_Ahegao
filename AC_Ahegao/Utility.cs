#nullable enable


namespace AC_Ahegao
{
	public static class Random
	{
		private static System.Random _random = new System.Random();

		public static float Range(float min, float max)
		{
			return min + _random.NextSingle() * (max - min);
		}
	}

	public static class Mathf
	{
		public static float Max(float a, float b)
		{
			if (a < b) return b;
			else return a;
		}
		public static float Clamp(float value, float min, float max)
		{
			if (value < min) return min;
			else if (value > max) return max;
			else return value;
		}

		public static float Clamp01(float value)
		{
			if (value < 0f) return 0f;
			else if (value > 1f) return 1f;
			else return value;
		}

		public static float Clamp_11(float value)
		{
			if (value < -1f) return -1f;
			else if (value > 1f) return 1f;
			else return value;
		}

		public static float ClampRadius(float value, float radius)
		{
			float minusRadius = -radius;
			if (value < minusRadius) return minusRadius;
			else if (value > radius) return radius;
			else return value;
		}

		public static float ClampOffsetRadius(float value, float otherValue, float radius)
		{
			float delta = value - otherValue;
			float minusRadius = -radius;
			if (delta < minusRadius) return otherValue + minusRadius;
			else if (delta > radius) return otherValue + radius;
			else return value;
		}
	}
}