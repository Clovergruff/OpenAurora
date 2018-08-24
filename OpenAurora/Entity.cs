using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public virtual void Render()
		{

		}

		public static object Create()
		{
			var ent = new Entity();
			return ent;
		}
	}
}
