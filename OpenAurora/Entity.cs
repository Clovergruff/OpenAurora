using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace OpenAurora
{
	public class Entity
	{
		public string name;
		public float timeScale = 1;

		// Transform
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 scale;

		// Graphics
		public Mesh mesh;
		public Texture2D texture;

		public Entity()
		{
			Game.entities.Add(this);

			Awake();
		}

		public virtual void Awake()
		{
			
		}

		public virtual void Update()
		{

		}

		public void SetTransform(Vector3 pos, Vector3 eulers, Vector3 sc)
		{
			position = pos;
			rotation = Quaternion.FromEulerAngles(MathHelper.DegreesToRadians(eulers.X),
													MathHelper.DegreesToRadians(eulers.Y),
														MathHelper.DegreesToRadians(eulers.Z));
			scale = sc;
		}

		public void SetTransform(Vector3 pos, Quaternion rot, Vector3 sc)
		{
			position = pos;
			rotation = rot;
			scale = sc;
		}

		public void SetModel(Mesh sourceMesh, string texName = null)
		{
			SetModel(sourceMesh, Resources.GetTexture(texName));
		}

		public void SetModel(Mesh sourceMesh, Texture2D tex = null)
		{
			mesh = new Mesh(sourceMesh.name, sourceMesh.vertices, sourceMesh.indices);
			if (tex != null)
				texture = tex;
		}

		public virtual void Render()
		{
			if (mesh == null)
				return;

			Draw.Mesh(mesh, position, rotation, scale, texture);
		}

		public static object Create()
		{
			var ent = new Entity();
			return ent;
		}
	}
}
