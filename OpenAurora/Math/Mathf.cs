using System;
using OpenTK;
using System.Threading;

namespace OpenAurora
{
	public partial struct MathfInternal
	{
		public static volatile float FloatMinNormal = 1.17549435E-38f;
		public static volatile float FloatMinDenormal = Single.Epsilon;

		public static bool IsFlushToZeroEnabled = (FloatMinDenormal == 0);
	}

	public partial struct Mathf
	{
		public static float Sin(float f) { return (float)Math.Sin(f); }
		public static float Cos(float f) { return (float)Math.Cos(f); }
		public static float Tan(float f) { return (float)Math.Tan(f); }
		public static float Asin(float f) { return (float)Math.Asin(f); }
		public static float Acos(float f) { return (float)Math.Acos(f); }
		public static float Atan(float f) { return (float)Math.Atan(f); }
		public static float Atan2(float y, float x) { return (float)Math.Atan2(y, x); }
		public static float Sqrt(float f) { return (float)Math.Sqrt(f); }
		public static float Abs(float f) { return (float)Math.Abs(f); }
		public static int Abs(int value) { return Math.Abs(value); }

		public static float Min(float a, float b) { return a < b ? a : b; }
		public static float Min(params float[] values)
		{
			int len = values.Length;
			if (len == 0)
				return 0;
			float m = values[0];
			for (int i = 1; i < len; i++)
			{
				if (values[i] < m)
					m = values[i];
			}
			return m;
		}

		public static int Min(int a, int b) { return a < b ? a : b; }
		public static int Min(params int[] values)
		{
			int len = values.Length;
			if (len == 0)
				return 0;
			int m = values[0];
			for (int i = 1; i < len; i++)
			{
				if (values[i] < m)
					m = values[i];
			}
			return m;
		}

		public static float Max(float a, float b) { return a > b ? a : b; }
		public static float Max(params float[] values)
		{
			int len = values.Length;
			if (len == 0)
				return 0;
			float m = values[0];
			for (int i = 1; i < len; i++)
			{
				if (values[i] > m)
					m = values[i];
			}
			return m;
		}

		public static int Max(int a, int b) { return a > b ? a : b; }
		public static int Max(params int[] values)
		{
			int len = values.Length;
			if (len == 0)
				return 0;
			int m = values[0];
			for (int i = 1; i < len; i++)
			{
				if (values[i] > m)
					m = values[i];
			}
			return m;
		}

		public static float Pow(float f, float p) { return (float)Math.Pow(f, p); }
		public static float Exp(float power) { return (float)Math.Exp(power); }
		public static float Log(float f, float p) { return (float)Math.Log(f, p); }
		public static float Log(float f) { return (float)Math.Log(f); }
		public static float Log10(float f) { return (float)Math.Log10(f); }
		public static float Ceil(float f) { return (float)Math.Ceiling(f); }
		public static float Floor(float f) { return (float)Math.Floor(f); }
		public static float Round(float f) { return (float)Math.Round(f); }
		public static int CeilToInt(float f) { return (int)Math.Ceiling(f); }
		public static int FloorToInt(float f) { return (int)Math.Floor(f); }
		public static int RoundToInt(float f) { return (int)Math.Round(f); }
		public static float Sign(float f) { return f >= 0F ? 1F : -1F; }
		public const float PI = (float)Math.PI;
		public const float Infinity = Single.PositiveInfinity;
		public const float NegativeInfinity = Single.NegativeInfinity;
		public const float Deg2Rad = PI * 2F / 360F;
		public const float Rad2Deg = 1F / Deg2Rad;

		// A tiny floating point value (RO).
		public static readonly float Epsilon =
			MathfInternal.IsFlushToZeroEnabled ? MathfInternal.FloatMinNormal
			: MathfInternal.FloatMinDenormal;

		// Clamps a value between a minimum float and maximum float value.
		public static float Clamp(float value, float min, float max)
		{
			if (value < min)
				value = min;
			else if (value > max)
				value = max;
			return value;
		}

		// Clamps value between min and max and returns value.
		// Set the position of the transform to be that of the time
		// but never less than 1 or more than 3
		//
		public static int Clamp(int value, int min, int max)
		{
			if (value < min)
				value = min;
			else if (value > max)
				value = max;
			return value;
		}

		// Clamps value between 0 and 1 and returns value
		public static float Clamp01(float value)
		{
			if (value < 0F)
				return 0F;
			else if (value > 1F)
				return 1F;
			else
				return value;
		}

