using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAurora
{
	public class Pawn : Entity
	{
		public float direction, directionTarget, turnSpeed = 10;
		public Vector3 velocity;

		public override void Update()
		{
			base.Update();

			direction = Mathf.LerpAngle(direction, directionTarget, turnSpeed * Time.deltaTime);
			rotation = Quaternion.FromEulerAngles(0, direction, 0);

			velocity -= velocity * 20 * Time.deltaTime;
			position += velocity * Time.deltaTime;
		}
	}
}
