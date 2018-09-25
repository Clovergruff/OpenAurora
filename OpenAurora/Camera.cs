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
		public Vector3 rotSpeed;
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

					if (Var.player != null)
					{
						target = Vector3.Lerp(target, Var.player.position + new Vector3(0, 1.5f, 0), 5 * Time.deltaTime);
					}

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
			if (!Game.window.CursorVisible && !Console.enabled)
			{
				rotSpeed.Y += Input.mouseDelta.X * 300 * Time.deltaTime;
				rotSpeed.X -= Input.mouseDelta.Y * 300 * Time.deltaTime;
			}

			pitch += rotSpeed.X * Time.deltaTime;
			yaw += rotSpeed.Y * Time.deltaTime;

			rotSpeed.X -= rotSpeed.X * 30 * Time.deltaTime;
			rotSpeed.Y -= rotSpeed.Y * 30 * Time.deltaTime;

			pitch = MathHelper.Clamp(pitch, -89, 89);
		}

		public Matrix4 GetViewMatrix()
		{
			Vector3 lookAt = rotation * new Vector3(0, 0, 1);
			return Matrix4.LookAt(position, position + lookAt, up);
		}
	}
}
