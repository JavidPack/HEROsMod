using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;

namespace HEROsMod.UIKit;

internal sealed class UIHyperlink : UIView
{
	private string _text = string.Empty;
	private string _url = string.Empty;

	private float _measuredWidth;
	private float _measuredHeight;

	private bool _textOutline = true;
	private bool _tooltipEnabled = true;

	private DynamicSpriteFont _font;

	private Color _fillColor = Color.White;

	private Color _fillHoverColor = Color.LightGray;
	private Color _outlineColor = Color.Black;
	private Color _outlineHoverColor = Color.Black;

	internal UIHyperlink()
	{
		_font = FontAssets.DeathText.Value;
		Text = string.Empty;
		Url = string.Empty;

		onLeftClick += (_, _) => OpenUrl();
	}

	internal UIHyperlink(string text, string url)
	{
		_font = FontAssets.DeathText.Value;
		Text = text ?? string.Empty;
		Url = url ?? string.Empty;

		onLeftClick += (_, _) => OpenUrl();
	}

	internal string Text
	{
		get { return _text; }
		set
		{
			_text = value ?? string.Empty;
			RecalculateSize();
		}
	}

	internal string Url
	{
		get { return _url; }
		set
		{
			_url = value ?? string.Empty;

			if (_tooltipEnabled)
			{
				Tooltip = _url;
			}
		}
	}

	internal bool TextOutline
	{
		get { return _textOutline; }
		set { _textOutline = value; }
	}

	internal bool TooltipEnabled
	{
		get { return _tooltipEnabled; }
		set
		{
			_tooltipEnabled = value;
			Tooltip = _tooltipEnabled ? _url : string.Empty;
		}
	}

	internal DynamicSpriteFont Font
	{
		get { return _font; }
		set
		{
			_font = value ?? FontAssets.DeathText.Value;
			RecalculateSize();
		}
	}

	internal Color FillColor
	{
		get { return _fillColor; }
		set { _fillColor = value; }
	}

	internal Color FillHoverColor
	{
		get { return _fillHoverColor; }
		set { _fillHoverColor = value; }
	}

	internal Color OutlineColor
	{
		get { return _outlineColor; }
		set { _outlineColor = value; }
	}

	internal Color OutlineHoverColor
	{
		get { return _outlineHoverColor; }
		set { _outlineHoverColor = value; }
	}

	protected override float GetWidth()
	{
		return _measuredWidth * Scale;
	}

	protected override float GetHeight()
	{
		if (_measuredHeight <= 0f)
		{
			return _font.MeasureString("H").Y * Scale;
		}

		return _measuredHeight * Scale;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (!Visible || string.IsNullOrEmpty(_text))
		{
			base.Draw(spriteBatch);
			return;
		}

		Color fill = MouseInside ? _fillHoverColor : _fillColor;
		Color stroke = MouseInside ? _outlineHoverColor : _outlineColor;

		fill *= Opacity;
		stroke *= Opacity;

		if (_textOutline)
		{
			Utils.DrawBorderStringFourWay(
				spriteBatch,
				_font,
				_text,
				DrawPosition.X,
				DrawPosition.Y,
				fill,
				stroke,
				Origin / Scale,
				Scale
			);
		}
		else
		{
			spriteBatch.DrawString(_font, _text, DrawPosition, fill, 0f, Origin / Scale, Scale, SpriteEffects.None, 0f);
		}

		base.Draw(spriteBatch);
	}

	private void RecalculateSize()
	{
		if (string.IsNullOrEmpty(_text))
		{
			_measuredWidth = 0f;
			_measuredHeight = 0f;
			return;
		}

		Vector2 size = _font.MeasureString(_text);
		_measuredWidth = size.X;
		_measuredHeight = size.Y;
	}

	private void OpenUrl()
	{
		if (string.IsNullOrWhiteSpace(_url))
		{
			return;
		}

		try
		{
			Utils.OpenToURL(_url);
		}
		catch
		{
			ModUtils.DebugText($"Failed to open URL: {_url}");
		}
	}
}
