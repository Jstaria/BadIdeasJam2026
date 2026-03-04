using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IdleEngine
{
    public class Spring2D
    {
        private Spring xPos;
        private Spring yPos;

        public Vector2 Position
        {
            get => new Vector2(xPos.Position, yPos.Position);
            set
            {
                xPos.Position = value.x;
                yPos.Position = value.y;
            }
        }

        public Vector3 XZPosition
        {
            get => new Vector3(xPos.Position, 0, yPos.Position);
        }

        public Vector2 RestPosition
        {
            get => new Vector2(xPos.RestPosition, yPos.RestPosition);
            set
            {
                xPos.RestPosition = value.x;
                yPos.RestPosition = value.y;
            }
        }


        public Spring2D(float angularFrequency, float dampingRatio, Vector2 restPosition)
        {
            xPos = new Spring(angularFrequency, dampingRatio, restPosition.x, true);
            yPos = new Spring(angularFrequency, dampingRatio, restPosition.y, true);
        }

        public void Update()
        {
            xPos.Update();
            yPos.Update();
        }
        public void SetValues(float angularFrequency, float dampingRatio)
        {
            xPos.SetValues(angularFrequency, dampingRatio);
            yPos.SetValues(angularFrequency, dampingRatio);
        }

        public void Nudge(Vector2 velocity)
        {
            xPos.Nudge(velocity.x);
            yPos.Nudge(velocity.y);
        }
    }
}
