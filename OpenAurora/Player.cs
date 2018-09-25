using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK;
using System.Drawing;

namespace OpenAurora
{
	public class Player : Pawn
	{
		private Vector2 inputVec;

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
			if (Console.enabled || Game.mode != Game.Mode.Game)
				return;

			inputVec = Vector2.Zero;

			if (Input.GetKey(Key.A))
				inputVec.X = 1;
			if (Input.GetKey(Key.D))
				inputVec.X = -1;

			if (Input.GetKey(Key.S))
				inputVec.Y = -1;
			if (Input.GetKey(Key.W))
				inputVec.Y = 1;

			if (inputVec.Length > 0.1f)
				inputVec.Normalize();


			velocity += Quaternion.FromEulerAngles(0, MathHelper.DegreesToRadians(Var.camera.yaw), 0) * new Vector3(inputVec.X, 0, inputVec.Y) * 400 * Time.deltaTime;
		}

		public override void Render()
		{
			Draw.Mesh(mesh, position + new Vector3(0, 1, 0), rotation, scale, texture);
		}
	}
}
