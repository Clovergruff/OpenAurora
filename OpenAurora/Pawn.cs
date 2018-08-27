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
		public float yaw;
		public Vector3 velocity;

		public override void Update()
		{
			base.Update();

			velocity -= velocity * 20 * Time.deltaTime;
			position += velocity * Time.deltaTime;
		}
	}
}
