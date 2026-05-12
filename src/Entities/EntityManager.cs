using System;
using System.Collections.Generic;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public class EntityManager
{
    private readonly List<Entity> _entities = new();

    public void Add(Entity entity)
    {
        _entities.Add(entity);
    }

    public void Destroy(Entity entity)
    {
        entity.isActive = false;
    }

    public void RemoveAtPositionSameType<T>(Vector2D<float> pos) where T : Entity
    {
        foreach (var e in _entities)
        {
            if (!e.isActive || e.collider == null)
                continue;

            // filtra por tipo
            if (e is not T)
                continue;

            float left   = e.X;
            float right  = e.X + e.collider.width;
            float top    = e.Y;
            float bottom = e.Y + e.collider.height;

            if (pos.X >= left && pos.X <= right &&
                pos.Y >= top  && pos.Y <= bottom)
            {
                e.isActive = false;
                return;
            }
        }
    }

    public void RemoveAtPosition(Vector2D<float> pos)
    {
        foreach (var e in _entities)
        {
            if (!e.isActive || e.collider == null)
                continue;

            float left = e.X;
            float right = e.X + e.collider.width;
            float top = e.Y;
            float bottom = e.Y + e.collider.height;

            if (pos.X >= left && pos.X <= right &&
                pos.Y >= top && pos.Y <= bottom)
            {
                e.isActive = false;
                return;
            }
        }
    }

    public void RemoveInactivesEntities()
    {
        _entities.RemoveAll(e => !e.isActive);
    }

    public void Update(float dt, InputManager input)
    {
        // Update all entities
        foreach (var e in _entities)
        {
            e.Update(dt, input);
        }

        //Handle all collisions
        foreach (var e in _entities)
        {
            if (!e.isActive) continue;
            if (!e.hasPhysics) continue;

            if (!e.isStatic)
                MoveAndCollide(e, dt);
        }

        RemoveInactivesEntities();
    }


    private void MoveAndCollide(Entity e,float dt)
    {
        MoveAxis(e, dt, true);  // X
        MoveAxis(e, dt, false); // Y
    }

    private void MoveAxis(Entity entity, float dt, bool isX)
    {
        float move = isX
            ? entity.velocity.X * dt
            : entity.velocity.Y * dt;

        if (isX)
            entity.X += move;
        else
            entity.Y += move;

        foreach (var other in _entities)
        {
            if (entity == other) continue;
            if (!other.isActive) continue;
            if (!other.hasPhysics) continue; 

            if (other.collider == null || entity.collider == null){
                continue;
            }

            if (Collider.Intersects(entity, other))
            {
                entity.OnCollide(other);
                other.OnCollide(entity);

                if(other.collider.type == ColliderType.Solid)
                {
                    ResolveCollision(entity, other, isX, move);
                }
            }
        }
    }

    private void ResolveCollision(Entity entity, Entity other, bool isX, float move)
    {
        var eCol = entity.collider;
        var oCol = other.collider;

        if (eCol == null || oCol == null)
            return;


        if (isX) //Horizontal movement
        {
            if (move > 0)
            {
                // a mover para a direita → encosta à esquerda do outro
                entity.X = oCol.Left(other) - eCol.width - eCol.offsetX;
            }
            else
            {
                // a mover para a esquerda → encosta à direita do outro
                entity.X = oCol.Right(other) - eCol.offsetX;
            }

            entity.SetVelocityX(0);
        }
        else //Vertical movement
        {
            if (move > 0)
            {
                // a cair para baixo → encosta em cima
                entity.Y = oCol.Top(other) - eCol.height - eCol.offsetY;
            }
            else
            {
                // a subir → encosta por baixo
                entity.Y = oCol.Bottom(other) - eCol.offsetY;
            }

            entity.SetVelocityY(0);
        }
    }

    public void Render(IntPtr renderer, Sdl sdl)
    {
        foreach (var e in _entities)
        {
            e.Render(renderer, sdl);

            if(e.collider != null){
                e.collider.Render(renderer,sdl,e);
            }
        }
    }
}