using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Input;

namespace OpenAurora
{
	public class Player : Pawn
	{
		public override void Awake()
		{
			base.Awake();

			Var.player = this;

			SetModel(Primitives.cube, "Shade");
			SetTransform(Vector3.Zero, Quaternion.Identity, new Vector3(2, 2, 2));
		}

		public override void Update()
		{
			base.Update();

			Controls();
		}

		void Controls()
		{
			if (Game.mode != Game.Mode.Game)
				return;

			if (Input.GetKey(Key.A))
				velocity.X -= 500 * Time.deltaTime;
			if (Input.GetKey(Key.D))
				velocity.X += 500 * Time.deltaTime;

			if (Input.GetKey(Key.S))
				velocity.Z += 500 * Time.deltaTime;
			if (Input.GetKey(Key.W))
				velocity.Z -= 500 * Time.deltaTime;
		}

		public override void Render()
		{
			Draw.Mesh(mesh, position + new Vector3(0, 1, 0), rotation, scale, texture);
		}
	}
}
