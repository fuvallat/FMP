Public Class OutlineLabel
    Inherits Label

    Public Property OutlineColor As Color = Color.Black
    Public Property OutlineWidth As Integer = 2

    Public Sub New()
        'Me.SetStyle(ControlStyles.Opaque, False)
        'Me.SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        'Me.BackColor = Color.Transparent
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        Dim path As New Drawing2D.GraphicsPath()
        path.AddString(Me.Text, Me.Font.FontFamily, Me.Font.Style, Me.Font.SizeInPoints * e.Graphics.DpiY / 72, New Point(0, 0), StringFormat.GenericDefault)

        Dim bounds = path.GetBounds()

        Dim x As Single = 0
        Dim y As Single = 0

        Select Case Me.TextAlign
            Case ContentAlignment.TopLeft
                x = 0 : y = 0

            Case ContentAlignment.TopCenter
                x = (Me.Width - bounds.Width) / 2
                y = 0

            Case ContentAlignment.TopRight
                x = Me.Width - bounds.Width
                y = 0

            Case ContentAlignment.MiddleLeft
                x = 0
                y = (Me.Height - bounds.Height) / 2

            Case ContentAlignment.MiddleCenter
                x = (Me.Width - bounds.Width) / 2
                y = (Me.Height - bounds.Height) / 2

            Case ContentAlignment.MiddleRight
                x = Me.Width - bounds.Width
                y = (Me.Height - bounds.Height) / 2

            Case ContentAlignment.BottomLeft
                x = 0
                y = Me.Height - bounds.Height

            Case ContentAlignment.BottomCenter
                x = (Me.Width - bounds.Width) / 2
                y = Me.Height - bounds.Height

            Case ContentAlignment.BottomRight
                x = Me.Width - bounds.Width
                y = Me.Height - bounds.Height
        End Select

        Dim translate = New Drawing2D.Matrix()
        translate.Translate(x - bounds.X, y - bounds.Y)
        path.Transform(translate)

        Using pen As New Pen(OutlineColor, OutlineWidth)
            pen.LineJoin = Drawing2D.LineJoin.Round
            e.Graphics.DrawPath(pen, path)
        End Using

        Using brush As New SolidBrush(Me.ForeColor)
            e.Graphics.FillPath(brush, path)
        End Using
    End Sub
End Class
