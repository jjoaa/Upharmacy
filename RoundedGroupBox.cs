using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundedGroupBox : GroupBox
{
    public int CornerRadius { get; set; } = 10;

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        // 배경을 라운드 바깥쪽 영역에 맞게 Color.FromArgb(180, 219, 200) 색으로 채우기
        using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(180, 219, 200)))
        {
            e.Graphics.FillRectangle(bgBrush, this.ClientRectangle);
        }

        // 라운드 내부 영역을 흰색으로 채우기
        Rectangle bounds = this.ClientRectangle;
        bounds.Width -= 1;
        bounds.Height -= 1;

        GraphicsPath path = GetRoundedRectangle(bounds, CornerRadius);
        using (SolidBrush whiteBrush = new SolidBrush(Color.White))
        {
            e.Graphics.FillPath(whiteBrush, path);
        }

        // 테두리 라운드 그리기
        using (Pen borderPen = new Pen(Color.Gray, 1))
        {
            e.Graphics.DrawPath(borderPen, path);
        }

        // 텍스트 배경
        SizeF textSize = e.Graphics.MeasureString(this.Text, this.Font);
        Rectangle textRect = new Rectangle(10, 0, (int)textSize.Width + 6, (int)textSize.Height);
        e.Graphics.FillRectangle(new SolidBrush(this.BackColor), textRect);
        e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), new PointF(10, 0));
    }

    private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
    {
        int diameter = radius * 2;
        GraphicsPath path = new GraphicsPath();
        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }
}
