using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Asteroidattack
{
    class asteroid : body
    {
        static Texture2D asttext;
        Rectangle world;
        bool onscreen = false;
        Camera2d Cam;

        //constructor
        public asteroid(Rectangle World,planet[] planets,Camera2d cam) : base(0.0001,10000.0,10000.0,0.0,0.0,asttext)
        {
            Cam=cam;
            this.world = World;
            appear(planets);
        }

        public static void LoadContent(ContentManager content)
        {
            asttext=content.Load<Texture2D>("asteroid");
        }

        public void update(planet[] planets)
        {
            //check if asteroid is on screen
            if (world.Contains(size.X, size.Y))
            {
                onscreen = true;
            }
            else onscreen = false;

            //if it is, update pretty much like a planet
            if (onscreen == true)
            {
                //move according to gravity and current velocity
                gravity(planets);
                mv = mass * v;
                pos.X=(float)(position[0]*100.0);
                pos.Y=(float)(position[1]*100.0);
                size.X = (int)pos.X;
                size.Y = (int)pos.Y;


                //check for collisions
                for (int i = 0; i < 7; i++)
                {
                    Rectangle other;
                    other = planets[i].getsize();
                    if (size.Intersects(other))
                    {
                        collision(planets[i]);
                    }
                }
            }
            //if not on screen, do aiming and re enter screen
            else
            {
                appear(planets);
            }
        }

        private void collision(planet other)
        {
            double othermass = other.getmass();
            other.smallhit(velocity[0],velocity[1],mass);
            position[0] = 1000000.0;
            position[1] = 1000000.0;
        }

        private void appear(planet[] planets)
        {
            Random rnd = new Random();
            double[] otherpos = new double[2];
            int side = rnd.Next(0, 2);
            if (side == 0)
            {
                size.X = rnd.Next(0, world.Width);
                size.Y = (rnd.Next(0, 2)) * world.Height;
            }
            else
            {
                size.X = (rnd.Next(0, 2)) * world.Width;
                size.Y = rnd.Next(0, world.Height);
            }

            //similar to abandon for a planet, set's asteroid position and velocity upon arrival on screen
            position[0] = (double)size.X;
            position[1] = (double)size.Y;
            position[0] = position[0] / 100;
            position[1] = position[1] / 100;

            velocity[0]=0.0;
            do
            {
                int i = rnd.Next(1, 7);
                if (planets[i].inhabit == true) otherpos = planets[i].getposition();
                velocity[0] = 1.0;
            } while (velocity[0] == 0.0);

            velocity[0]=(otherpos[0]-position[0])/250.0;
            velocity[1]=(otherpos[1]-position[1])/250.0;

        }
    }
}

