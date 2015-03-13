using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroidattack
{   
    //the purely physical class that planet and asteroid build from.
    //no player control, or drawing will be included
    class body
    {
        #region variables
        //physical variables, physical constants, position/velocity vectors
        protected double mass, dx, dy, mv, r, theta, v;
        protected const double g = 2.824e-7, pi = 3.141592654, msun = 1047.2, tstep = 0.001;
        protected double[] position = new double[2], velocity = new double[2];

        //game variables
        protected bool alive = true;
        protected Rectangle size = new Rectangle(0, 0, 15, 15);
        protected Vector2 pos;
        protected Texture2D texture;
        #endregion

        #region public functions
        //constructor for the body
        public body(double M,double X,double Y,double U,double V,Texture2D text)
        {
            //constructor
            mass=M;
            position[0]=X;
            position[1]=Y;
            velocity[0]=U;
            velocity[1]=V;
            this.texture = text;
            positions();
           
        }

        public body(double M, double X, double Y, double U, double V)
        {
            //constructor
            mass = M;
            position[0] = X;
            position[1] = Y;
            velocity[0] = U;
            velocity[1] = V;
            positions();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           if (alive ==true) spriteBatch.Draw(texture, size, Color.White);
        }

#endregion

        #region get functions
        public double[] getposition()
        {
            return position;
        }

        public double getmass()
        {
            return mass;
        }

        public double getmomentum()
        {
            mv = mass * v;
            return mv;
        }

        public Rectangle getsize()
        {
            return size;
        }

        #endregion 
        
        #region private/protected functions
        //gravity
        protected void gravity(planet[] planets)
        {
            //gravity solver! it's a simple euler method, called 10 times from main

            position[0] += (velocity[0] * tstep);
            position[1] += (velocity[1] * tstep);

            //work out theta from new x,y co-ords
            theta = Math.Atan(position[1] / position[0]);
            if (position[1] > 0 && position[0] < 0)
                theta = theta + (pi);
            if (position[1] < 0 && position[0] < 0)
                theta = theta + (pi);
            if (position[1] < 0 && position[0] > 0)
                theta = theta + (2 * pi);

            //update u and v due to grav forces from sun
            //all units are in au, days, jupiter masses
            r = Math.Sqrt((position[0] * position[0]) + (position[1] * position[1]));
            velocity[0] = velocity[0] - (((msun * g) / (r * r)) * tstep * (Math.Cos(theta)));
            velocity[1] = velocity[1] - (((msun * g) / (r * r)) * tstep * (Math.Sin(theta)));

            for (int i = 0; i < 8; i++)
            {
                //same again for planet-planet interaction
                dx = (position[0] - planets[i].position[0]);
                dy = (position[1] - planets[i].position[1]);
                theta = Math.Atan(dy / dx);
                if (dy > 0 && dx < 0)
                    theta = theta + (pi);
                if (dy < 0 && dx < 0)
                    theta = theta + (pi);
                if (dy < 0 && dx > 0)
                    theta = theta + (2 * pi);
                r = Math.Sqrt((position[0] - planets[i].position[0]) * (position[0] - planets[i].position[0]) + (position[1] - planets[i].position[1]) * (position[1] - planets[i].position[1]));
                if (r > 0)
                {
                    velocity[0] = velocity[0] - (((planets[i].mass * g) / (r * r)) * tstep * (Math.Cos(theta)));
                    velocity[1] = velocity[1] - (((planets[i].mass * g) / (r * r)) * tstep * (Math.Sin(theta)));
                    v = (velocity[0] * velocity[0]) + (velocity[1] * velocity[1]);
                    v = Math.Sqrt(v);
                }
            }

            positions();

        }

        protected void collision(planet other)
        {
            double m2=other.mass;
            if (mass < m2)
            {
                other.smallhit(velocity[0],velocity[1],mass);
                velocity[0] = (velocity[0] * (mass - m2)) + (2 * m2 * other.velocity[0]);
                velocity[0] /= (mass + m2);
                velocity[1] = (velocity[1] * (mass - m2)) + (2 * m2 * other.velocity[0]);
                velocity[1] /= (mass + m2);
            }
        }

        //calculates r,v,mv,pos and size. Use whenever position[] gets changed
        protected void positions()
        {
            r = Math.Sqrt((position[0] * position[0]) + (position[1] * position[1]));
            v = Math.Sqrt((velocity[0] * velocity[0]) + (velocity[1] * velocity[1]));
            mv = mass * v;
            pos.X = (float)(position[0]);
            pos.Y = (float)(position[1]);
            pos.X = (100 * pos.X);
            pos.Y = (100 * pos.Y);
            size.X = (int)pos.X;
            size.Y = (int)pos.Y;
        }
        #endregion

    }
}
