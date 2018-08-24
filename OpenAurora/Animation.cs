using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAurora
{
	public class AnimationClip
	{
		public class Keyframe
		{
			public string name;
			public int frame;

			public Vector3 position;
			public Quaternion rotation;
			public Vector3 scale;
		}

		public int start, end;
		public float defaultSpeed = 1;
		public Keyframe[] keyframes;
	}

	public class Animation
	{

	}
}
