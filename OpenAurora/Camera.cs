using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAurora
{
	public class Camera : Entity
	{
		public enum Mode
		{
			Gameplay,
			Cutscene,
		}

		public Mode mode = Mode.Gameplay;
		public Vector3 up = Vector3.UnitY;
		public Vector3 target = Vector3.Zero;
		public float pitch, yaw, roll;
		public float distance = 15;

		public float fov = 80;

		public override void Awake()
		{
			base.Awake();

			Var.camera = this;
		}

		public override void Update()
		{
			base.Update();


			switch (mode)
			{
				case Mode.Gameplay:
					GameControls();

					rotation = Quaternion.FromEulerAngles(0, MathHelper.DegreesToRadians(yaw), 0) * Quaternion.FromEulerAngles(MathHelper.DegreesToRadians(pitch), 0, 0);
					position = target + rotation * new Vector3(0, 0, -distance);
					break;
				case Mode.Cutscene:
					break;
				default:
					break;
			}
		}

		void GameControls()
		{
			yaw += Input.mouseDelta.X;
			pitch -= Input.mouseDelta.Y;

			pitch = MathHelper.Clamp(pitch, -89, 89);
		}

		public Matrix4 GetViewMatrix()
		{
			Vector3 lookAt = rotation * new Vector3(0, 0, 1);
			return Matrix4.LookAt(position, position + lookAt, up);
		}
	}
}
