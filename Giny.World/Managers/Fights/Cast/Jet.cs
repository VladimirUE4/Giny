using Giny.Core.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Cast
{
    public class Jet
    {
        public double Min
        {
            get;
            set;
        }
        public double Max
        {
            get;
            set;
        }

        public Jet(double min, double max)
        {
            this.Min = min;
            this.Max = max;
        }

        public void ValidateBounds()
        {
            if (Min < 0)
            {
                Min = 0;
            }
            if (Max < 0)
            {
                Max = 0;
            }

            if (Min > Max)
            {
                throw new Exception("Cannot compute damages. Min > Max.");
            }
        }
        public short Generate()
        {
            if (Min == Max)
            {
                return (short)Max;
            }
            else
            {
                return (short)new AsyncRandom().Next((short)Min, (short)Max + 1);
            }
        }
    }
}