		// Interpolates between /a/ and /b/ by /t/. /t/ is clamped between 0 and 1.
		public static float Lerp(float a, float b, float t)
		{
			return a + (b - a) * Clamp01(t);
		}

		// Interpolates between /a/ and /b/ by /t/ without clamping the interpolant.
		public static float LerpUnclamped(float a, float b, float t)
		{
			return a + (b - a) * t;
		}

		// Same as ::ref::Lerp but makes sure the values interpolate correctly when they wrap around 360 degrees.
		public static float LerpAngle(float a, float b, float t, bool useRadians = true)
		{
			float loop = 360;
			if (useRadians)
				loop = Deg2Rad * loop;

			float delta = Repeat((b - a), loop);
			if (delta > loop * 0.5f)
				delta -= loop;

			return a + delta * Clamp01(t);
		}

		// Moves a value /current/ towards /target/.
		static public float MoveTowards(float current, float target, float maxDelta)
		{
			if (Mathf.Abs(target - current) <= maxDelta)
				return target;
			return current + Mathf.Sign(target - current) * maxDelta;
		}

		// Same as ::ref::MoveTowards but makes sure the values interpolate correctly when they wrap around 360 degrees.
		static public float MoveTowardsAngle(float current, float target, float maxDelta)
		{
			float deltaAngle = DeltaAngle(current, target);
			if (-maxDelta < deltaAngle && deltaAngle < maxDelta)
				return target;
			target = current + deltaAngle;
			return MoveTowards(current, target, maxDelta);
		}

		// Interpolates between /min/ and /max/ with smoothing at the limits.
		public static float SmoothStep(float from, float to, float t)
		{
			t = Mathf.Clamp01(t);
			t = -2.0F * t * t * t + 3.0F * t * t;
			return to * t + from * (1F - t);
		}

		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
		{
			float deltaTime = Time.deltaTime;
			return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime)
		{
			float deltaTime = Time.deltaTime;
			float maxSpeed = Mathf.Infinity;
			return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
		{
			// Based on Game Programming Gems 4 Chapter 1.10
			smoothTime = Mathf.Max(0.0001F, smoothTime);
			float omega = 2F / smoothTime;

			float x = omega * deltaTime;
			float exp = 1F / (1F + x + 0.48F * x * x + 0.235F * x * x * x);
			float change = current - target;
			float originalTo = target;

			// Clamp maximum speed
			float maxChange = maxSpeed * smoothTime;
			change = Mathf.Clamp(change, -maxChange, maxChange);
			target = current - change;

			float temp = (currentVelocity + omega * change) * deltaTime;
			currentVelocity = (currentVelocity - omega * temp) * exp;
			float output = target + (change + temp) * exp;

			// Prevent overshooting
			if (originalTo - current > 0.0F == output > originalTo)
			{
				output = originalTo;
				currentVelocity = (output - originalTo) / deltaTime;
			}

			return output;
		}

		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
		{
			float deltaTime = Time.deltaTime;
			return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime)
		{
			float deltaTime = Time.deltaTime;
			float maxSpeed = Mathf.Infinity;
			return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
		{
			target = current + DeltaAngle(current, target);
			return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static float Repeat(float t, float length)
		{
			return Clamp(t - Mathf.Floor(t / length) * length, 0.0f, length);
		}

		public static float PingPong(float t, float length)
		{
			t = Repeat(t, length * 2F);
			return length - Mathf.Abs(t - length);
		}

		public static float InverseLerp(float a, float b, float value)
		{
			if (a != b)
				return Clamp01((value - a) / (b - a));
			else
				return 0.0f;
		}

		public static float DeltaAngle(float current, float target)
		{
			float delta = Mathf.Repeat((target - current), 360.0F);
			if (delta > 180.0F)
				delta -= 360.0F;
			return delta;
		}

		public static float Vector2AngleInRad(Vector2 vec1, Vector2 vec2) { return Vector2AngleInRad(new Vector2(vec2.Y - vec1.Y, vec2.X - vec1.X)); }
		public static float Vector2AngleInRad(Vector2 vec)
		{
			return Mathf.Atan2(vec.Y, vec.X);
		}

		public static float Vector2AngleInDeg(Vector2 vec1, Vector2 vec2) { return Vector2AngleInDeg(vec2 - vec1); }
		public static float Vector2AngleInDeg(Vector2 vec)
		{
			return Vector2AngleInRad(vec) * 180 / Mathf.PI;
		}
	}
}