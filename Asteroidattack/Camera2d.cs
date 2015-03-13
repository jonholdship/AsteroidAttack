using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroidattack
{
    public class Camera2d
    {
        
       private float zoommax = 1.5f;
       private float zoommin = .5f;

       private float _zoom;
       private Matrix _transform;
       private Vector2 _pos;
       private float _rotation;
       private int _viewportWidth;
       private int _viewportHeight;
       private int _worldWidth;
       private int _worldHeight;
       private Vector2 movement = Vector2.Zero;
       KeyboardState keyboardState;

       public Camera2d(Viewport viewport, int worldWidth, 
          int worldHeight, float initialZoom)
       {
          _zoom = initialZoom;
          _rotation = 0.0f;
          _pos = Vector2.Zero;
          _viewportWidth = viewport.Width;
          _viewportHeight = viewport.Height;
          _worldWidth = worldWidth;
          _worldHeight = worldHeight;
        zoommin=_worldWidth/_viewportWidth;
        //if (_worldHeight / _viewportHeight > zoommin)
          //  zoommin = _worldHeight / _viewportHeight;
       }
        
       public void update(KeyboardState keyboardstate)
       {
           keyboardState = keyboardstate;
           movement = Vector2.Zero;
           
           if (keyboardState.IsKeyDown(Keys.LeftControl))
           {
               if (keyboardState.IsKeyDown(Keys.Left))
                   _rotation -=0.1f;
               if (keyboardState.IsKeyDown(Keys.Right))
                   _rotation +=0.1f;
               if (keyboardState.IsKeyDown(Keys.Up))
                   _zoom += 0.01f;
               if (keyboardState.IsKeyDown(Keys.Down))
                   _zoom -= 0.01f;
               zoomcheck();
               rotcheck();
           }
           else
           {
               if (keyboardState.IsKeyDown(Keys.Left))
                   movement.X--;
               if (keyboardState.IsKeyDown(Keys.Right))
                   movement.X++;
               if (keyboardState.IsKeyDown(Keys.Up))
                   movement.Y--;
               if (keyboardState.IsKeyDown(Keys.Down))
                   movement.Y++;
               _pos += movement;
              poscheck();
           }
       }
       
       public void poscheck()
       {
           float leftBarrier = ((float)_viewportWidth - _worldWidth) * _zoom;
           float rightBarrier = ((_worldWidth) - (float)_viewportWidth)*_zoom;
           float topBarrier = ((float)_viewportWidth- _worldWidth)*_zoom;
           float bottomBarrier = (_worldWidth-(float)_viewportWidth) * _zoom;
               if (_pos.X < leftBarrier)
                   _pos.X = leftBarrier;
               if (_pos.X > rightBarrier)
                   _pos.X = rightBarrier;
               if (_pos.Y < topBarrier)
                   _pos.Y = topBarrier;
               if (_pos.Y > bottomBarrier)
                   _pos.Y = bottomBarrier;
       }

       public void zoomcheck()
       {
           if (_zoom > zoommax)
               _zoom = zoommax;
           if (_zoom < zoommin)
               _zoom = zoommin;
       }

       public void rotcheck()
       {
           if (_rotation > 6.282)
               _rotation = 0.0f;
       }

       public Matrix GetTransformation()
       {
         _transform = 
            Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
            Matrix.CreateRotationZ(_rotation) *
            Matrix.CreateScale(new Vector3(_zoom, _zoom, 1)) *
            Matrix.CreateTranslation(new Vector3(_viewportWidth * 0.5f,
                _viewportHeight * 0.5f, 0));

           return _transform;
       }
    }
}
