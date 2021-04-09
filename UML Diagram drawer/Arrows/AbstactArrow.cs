﻿using System;
using System.Drawing;
using UML_Diagram_drawer.Forms;

namespace UML_Diagram_drawer.Arrows
{
    public abstract class AbstactArrow : ISelectable
    {
        protected int _sizeArrowhead;

        public bool IsHorizontal { get; set; }
        public Graphics Graphics { get; set; }
        public Pen Pen { get; set; }
        public ContactPoint StartPoint { get; set; }
        public ContactPoint EndPoint { get; set; }
        public bool IsSelected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsMove { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Point StartMovePoint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Rectangle[] Rectangles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Point[] Points { get; set; }

        public AbstactArrow(Pen pen, Graphics graphics, Point startPoint, Point endPoint)
        {
            Pen = pen;
            _sizeArrowhead = (int)Pen.Width * 3;
            Graphics = graphics;

            StartPoint = new ContactPoint(startPoint);
            EndPoint = new ContactPoint(endPoint);
        }
        public AbstactArrow(Pen pen, Graphics graphics)
        {
            Pen = pen;
            Graphics = graphics;
            _sizeArrowhead = (int)Pen.Width * 3;

            StartPoint = new ContactPoint(Point.Empty);
            EndPoint = new ContactPoint(Point.Empty);
        }

        public abstract void Draw();

        public void CreateSelectionBorders()
        {
            throw new NotImplementedException();
        }

        protected Point[] GetPoints()
        {
            Points = new Point[4];
            Points[0] = (StartPoint.Location);

            if (StartPoint.Side == Side.Left || StartPoint.Side == Side.Right)
            {
                int middle = (StartPoint.Location.X + EndPoint.Location.X) / 2;
                Points[1] = new Point(middle, StartPoint.Location.Y);
                Points[2] = new Point(middle, EndPoint.Location.Y);
            }
            else
            {
                int middle = (StartPoint.Location.Y + EndPoint.Location.Y) / 2;
                Points[1] = new Point(StartPoint.Location.X, middle);
                Points[2] = new Point(EndPoint.Location.X, middle);
            }

            Points[3] = EndPoint.Location;

            return Points;
        }

        public void DrawStraightBrokenLine()
        {
            Graphics.DrawLines(Pen, GetPoints());
        }


        public void Move(int deltaX, int deltaY)
        {
            throw new NotImplementedException();
        }

        public void Select(Point point)
        {
            throw new NotImplementedException();
        }

        public void RemoveSelect()
        {
            throw new NotImplementedException();
        }
    }
}
