using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

public sealed class LineGraph : Control
{
    private static readonly Font HighlightFont = new Font("Verdana", 8);

    private readonly List<DataPoint> DataPoints;
    private Point MouseLocation;
    private long PointIndex = 1;

    private int LineDensity = 2;
    private int PointDensity = 5;
    private bool ShowAverage = true;
    private bool HighlightPoint = true;
    private Color MainColor = Color.FromArgb(18, 121, 192);

    public int GridLineDensity
    {
        get { return LineDensity; }
        set
        {
            LineDensity = value;
            Invalidate();
        }
    }

    public int DataPointDensity
    {
        get { return PointDensity; }
        set
        {
            PointDensity = value;
            RemoveHiddenPoints();
            Invalidate();
        }
    }

    public bool ShowAverageBar
    {
        get { return ShowAverage; }
        set
        {
            ShowAverage = value;
            Invalidate();
        }
    }

    public bool HighlightDataPoint
    {
        get { return HighlightPoint; }
        set
        {
            HighlightPoint = value;
            Invalidate();
        }
    }

    public Color GraphColor
    {
        get { return MainColor; }
        set
        {
            MainColor = value;
            Invalidate();
        }
    }

    public LineGraph()
    {
        SetStyle(ControlStyles.ResizeRedraw, true);
        DoubleBuffered = true;

        DataPoints = new List<DataPoint>
        {
            new DataPoint(0, 0)
        };
        MouseLocation = new Point(-1, -1);
    }

    public void AddDataPoint(float dataPoint)
    {
        DataPoints.Add(new DataPoint(dataPoint, PointIndex));
        RemoveHiddenPoints();
        Invalidate();

        PointIndex++;
    }

    public void AddDataPoints(IEnumerable<float> dataPoints)
    {
        foreach (var item in dataPoints)
        {
            DataPoints.Add(new DataPoint(item, PointIndex));
            PointIndex++;
        }

        RemoveHiddenPoints();
        Invalidate();
    }

    public void ClearDataPoints()
    {
        DataPoints.Clear();
        DataPoints.Add(new DataPoint(0, 0));
        Invalidate();

        PointIndex = 1;
    }

    public float GetAverage()
    {
        return DataPoints.Select(p => p.Value).Average();
    }

    public float GetMax()
    {
        float max = DataPoints.Select(p => p.Value).Max();
        return (max == 0) ? 1 : max;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (HighlightPoint)
        {
            MouseLocation = e.Location;
            Invalidate();
        }
        base.OnMouseMove(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        if (HighlightPoint)
        {
            MouseLocation = new Point(-1, -1);
            Invalidate();
        }
        base.OnMouseLeave(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;

        g.Clear(Color.White);

        using (var dataPointPath = new GraphicsPath())
        using (var gridPen = new Pen(Color.FromArgb(20, Color.Black)))
        using (var dashPen = new Pen(MainColor) { DashStyle = DashStyle.Dash })
        using (var borderPen = new Pen(MainColor))
        using (var fillBrush = new SolidBrush(Color.FromArgb(35, MainColor)))
        using (var textBrush = new SolidBrush(MainColor))
        {
            if (DataPoints.Count > 1)
            {
                var infoRect = new Rectangle();
                var highlightRect = new RectangleF();
                var infoString = string.Empty;

                dataPointPath.AddLine(Width + 10, Height + 10, Width + 10, Height + 10);

                var offset = 0;

                for (var i = DataPoints.Count - 1; i >= 1; i--)
                {
                    if (DataPoints[i].Index % LineDensity == 0)
                        g.DrawLine(gridPen, Width - offset, 0, Width - offset, Height);

                    var scaledY = GetScaledPoint(DataPoints[i].Value);

                    dataPointPath.AddLine(Width - offset, scaledY, Width - offset - PointDensity,
                        GetScaledPoint(DataPoints[i - 1].Value));

                    if (HighlightPoint && new Rectangle(Width - offset - (PointDensity / 2), 0, PointDensity, Height).Contains(MouseLocation))
                    {
                        g.DrawLine(gridPen, 0, scaledY, Width, scaledY);

                        highlightRect = new RectangleF(Width - offset - 3, scaledY - 3, 6, 6);

                        infoString = DataPoints[i].Value.ToString(CultureInfo.InvariantCulture);
                        var infoStringSize = TextRenderer.MeasureText(infoString, Font);

                        infoRect = new Rectangle
                        {
                            Height = infoStringSize.Height + 2,
                            Width = infoStringSize.Width + 5
                        };

                        if (offset < infoStringSize.Width + 10)
                            infoRect.X = Width - offset - infoStringSize.Width - 10;
                        else
                            infoRect.X = Width - offset + 5;

                        if (scaledY > Height - infoStringSize.Height - 5)
                            infoRect.Y = (int)scaledY - infoStringSize.Height - 5;
                        else
                            infoRect.Y = (int)scaledY + 5;
                    }

                    offset += PointDensity;
                }

                dataPointPath.AddLine(-10, Height + 10, -10, Height + 10);

                dataPointPath.CloseFigure();

                g.SmoothingMode = SmoothingMode.AntiAlias;

                g.FillPath(fillBrush, dataPointPath);
                g.DrawPath(borderPen, dataPointPath);

                g.SmoothingMode = SmoothingMode.None;

                if (ShowAverage)
                {
                    var average = GetScaledPoint(GetAverage());
                    g.DrawLine(dashPen, 0, average, Width, average);
                    g.FillRectangle(fillBrush, 0, average, Width, Height - average);
                }

                if (HighlightPoint)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    g.FillEllipse(Brushes.White, highlightRect);
                    g.DrawEllipse(borderPen, highlightRect);

                    g.SmoothingMode = SmoothingMode.None;

                    g.FillRectangle(Brushes.White, infoRect);
                    g.DrawRectangle(borderPen, infoRect);
                    g.DrawString(infoString, HighlightFont, textBrush, infoRect.X + 4, infoRect.Y + 1);
                }
            }

            g.DrawRectangle(borderPen, new Rectangle(0, 0, Width - 1, Height - 1));
        }
    }

    private float GetScaledPoint(float value)
    {
        return Height - value * Height / GetMax();
    }

    private void RemoveHiddenPoints()
    {
        if (DataPoints.Count * PointDensity > Width + PointDensity * 2)
            DataPoints.RemoveRange(0, DataPoints.Count - (Width + PointDensity * 2) / PointDensity);
    }

    private class DataPoint
    {
        public float Value { get; set; }
        public long Index { get; set; }

        public DataPoint(float val, long index)
        {
            Value = val;
            Index = index;
        }
    }
}

