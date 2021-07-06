using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace HEROsMod.UIKit
{
	internal class UIImage : UIView
	{
		private Asset<Texture2D> texture;

		public Asset<Texture2D> Texture
		{
			get { return texture; }
			set { this.texture = value; }
		}

		private float width
		{ get { return texture.Value.Width; } }
		private float height
		{ get { return texture.Value.Height; } }
		private SpriteEffects _spriteEfftct = SpriteEffects.None;

		public SpriteEffects SpriteEffect
		{
			get { return _spriteEfftct; }
			set { _spriteEfftct = value; }
		}

		private Rectangle? sourceRectangle = null;

		public Rectangle SourceRectangle
		{
			get
			{
				if (sourceRectangle == null) sourceRectangle = new Rectangle();
				return (Rectangle)sourceRectangle;
			}
			set { sourceRectangle = value; }
		}

		public int SR_X
		{
			get { return SourceRectangle.X; }
			set { SourceRectangle = new Rectangle(value, SourceRectangle.Y, SourceRectangle.Width, SourceRectangle.Height); }
		}

		public int SR_Y
		{
			get { return SourceRectangle.X; }
			set { SourceRectangle = new Rectangle(SourceRectangle.X, value, SourceRectangle.Width, SourceRectangle.Height); }
		}

		public int SR_Width
		{
			get { return SourceRectangle.X; }
			set { SourceRectangle = new Rectangle(SourceRectangle.X, SourceRectangle.Y, value, SourceRectangle.Height); }
		}

		public int SR_Height
		{
			get { return SourceRectangle.X; }
			set { SourceRectangle = new Rectangle(SourceRectangle.X, SourceRectangle.Y, SourceRectangle.Width, value); }
		}

		public UIImage(Asset<Texture2D> texture)
		{
			this.Texture = texture;
		}

		public UIImage()
		{
		}

		protected override float GetWidth()
		{
			if (sourceRectangle != null) return ((Rectangle)sourceRectangle).Width * Scale;
			return width * Scale;
		}

		protected override float GetHeight()
		{
			if (sourceRectangle != null) return ((Rectangle)sourceRectangle).Height * Scale;
			return height * Scale;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (Visible)
				spriteBatch.Draw(texture.Value, DrawPosition, sourceRectangle, ForegroundColor * Opacity, 0f, Origin / Scale, Scale, SpriteEffect, 0f);

			base.Draw(spriteBatch);
		}
	}
}