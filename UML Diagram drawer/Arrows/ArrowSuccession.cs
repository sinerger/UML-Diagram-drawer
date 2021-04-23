﻿using System;
using System.Drawing;

namespace UML_Diagram_drawer.Arrows
{
    class ArrowSuccession : AbstactArrow
    {
        public ArrowSuccession()
        {

        }
        public ArrowSuccession(Pen pen) : base(pen)
        {
        }

        public ArrowSuccession(Pen pen, Point startPoint, Point endPoint) : base(pen, startPoint, endPoint)
        {
        }

        public override void Draw()
        {
            if (!StartPoint.Location.IsEmpty && !EndPoint.Location.IsEmpty)
            {
                DrawStraightBrokenLine();
                DrawArrowhead();
            }
        }

        private void DrawArrowhead()
        {
            Point[] arrowHeadPoints = new Point[3];

            if (!StartPoint.Location.IsEmpty && !EndPoint.Location.IsEmpty)
            {
                if(Points[Points.Length - 2].Y == EndPoint.Location.Y)
                {
                    if (Points[Points.Length - 2].X < EndPoint.Location.X)
                    {
                        arrowHeadPoints[0] = EndPoint.Location;
                        arrowHeadPoints[1] = new Point(EndPoint.Location.X - _sizeArrowhead, EndPoint.Location.Y + _sizeArrowhead);
                        arrowHeadPoints[2] = new Point(EndPoint.Location.X - _sizeArrowhead, EndPoint.Location.Y - _sizeArrowhead);
                    }
                    else
                    {
                        arrowHeadPoints[0] = EndPoint.Location;
                        arrowHeadPoints[1] = new Point(EndPoint.Location.X + _sizeArrowhead, EndPoint.Location.Y + _sizeArrowhead);
                        arrowHeadPoints[2] = new Point(EndPoint.Location.X + _sizeArrowhead, EndPoint.Location.Y - _sizeArrowhead);
                    }
                }
                else
                {
                    if (Points[Points.Length - 2].Y < EndPoint.Location.Y)
                    {
                        arrowHeadPoints[0] = EndPoint.Location;
                        arrowHeadPoints[1] = new Point(EndPoint.Location.X + _sizeArrowhead, EndPoint.Location.Y - _sizeArrowhead);
                        arrowHeadPoints[2] = new Point(EndPoint.Location.X - _sizeArrowhead, EndPoint.Location.Y - _sizeArrowhead);
                    }
                    else
                    {
                        arrowHeadPoints[0] = EndPoint.Location;
                        arrowHeadPoints[1] = new Point(EndPoint.Location.X + _sizeArrowhead, EndPoint.Location.Y + _sizeArrowhead);
                        arrowHeadPoints[2] = new Point(EndPoint.Location.X - _sizeArrowhead, EndPoint.Location.Y + _sizeArrowhead);
                    }
                }

                MainGraphics.Graphics.DrawPolygon(Pen, arrowHeadPoints);
            }
        }
    }
}
