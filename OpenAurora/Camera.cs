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
		public override void Awake()
		{
			base.Awake();

			Var.camera = this;
		}
	}
}
