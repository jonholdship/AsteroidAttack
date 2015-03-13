using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroidattack
{
    class planet : body
    {
        double  pop = 100000.0;
        string stringpop;
        int selected = 0;
        public bool inhabit = false;
        MouseState mouseState;

        //constructor just forwards values to base
        public planet(double M, double X, double Y, double U, double V, Texture2D texture) :base(M,X,Y,U,V,texture)
        {            
        }

        public void update(planet[] planets, Camera2d Cam)
        {
            //main function to update each time step, first decides if mouse is controlling it

            mouseState = Mouse.GetState();
            Matrix inverse = Matrix.Invert(Cam.GetTransformation());
            Vector2 mousePos = Vector2.Transform(
            new Vector2(mouseState.X, mouseState.Y), inverse);

            for (int i = 0; i < 7; i++)
            {
                if (size.Intersects(planets[i].getsize()))
                {
                    collision(planets[i]);
                    if (inhabit == true) pop *= 0.9;
                }
            }

            //set selected to 1 if mouse is controlling
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (size.Contains((int)mousePos.X, (int)mousePos.Y))
                {
                    selected = 1;
                }
            }

            //follow mouse if selected
            if (selected == 1)
            {
                size.X = (int)mousePos.X;
                size.Y = (int)mousePos.Y;
            }
            //progress according to physics otherwise, gravity() does this
            else
            {
                gravity(planets);
            }

            //if player lets go off mouse, calculate a circular orbit for the planet in it's current position
            if (mouseState.LeftButton == ButtonState.Released && selected == 1)
            {
                selected = 0;
                abandon();
            }

            if (inhabit == true) growpop();

        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont Scorefont)
        {
            //here we simply redraw the planet in new location
            base.Draw(spriteBatch);
            if (inhabit == true) spriteBatch.DrawString(Scorefont, stringpop, new Vector2(-250, -150), Color.White);

        }

        private void abandon()
        {
            //function that creates a new orbit for the planet by finding velocity required for circular orbit around sun based on pixel positions
            position[0] = (double)size.X;
            position[1] = (double)size.Y;
            position[0] /= 100;
            position[1] /= 100;
            
            //theta 
            theta = Math.Atan(position[1] / position[0]);
            if (position[1] > 0 && position[0] < 0)
                theta = theta + (pi);
            if (position[1] < 0 && position[0] < 0)
                theta = theta + (pi);
            if (position[1] < 0 && position[0] > 0)
                theta = theta + (2 * pi);

            //now turn v and r into x,y,u,v
            //v perpendicular to r so y ends up next to theta and hence cos for y component
            v = Math.Sqrt(g * msun / r);
            velocity[0] = v * Math.Sin(theta);
            velocity[1] = v * Math.Cos(theta);
            positions();
        }

        public void smallhit(double u2, double v2,double m2)
        {
            velocity[0] = (velocity[0] * (mass - m2)) + (2 * m2 * u2);
            velocity[0] /= (mass + m2);
            velocity[1] = (velocity[1] * (mass - m2)) + (2 * m2 * v2);
            velocity[1] /= (mass + m2);        

        }

        private void growpop()
        {
            pop += 10.0;
            stringpop = pop.ToString();
        }
    }
}
