using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;

namespace Invaders.Infrastructure
{
    public enum eBulletOf
    {
        SpaceShip,
        Enemy
    }

    public interface ICollisionsManager
    {
        void AddObjectToMonitor(ICollidable i_Collidable);
    }

    public interface ICollidable
    {
        event EventHandler<EventArgs> PositionChanged;

        event EventHandler<EventArgs> SizeChanged;

        event EventHandler<EventArgs> VisibleChanged;

        event EventHandler<EventArgs> Disposed;

        bool Visible { get; }

        bool CheckCollision(ICollidable i_Source);

        bool CheckPixelCollision(ICollidable i_Source);
 
        void Collided(ICollidable i_Collidable);
    }

    public interface ICollidable2D : ICollidable
    {
        Rectangle Bounds { get; }

        Texture2D Texture { get; }

        Vector2 Velocity { get; }
    }

    public interface ICollidable3D : ICollidable
    {
        BoundingBox Bounds { get; }

        Texture2D Texture { get; }

        Vector3 Velocity { get; }
    }
}