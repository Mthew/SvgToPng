Imports System.Drawing.Graphics
Imports System.Drawing
Imports System.IOPartial
Imports System.Drawing.Imaging

Public Class BarCode
    Private _encoding As Hashtable = New Hashtable
    Private Const _wideBarWidth As Short = 2
    Private Const _narrowBarWidth As Short = 1
    Private Const _barHeight As Short = 31

    'Here we define the page counters -- this comes into play later!
    Public totalPages As Integer = 1
    Public curPage As Integer = 0

    'Create a new box scan form
    'Public boxScanFrm As boxScan = New boxScan

    'Declare the return timer countdown (seconds before return to the box scanner)
    Public returnTimer As Integer = 15

    Public doCancel As Boolean = False
    Public printFromClick As Boolean = False

    'Define the main barcode canvas used for the master page
    Dim barcodeCanvas As Bitmap

    Public Sub New()
        ITS_BarcodeC39()
    End Sub
#Region "Barcode Generation System"
    'This adds keys to the encoding hashtable, which tells us exactly what we need to draw later in GDI
    'But more or less, these are the encoding bits for each of the Code39 allowable characters
    Sub ITS_BarcodeC39()
        _encoding.Add("*", "bWbwBwBwb")
        _encoding.Add("-", "bWbwbwBwB")
        _encoding.Add("$", "bWbWbWbwb")
        _encoding.Add("%", "bwbWbWbWb")
        _encoding.Add(" ", "bWBwbwBwb")
        _encoding.Add(".", "BWbwbwBwb")
        _encoding.Add("/", "bWbWbwbWb")
        _encoding.Add("+", "bWbwbWbWb")
        _encoding.Add("0", "bwbWBwBwb")
        _encoding.Add("1", "BwbWbwbwB")
        _encoding.Add("2", "bwBWbwbwB")
        _encoding.Add("3", "BwBWbwbwb")
        _encoding.Add("4", "bwbWBwbwB")
        _encoding.Add("5", "BwbWBwbwb")
        _encoding.Add("6", "bwBWBwbwb")
        _encoding.Add("7", "bwbWbwBwB")
        _encoding.Add("8", "BwbWbwBwb")
        _encoding.Add("9", "bwBWbwBwb")
        _encoding.Add("A", "BwbwbWbwB")
        _encoding.Add("B", "bwBwbWbwB")
        _encoding.Add("C", "BwBwbWbwb")
        _encoding.Add("D", "bwbwBWbwB")
        _encoding.Add("E", "BwbwBWbwb")
        _encoding.Add("F", "bwBwBWbwb")
        _encoding.Add("G", "bwbwbWBwB")
        _encoding.Add("H", "BwbwbWBwb")
        _encoding.Add("I", "bwBwbWBwb")
        _encoding.Add("J", "bwbwBWBwb")
        _encoding.Add("K", "BwbwbwbWB")
        _encoding.Add("L", "bwBwbwbWB")
        _encoding.Add("M", "BwBwbwbWb")
        _encoding.Add("N", "bwbwBwbWB")
        _encoding.Add("O", "BwbwBwbWb")
        _encoding.Add("P", "bwBwBwbWb")
        _encoding.Add("Q", "bwbwbwBWB")
        _encoding.Add("R", "BwbwbwBWb")
        _encoding.Add("S", "bwBwbwBWb")
        _encoding.Add("T", "bwbwBwBWb")
        _encoding.Add("U", "BWbwbwbwB")
        _encoding.Add("V", "bWBwbwbwB")
        _encoding.Add("W", "BWBwbwbwb")
        _encoding.Add("X", "bWbwBwbwB")
        _encoding.Add("Y", "BWbwBwbwb")
        _encoding.Add("Z", "bWBwBwbwb")
    End Sub

    'This returns a color associated with the current line drawing in GDI, since it's a barcode we primarly want a contrasting
    'color and a light background color, so depending on what the character is (whether it's a char or white space) we add color
    Public Function getBCSymbolColor(ByVal symbol As String) As System.Drawing.Brush
        getBCSymbolColor = Brushes.Black
        If symbol = "W" Or symbol = "w" Then

            getBCSymbolColor = Brushes.White
        End If
    End Function

    'Now we determine whether or not we are going to be drawing a small or large BC bar on this character code
    Public Function getBCSymbolWidth(ByVal symbol As String) As Short
        getBCSymbolWidth = _narrowBarWidth
        If symbol = "B" Or symbol = "W" Then
            getBCSymbolWidth = _wideBarWidth
        End If
    End Function

    'Now the fun part, this function is called to generate the actual barcode
    Public Overridable Function GenerateBarcodeImage(ByVal imageWidth As Short, ByVal imageHeight As Short, ByVal Code As String) As IO.MemoryStream
        'Declare a new bitmap canvas to store our new barcode (well, it will be new -- we will make it soon)!
        Dim b As New Bitmap(imageWidth, imageHeight, Imaging.PixelFormat.Format32bppArgb)

        'Create the actualy canvas associated with the barcode drawing       
        Dim canvas As New Rectangle(0, 0, imageWidth, imageHeight)

        'Create our graphics object from our barcode canvas
        Dim g As Graphics = Graphics.FromImage(b)

        'Fill the drawing with a white background
        g.FillRectangle(Brushes.White, 0, 0, imageWidth, imageHeight)

        'Draw the "eye candy" items on the barcode canvas
        'Here we are drawing the gradient directly behind the barcode are at the top
        g.FillRectangle(New Drawing2D.LinearGradientBrush(New Drawing.RectangleF(1, 1, 169, 30), Color.White, Color.LightGray, Drawing2D.LinearGradientMode.Vertical), 1, 1, 169, 30)

        ''Now we draw the seperation line (under barcode) and the liner gradient background
        g.FillRectangle(New Drawing2D.LinearGradientBrush(New Drawing.RectangleF(1, 34, 169, 15), Color.LightGray, Color.White, Drawing2D.LinearGradientMode.Vertical), 1, 34, 169, 15)
        g.FillRectangle(New SolidBrush(Color.Black), 1, 33, 169, 1)

        'Now that we have the "fine art" drawn, let's switch over to high-quality rendering for our text and images
        'However, we are using SingleBitPerPixel because when printed it appears sharper as opposed to anti-aliased
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality

        'If we are printing a box label then we need to add a side image and a "BOX" announcment on the right.
        'If boxNumber.Enabled = False Then
        '    g.DrawImage(My.Resources.Shipping, 10, 2, 23, 23)
        '    g.DrawString("BOX", New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.Silver), 129, 7)

        '    'Write out the original barcode ID, and preceed it with "BOX"
        '    g.DrawString("Box " & Code, New Font("Tahoma", 8, FontStyle.Bold), New SolidBrush(Color.Black), 3, 34)
        'Else
        '    'Write out the original barcode ID
        '    g.DrawString(Code, New Font("Tahoma", 8, FontStyle.Bold), New SolidBrush(Color.Black), 3, 34)
        'End If

        'Write out a lighter barcode generation script version on the right (again, eye candy)
        g.DrawString("ITS v2.0", New Font("Tahoma", 8, FontStyle.Bold), New SolidBrush(Color.Gray), 113, 34)

        'Now that we are done with the high quality rendering, we are going to draw the barcode lines -- which needs to be very straight, and not blended
        'Else it may blur and cause complications with the barcode reading device -- so we won't take any chances.
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.SystemDefault
        g.SmoothingMode = Drawing2D.SmoothingMode.None
        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        g.CompositingQuality = Drawing2D.CompositingQuality.Default

        'Code has to be surrounded by asterisks to make it a valid C39 barcode, so add "*" to the front and read of the barcode
        Dim UseCode As String = String.Format("{0}{1}{0}", "*", Code)

        'Define a starting X and Y position for the barcode
        Dim XPosition As Short = 1
        Dim YPosition As Short = 1

        'Initialize our IC marker, and give a default of false
        'This is used to track what?  Incorrectly assigned characters for the barcode (ones that won't match
        'C39 standards) So we don't use them, and mark it as invalid.
        Dim invalidCharacter As Boolean = False

        'Declare our current character string holding variable
        Dim CurrentSymbol As String = String.Empty

        ''THIS PART IS *ONLY* FOR CALCULATING THE WIDTH OF THE BARCODE TO CENTER IT!
        ''Begin at the starting area of our FINAL rendered barcode value
        'For j As Short = 0 To CShort(UseCode.Length - 1)
        '    'Set our current character to the character space of the barcode
        '    CurrentSymbol = UseCode.Substring(j, 1)

        '    'Check to ensure that it's a valid character per our encoding hashtable we defined earlier
        '    If Not IsNothing(_encoding(CurrentSymbol)) Then
        '        'Create a new rendered version of the character per our hashtable with valid values (don't read it, it will be encoded -- look above at the HT)
        '        Dim EncodedSymbol As String = _encoding(CurrentSymbol).ToString

        '        'Progress throughout the entire encoding value of this character
        '        For i As Short = 0 To CShort(EncodedSymbol.Length - 1)
        '            'Extract the current encoded character value from the complete rendering of this character (it's getting deep, right?)
        '            Dim CurrentCode As String = EncodedSymbol.Substring(i, 1)

        '            'Change our coordinates for drawing to match the next position (current position plus the char. bar width)
        '            XPosition = XPosition + getBCSymbolWidth(CurrentCode)
        '        Next

        '        'Now we need to "create" a whitespace as needed, and get the width
        '        XPosition = XPosition + getBCSymbolWidth("w")
        '    End If
        'Next

        ''Now the nice trick of division helps with centering the barcode on the canvas
        'XPosition = (imageWidth / 2) - (XPosition / 2)

        'NOW WE GO LIVE!  THIS IS WHERE WE ACTUALLY DRAW THE BARCODE BARS
        'Begin at the starting area of our FINAL rendered barcode value
        For j As Short = 0 To CShort(UseCode.Length - 1)
            'Set our current character to the character space of the barcode
            CurrentSymbol = UseCode.Substring(j, 1)

            'Check to ensure that it's a valid character per our encoding hashtable we defined earlier                  
            If Not IsNothing(_encoding(CurrentSymbol)) Then
                'Create a new rendered version of the character per our hashtable with valid values (don't read it, it will be encoded -- look above at the HT)
                Dim EncodedSymbol As String = _encoding(CurrentSymbol).ToString

                'Progress throughout the entire encoding value of this character
                For i As Short = 0 To CShort(EncodedSymbol.Length - 1)
                    'Extract the current encoded character value from the complete rendering of this character (it's getting deep, right?)
                    Dim CurrentCode As String = EncodedSymbol.Substring(i, 1)

                    'Use our drawing graphics object on the canvase to create a bar with out position and values based on the current character encoding value
                    g.FillRectangle(getBCSymbolColor(CurrentCode), XPosition, YPosition, getBCSymbolWidth(CurrentCode), _barHeight)
                    'Lets disect this a little to see how it actually works, want to?
                    '   getBCSymbolColor(CurrentCode)
                    '       We already know, but this gets the color of the bar, either white or colorized (in this case, black)
                    '   XPosition, YPosition
                    '       Again, we already know -- but this is the coordinates to draw the bar based on previous locations
                    '   getBCSymbolWidth(CurrentCode)
                    '       This is the important part, we get the correct width (either narrow or wide) for this character (post encoding)
                    '   _barHeight
                    '       This is static as defined earlier, it doesn't much matter but it also depends on your Barcode reader

                    'Change our coordinates for drawing to match the next position (current position plus the char. bar width)
                    XPosition = XPosition + getBCSymbolWidth(CurrentCode)
                Next

                'Now we need to "ACTUALLY" create a whitespace as needed, and get the width
                g.FillRectangle(getBCSymbolColor("w"), XPosition, YPosition, getBCSymbolWidth("w"), _barHeight)

                'Change our coordinates for drawing to match the next position (current position plus the char. bar width)
                XPosition = XPosition + getBCSymbolWidth("w")
            Else
                'This is our fallback, if it's not a valid character per our hashtable in C39, discard!
                invalidCharacter = True
            End If
        Next

        'As we set it above (if needed) for an invalid character (not allowed by the C39 guide), then we handle it here  
        If invalidCharacter Then
            'Just fill the whole canvas white
            g.FillRectangle(Brushes.White, 0, 0, imageWidth, imageHeight)

            'What's the deal?  Tell them!  It's not right, so we can't do it -- here is why.
            g.DrawString("Invalid Charachers Detected", New Font("Tahoma", 8), New SolidBrush(Color.Red), 0, 0)
            g.DrawString("- Barcode Not Generated -", New Font("Tahoma", 8), New SolidBrush(Color.Black), 0, 10)
            g.DrawString(Code, New Font("Tahoma", 8, FontStyle.Italic), New SolidBrush(Color.Black), 0, 30)
        End If

        'Create a new memorystream to use with our function return
        Dim ms As New IO.MemoryStream

        'Setup the encoding quality of the final barcode rendered image
        Dim encodingParams As New EncoderParameters
        encodingParams.Param(0) = New EncoderParameter(Encoder.Quality, 100)

        'Define the encoding details of "how" for the image
        'We will use PNG because, well it's got the best image quality for it's footprint
        Dim encodingInfo As ImageCodecInfo = FindCodecInfo("PNG")

        'Save the drawing directly into the stream
        b.Save(ms, encodingInfo, encodingParams)

        'Clean-up!  Nobody likes a possible memory leaking application!
        g.Dispose()
        b.Dispose()

        'Finally, return the image via the memorystream
        Return ms
    End Function

    'Find the encoding method in the codec list on the computer based on the known-name (PNG, JPEG, etc)
    Public Overridable Function FindCodecInfo(ByVal codec As String) As ImageCodecInfo
        Dim encoders As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders
        For Each e As ImageCodecInfo In encoders
            If e.FormatDescription.Equals(codec) Then Return e
        Next
        Return Nothing
    End Function
#End Region
End Class
