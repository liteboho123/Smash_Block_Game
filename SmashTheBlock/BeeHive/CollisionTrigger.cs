using System;
using System.Linq;
using System.Windows;

namespace BeeHive
{
	public class CollisionTrigger : GameTrigger<FrameworkElement>
	{
		protected override void OnGameEnvironmentInitialized()
		{
			this.GameEnvironment.Collision += this.OnGameEnvironmentCollision;
		}

		private void OnGameEnvironmentCollision(object sender, GameEnvironment.CollisionEventArgs e)
		{
			if (e.CollisionInformation.Any(collisionInfo => object.ReferenceEquals(this.Source, collisionInfo.CollidingElement) || object.ReferenceEquals(this.Source, collisionInfo.HitElement)))
			{
				this.InvokeActions(EventArgs.Empty);
			}
		}
	}
}
