// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using TerraFX.Numerics;

namespace TerraFX.Graphics.Geometry2D;

/// <summary>Defines a rectangle.</summary>
public readonly struct Rectangle : IEquatable<Rectangle>, IFormattable
{
    private readonly Vector2 _location;
    private readonly Vector2 _size;

    /// <summary>Initializes a new instance of the <see cref="Rectangle" /> struct.</summary>
    /// <param name="location">The location of the instance.</param>
    /// <param name="size">The size of the instance.</param>
    public Rectangle(Vector2 location, Vector2 size)
    {
        _location = location;
        _size = size;
    }

    /// <summary>Initializes a new instance of the <see cref="Rectangle" /> struct.</summary>
    /// <param name="x">The x-coordinate of the instance.</param>
    /// <param name="y">The y-coordinate of the instance.</param>
    /// <param name="width">The width of the instance.</param>
    /// <param name="height">The height of the instance.</param>
    public Rectangle(float x, float y, float width, float height)
    {
        _location = new Vector2(x, y);
        _size = new Vector2(width, height);
    }

    /// <summary>Gets the height of the instance.</summary>
    public float Height => _size.Y;

    /// <summary>Gets the location of the instance.</summary>
    public Vector2 Location => _location;

    /// <summary>Gets the size of the instance.</summary>
    public Vector2 Size => _size;

    /// <summary>Gets the width of the instance.</summary>
    public float Width => _size.X;

    /// <summary>Gets the value of the x-coordinate.</summary>
    public float X => _location.X;

    /// <summary>Gets the value of the y-coordinate.</summary>
    public float Y => _location.Y;

    /// <summary>Compares two <see cref="Rectangle" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="Rectangle" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="Rectangle" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Rectangle left, Rectangle right)
    {
        return (left.Location == right.Location)
            && (left.Size == right.Size);
    }

    /// <summary>Compares two <see cref="Rectangle" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="Rectangle" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="Rectangle" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Rectangle left, Rectangle right)
    {
        return (left.Location != right.Location)
            || (left.Size != right.Size);
    }

    /// <summary>Creates a new <see cref="Rectangle" /> instance with <see cref="Height" /> set to the specified value.</summary>
    /// <param name="value">The new height of the instance.</param>
    /// <returns>A new <see cref="Rectangle" /> instance with <see cref="Height" /> set to <paramref name="value" />.</returns>
    public Rectangle WithHeight(float value)
    {
        var size = new Vector2(Width, value);
        return new Rectangle(Location, size);
    }

    /// <summary>Creates a new <see cref="Rectangle" /> instance with <see cref="Location" /> set to the specified value.</summary>
    /// <param name="value">The new location of the instance.</param>
    /// <returns>A new <see cref="Rectangle" /> instance with <see cref="Location" /> set to <paramref name="value" />.</returns>
    public Rectangle WithLocation(Vector2 value) => new Rectangle(value, Size);

    /// <summary>Creates a new <see cref="Rectangle" /> instance with <see cref="Size" /> set to the specified value.</summary>
    /// <param name="value">The new size of the instance.</param>
    /// <returns>A new <see cref="Rectangle" /> instance with <see cref="Size" /> set to <paramref name="value" />.</returns>
    public Rectangle WithSize(Vector2 value) => new Rectangle(Location, value);

    /// <summary>Creates a new <see cref="Rectangle" /> instance with <see cref="Width" /> set to the specified value.</summary>
    /// <param name="value">The new width of the instance.</param>
    /// <returns>A new <see cref="Rectangle" /> instance with <see cref="Width" /> set to <paramref name="value" />.</returns>
    public Rectangle WithWidth(float value)
    {
        var size = new Vector2(value, Height);
        return new Rectangle(Location, size);
    }

    /// <summary>Creates a new <see cref="Rectangle" /> instance with <see cref="X" /> set to the specified value.</summary>
    /// <param name="value">The new value of the x-coordinate.</param>
    /// <returns>A new <see cref="Rectangle" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
    public Rectangle WithX(float value)
    {
        var location = new Vector2(value, Y);
        return new Rectangle(location, Size);
    }

    /// <summary>Creates a new <see cref="Rectangle" /> instance with <see cref="Y" /> set to the specified value.</summary>
    /// <param name="value">The new value of the y-coordinate.</param>
    /// <returns>A new <see cref="Rectangle" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
    public Rectangle WithY(float value)
    {
        var location = new Vector2(X, value);
        return new Rectangle(location, Size);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is Rectangle other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(Rectangle other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        {
            hashCode.Add(X);
            hashCode.Add(Y);
            hashCode.Add(Width);
            hashCode.Add(Height);
        }
        return hashCode.ToHashCode();
    }

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        return new StringBuilder(5 + separator.Length)
            .Append('<')
            .Append(X.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append(Y.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append(Width.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append(Height.ToString(format, formatProvider))
            .Append('>')
            .ToString();
    }
}
