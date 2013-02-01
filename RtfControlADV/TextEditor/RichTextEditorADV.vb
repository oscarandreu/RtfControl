Imports System.Windows.Forms
Imports System.Drawing
Imports System.ComponentModel
Imports System.IO

Public Class RichTextEditorADV


#Region "Declarations"

	Private currentFile As String
	Private checkPrint As Integer

	Private IsExpanded As Boolean = False
	Private _parentLevel As Integer = 0
	Private _parentContainer As Control = Parent
	''' <summary>
	''' Label que sirve de referencia para volver al tamaño y posición original
	''' </summary>
	''' <remarks></remarks>
	Private marcador As New Label

#End Region

#Region " Propiedades "

	<Category("Comportamiento")> _
	  Public Property ParentLevel() As Integer
		Get
			Return _parentLevel
		End Get
		Set(ByVal value As Integer)
			_parentLevel = value
		End Set
	End Property

#End Region

#Region " Constructor "

	Public Sub New()
		' Llamada necesaria para el Diseñador de Windows Forms.
		InitializeComponent()

		marcador.Anchor = Anchor
		marcador.Margin = Margin
		marcador.Dock = Dock
		marcador.SendToBack()
	End Sub

#End Region

#Region "Menu Methods"

	Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If rtbDoc.Modified Then
			Dim answer As Integer
			answer = MessageBox.Show("The current document has not been saved, would you like to continue without saving?", "Unsaved Document", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
			If answer = Windows.Forms.DialogResult.Yes Then
				rtbDoc.Clear()
			Else
				Exit Sub
			End If
		Else
			rtbDoc.Clear()
		End If
		currentFile = String.Empty
		Me.Text = "Editor: New Document"
	End Sub

	Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click, tbrOpen.Click
		OpenFile()
	End Sub

	Private Sub OpenFile()
		OpenFileDialog1.Title = "Seleccione el archivo"
		OpenFileDialog1.DefaultExt = "rtf"
		OpenFileDialog1.Filter = "Archivo RTF|*.rtf|Archivo de texto|*.txt|Archivo HTML|*.htm|Cualquier archivo|*.*"
		'OpenFileDialog1.Filter = "Archivo RTF|*.rtf|Archivo PDF|*.pdf|Archivo de texto|*.txt|Archivo HTML|*.htm|Cualquier archivo|*.*"
		OpenFileDialog1.FilterIndex = 1


		If (OpenFileDialog1.ShowDialog() = DialogResult.Cancel OrElse OpenFileDialog1.FileName = String.Empty) Then
			Exit Sub
		End If

		Dim strExt As String
		strExt = System.IO.Path.GetExtension(OpenFileDialog1.FileName)
		strExt = strExt.ToUpper()
		Select Case strExt
			Case ".RTF"
				rtbDoc.LoadFile(OpenFileDialog1.FileName, RichTextBoxStreamType.RichText)

				'Case ".PDF"
				'	rtbDoc.LoadFile(OpenFileDialog1.FileName, RichTextBoxStreamType.RichText)

			Case Else
				Dim txtReader As System.IO.StreamReader
				txtReader = New System.IO.StreamReader(OpenFileDialog1.FileName)
				rtbDoc.Text = txtReader.ReadToEnd
				txtReader.Close()
				txtReader = Nothing
				rtbDoc.SelectionStart = 0
				rtbDoc.SelectionLength = 0
		End Select
		currentFile = OpenFileDialog1.FileName
		rtbDoc.Modified = False
		Me.Text = "Editor: " & currentFile.ToString()
	End Sub

	Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click, tbrSave.Click
		If currentFile = String.Empty Then
			SaveAsToolStripMenuItem_Click(Me, e)
			Exit Sub
		End If
		Dim strExt As String
		strExt = System.IO.Path.GetExtension(currentFile)
		strExt = strExt.ToUpper()
		Select Case strExt
			Case ".RTF"
				rtbDoc.SaveFile(currentFile)
			Case Else
				' to save as plain text
				Dim txtWriter As System.IO.StreamWriter
				txtWriter = New System.IO.StreamWriter(currentFile)
				txtWriter.Write(rtbDoc.Text)
				txtWriter.Close()
				txtWriter = Nothing
				rtbDoc.SelectionStart = 0
				rtbDoc.SelectionLength = 0
				rtbDoc.Modified = False
		End Select
		Me.Text = "Editor: " & currentFile.ToString()
	End Sub

	Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
		SaveFileDialog1.Title = "Guardar como"
		SaveFileDialog1.DefaultExt = "rtf"
		SaveFileDialog1.Filter = "Archivo RTF|*.rtf|PDF|*.pdf|Archivo de texto|*.txt|Archivo HTML|*.htm|Cualquier archivo|*.*"
		SaveFileDialog1.FilterIndex = 1

		If SaveFileDialog1.ShowDialog() = DialogResult.Cancel OrElse SaveFileDialog1.FileName = String.Empty Then
			Exit Sub
		End If

		Dim fileName As String = SaveFileDialog1.FileName
		Dim mpath As String = Path.GetDirectoryName(fileName)

		Dim strExt As String
		strExt = System.IO.Path.GetExtension(fileName)
		strExt = strExt.ToUpper()
		Select Case strExt
			Case ".RTF"
				rtbDoc.SaveFile(fileName, RichTextBoxStreamType.RichText)

			Case ".PDF"
				Dim temp As Object = mpath + "\\foo.rtf"
				rtbDoc.SaveFile(temp.ToString, RichTextBoxStreamType.RichText)

				Dim newApp As New Microsoft.Office.Interop.Word.Application
				Dim Unknown As Object = Type.Missing

				newApp.Visible = False
				newApp.Documents.Open(temp, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, _
				 Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown)

				Dim format As Object = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF
				newApp.ActiveDocument.SaveAs(fileName.ToString, format, _
				 Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, _
				 Unknown, Unknown, Unknown, Unknown, Unknown)

				newApp.Documents.Close()
				newApp.Quit(Unknown, Unknown, Unknown)

				File.Delete(temp.ToString)

			Case Else
				Dim txtWriter As System.IO.StreamWriter
				txtWriter = New System.IO.StreamWriter(fileName)
				txtWriter.Write(rtbDoc.Text)
				txtWriter.Close()
				txtWriter = Nothing
				rtbDoc.SelectionStart = 0
				rtbDoc.SelectionLength = 0
		End Select
		currentFile = fileName
		rtbDoc.Modified = False
		Me.Text = "Editor: " & currentFile.ToString()
	End Sub

	Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
		Try
			rtbDoc.SelectAll()
		Catch exc As Exception
			MessageBox.Show("Imposible seleccionar todo el contenido.", "Seleccionar todo", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
	End Sub

	Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
		Try
			rtbDoc.Copy()
		Catch exc As Exception
			MessageBox.Show("Imposible copiar el contenido del documento.", "Copiar", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
	End Sub

	Private Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
		Try
			rtbDoc.Cut()
		Catch exc As Exception
			MessageBox.Show("Imposible cortar el contenido del documento.", "Coratar", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
	End Sub

	Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
		Try
			rtbDoc.Paste()
		Catch exc As Exception
			MessageBox.Show("Imposible copiar el contenido del portapapeles en el documento.", "Pegar", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
	End Sub

	Private Sub SelectFontToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectFontToolStripMenuItem.Click, tbrFont.Click
		If Not rtbDoc.SelectionFont Is Nothing Then
			FontDialog1.Font = rtbDoc.SelectionFont
		Else
			FontDialog1.Font = Nothing
		End If
		FontDialog1.ShowApply = True
		If FontDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
			rtbDoc.SelectionFont = FontDialog1.Font
		End If
	End Sub

	Private Sub FontColorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FontColorToolStripMenuItem.Click, tbrForeColor.Click
		ColorDialog1.Color = rtbDoc.ForeColor
		If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
			rtbDoc.SelectionColor = ColorDialog1.Color
		End If
	End Sub

	Private Sub tbrHighlight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbrHighlight.Click
		rtbDoc.SelectionBackColor = Color.Yellow
	End Sub

	Private Sub BoldToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BoldToolStripMenuItem.Click, tbrBold.Click
		If Not rtbDoc.SelectionFont Is Nothing Then
			Dim currentFont As Font = rtbDoc.SelectionFont
			Dim newFontStyle As FontStyle
			If rtbDoc.SelectionFont.Bold = True Then
				newFontStyle = FontStyle.Regular
			Else
				newFontStyle = FontStyle.Bold
			End If
			rtbDoc.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size, newFontStyle)
		End If
	End Sub

	Private Sub ItalicToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItalicToolStripMenuItem.Click, tbrItalic.Click
		If Not rtbDoc.SelectionFont Is Nothing Then
			Dim currentFont As Font = rtbDoc.SelectionFont
			Dim newFontStyle As FontStyle
			If rtbDoc.SelectionFont.Italic = True Then
				newFontStyle = FontStyle.Regular
			Else
				newFontStyle = FontStyle.Italic
			End If
			rtbDoc.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size, newFontStyle)
		End If
	End Sub

	Private Sub UnderlineToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnderlineToolStripMenuItem.Click, tbrUnderline.Click
		If Not rtbDoc.SelectionFont Is Nothing Then
			Dim currentFont As Font = rtbDoc.SelectionFont
			Dim newFontStyle As FontStyle
			If rtbDoc.SelectionFont.Underline = True Then
				newFontStyle = FontStyle.Regular
			Else
				newFontStyle = FontStyle.Underline
			End If
			rtbDoc.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size, newFontStyle)
		End If
	End Sub

	Private Sub StrikeoutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnderlineToolStripMenuItem.Click, tbrStrike.Click
		If Not rtbDoc.SelectionFont Is Nothing Then
			Dim currentFont As Font = rtbDoc.SelectionFont
			Dim newFontStyle As FontStyle
			If rtbDoc.SelectionFont.Strikeout = True Then
				newFontStyle = FontStyle.Regular
			Else
				newFontStyle = FontStyle.Strikeout
			End If
			rtbDoc.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size, newFontStyle)
		End If
	End Sub

	Private Sub NormalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NormalToolStripMenuItem.Click
		If Not rtbDoc.SelectionFont Is Nothing Then
			Dim currentFont As Font = rtbDoc.SelectionFont
			Dim newFontStyle As FontStyle
			newFontStyle = FontStyle.Regular
			rtbDoc.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size, newFontStyle)
		End If
	End Sub

	Private Sub PageColorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageColorToolStripMenuItem.Click, tbrBackgroundColor.Click
		ColorDialog1.Color = rtbDoc.BackColor
		If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
			rtbDoc.SelectionBackColor = ColorDialog1.Color
		End If
	End Sub

	Private Sub mnuUndo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUndo.Click, tbrUndo.Click
		If rtbDoc.CanUndo Then rtbDoc.Undo()
	End Sub

	Private Sub mnuRedo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRedo.Click, tbrRedo.Click
		If rtbDoc.CanRedo Then rtbDoc.Redo()
	End Sub

	Private Sub LeftToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LeftToolStripMenuItem.Click, tbrLeft.Click
		rtbDoc.SelectionAlignment = ComponentsADV.TextAlign.Left
	End Sub

	Private Sub CenterToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CenterToolStripMenuItem.Click, tbrCenter.Click
		rtbDoc.SelectionAlignment = ComponentsADV.TextAlign.Center
	End Sub

	Private Sub RightToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RightToolStripMenuItem.Click, tbrRight.Click
		rtbDoc.SelectionAlignment = ComponentsADV.TextAlign.Right
	End Sub


	Private Sub tbrJustified_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbrJustified.Click
		rtbDoc.SelectionAlignment = ComponentsADV.TextAlign.Justify
	End Sub

	Private Sub AddBulletsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddBulletsToolStripMenuItem.Click
		rtbDoc.BulletIndent = 10
		rtbDoc.SelectionBullet = True
	End Sub

	Private Sub RemoveBulletsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveBulletsToolStripMenuItem.Click
		rtbDoc.SelectionBullet = False
	End Sub

	Private Sub mnuIndent0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuIndent0.Click
		rtbDoc.SelectionIndent = 0
	End Sub

	Private Sub mnuIndent5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuIndent5.Click
		rtbDoc.SelectionIndent = 5
	End Sub

	Private Sub mnuIndent10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuIndent10.Click
		rtbDoc.SelectionIndent = 10
	End Sub

	Private Sub mnuIndent15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuIndent15.Click
		rtbDoc.SelectionIndent = 15
	End Sub

	Private Sub mnuIndent20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuIndent20.Click
		rtbDoc.SelectionIndent = 20
	End Sub

	Private Sub FindToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindToolStripMenuItem.Click, tbrFind.Click
		Dim f As New frmFindAndReplace(rtbDoc, False)
		f.Show()
	End Sub

	Private Sub FindAndReplaceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindAndReplaceToolStripMenuItem.Click
		Dim f As New frmFindAndReplace(rtbDoc, True)
		f.Show()
	End Sub

	Private Sub PreviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PreviewToolStripMenuItem.Click
		PrintPreviewDialog1.Document = PrintDocument1
		PrintPreviewDialog1.ShowDialog()
	End Sub

	Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
		PrintDialog1.Document = PrintDocument1
		If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
			PrintDocument1.Print()
		End If
	End Sub

	Private Sub mnuPageSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPageSetup.Click
		PageSetupDialog1.Document = PrintDocument1
		PageSetupDialog1.ShowDialog()
	End Sub

	Private Sub InsertImageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InsertImageToolStripMenuItem.Click
		OpenFileDialog1.Title = "Insertar imagen"
		OpenFileDialog1.DefaultExt = "rtf"
		OpenFileDialog1.Filter = "Imagen|*.bmp;*.jpg;*.gif;*.png"
		OpenFileDialog1.FilterIndex = 1

		If OpenFileDialog1.ShowDialog() = DialogResult.Cancel OrElse OpenFileDialog1.FileName = String.Empty Then
			Exit Sub
		End If

		Try
			Dim strImagePath As String = OpenFileDialog1.FileName
			Dim img As Image
			img = Image.FromFile(strImagePath)
			Clipboard.SetDataObject(img)
			Dim df As DataFormats.Format
			df = DataFormats.GetFormat(DataFormats.Bitmap)
			If Me.rtbDoc.CanPaste(df) Then
				rtbDoc.Paste(df)
			End If

		Catch ex As Exception
			MessageBox.Show("Imposible insertar el formato de imagen seleccionado.", "Pegar", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
	End Sub

#End Region

#Region "Toolbar Methods"

	Private Sub tbrSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		SaveToolStripMenuItem_Click(Me, e)
	End Sub

	Private Sub tbrOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		OpenToolStripMenuItem_Click(Me, e)
	End Sub

	Private Sub tbrNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		NewToolStripMenuItem_Click(Me, e)
	End Sub

	Private Sub tbrBold_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		BoldToolStripMenuItem_Click(Me, e)
	End Sub

	Private Sub tbrItalic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		ItalicToolStripMenuItem_Click(Me, e)
	End Sub

	Private Sub tbrUnderline_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		UnderlineToolStripMenuItem_Click(Me, e)
	End Sub

	Private Sub trbStrike_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TachadoToolStripMenuItem.Click
		StrikeoutToolStripMenuItem_Click(Me, e)
	End Sub

	Private Sub tbrFont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		SelectFontToolStripMenuItem_Click(Me, e)
	End Sub

	Private Sub tbrLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		rtbDoc.SelectionAlignment = ComponentsADV.TextAlign.Left
	End Sub

	Private Sub tbrCenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		rtbDoc.SelectionAlignment = ComponentsADV.TextAlign.Center
	End Sub

	Private Sub tbrRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		rtbDoc.SelectionAlignment = ComponentsADV.TextAlign.Right
	End Sub

	Private Sub tbrFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Dim f As New frmFindAndReplace(rtbDoc, False)
		f.Show()
	End Sub

#End Region

#Region "Printing"

	Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
		' Adapted from Microsoft's example for extended richtextbox control
		'
		checkPrint = 0
	End Sub

	Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
		' Adapted from Microsoft's example for extended richtextbox control
		'
		' Print the content of the RichTextBox. Store the last character printed.
		checkPrint = rtbDoc.Print(checkPrint, rtbDoc.TextLength, e)
		' Look for more pages
		If checkPrint < rtbDoc.TextLength Then
			e.HasMorePages = True
		Else
			e.HasMorePages = False
		End If
	End Sub

#End Region

#Region " ExpandibleControl "

#Region " Métodos privados "

	Protected Overridable Sub Expand()
		Dim newParentContainer As Control = Parent

		marcador.Location = Location
		marcador.Size = Size
		marcador.Anchor = Anchor

		If (_parentLevel > 0) Then
			For ipx As Integer = 0 To _parentLevel
				newParentContainer = newParentContainer.Parent
			Next
			_parentContainer.Controls.Remove(Me)
			newParentContainer.Controls.Add(Me)
		End If

		Dim newSize As Size = New Size(Parent.DisplayRectangle.Size.Width - Parent.Margin.Horizontal, Parent.DisplayRectangle.Size.Height - Parent.Margin.Vertical)
		Dim newLocation As Point = New Point(Parent.Margin.Left, Parent.Margin.Top)

		Size = newSize
		Location = newLocation

		IsExpanded = True
		BackColor = Color.FromKnownColor(KnownColor.Window)
		lblExpand.ImageIndex = 0
	End Sub

	Protected Overridable Sub Collapse()
		Dim newParentContainer As Control = Parent

		If (_parentLevel > 0) Then
			newParentContainer.Controls.Remove(Me)
			_parentContainer.Controls.Add(Me)
		End If

		Anchor = marcador.Anchor
		Location = marcador.Location
		Size = marcador.Size

		BackColor = Color.FromKnownColor(KnownColor.Info)
		lblExpand.ImageIndex = 1
		IsExpanded = False
	End Sub

	Private Sub fadeMovement(ByVal size As Size, ByVal location As Point)
		Dim fadeStep As Integer = 10
		Dim x As Integer
		Dim y As Integer
		Dim w As Integer
		Dim h As Integer

		While (Me.Location <> location OrElse Me.Size <> size)

			x = Me.Location.X
			y = Me.Location.Y

			If (x < location.X) Then
				x += fadeStep
				If (x > location.X) Then x = location.X
			ElseIf (x > location.X) Then
				x -= fadeStep
				If (x < location.X) Then x = location.X
			End If
			If (y < location.Y) Then
				y += fadeStep
				If (y > location.Y) Then y = location.Y
			ElseIf (y > location.Y) Then
				y -= fadeStep
				If (y < location.Y) Then y = location.Y
			End If
			'Me.Location = New Point(CInt(IIf(x > location.X, location.X, x)), CInt(IIf(y > location.Y, location.Y, y)))

			w = Me.Size.Width
			h = Me.Size.Height

			If (w < size.Width) Then
				w += 2
				If (w > size.Width) Then w = size.Width
			ElseIf (w > size.Width) Then
				w -= 2
				If (w < size.Width) Then w = size.Width
			End If
			If (h < size.Height) Then
				h += 2
				If (h > size.Height) Then h = size.Height
			ElseIf (h > size.Height) Then
				h -= 2
				If (h < size.Height) Then h = size.Height
			End If
			'Me.Size = New Size(CInt(IIf(w > size.Width, size.Width, w)), CInt(IIf(h > size.Height, size.Height, h)))

			Me.Location = New Point(x, y)
			Me.Size = New Size(w, h)

		End While
	End Sub

#End Region

#Region " Eventos del control "

	Private Sub lblExpand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblExpand.Click
		If (Not IsExpanded) Then
			Expand()
		Else
			Collapse()
		End If

		BringToFront()
	End Sub

	Private Sub lblExpand_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblExpand.MouseHover
		If (lblExpand.ImageList Is ImgList32) Then Exit Sub

		lblExpand.Size = New Size(32, 32)
		lblExpand.ImageList = ImgList32
		If (Not IsExpanded) Then
			lblExpand.ImageIndex = 0
		Else
			lblExpand.ImageIndex = 1
		End If
		lblExpand.Location = New Point(lblExpand.Location.X - 16, lblExpand.Location.Y)
		lblExpand.BringToFront()
		Application.DoEvents()
	End Sub

	Private Sub lblExpand_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblExpand.MouseLeave
		If (lblExpand.ImageList Is imgList16) Then Exit Sub

		lblExpand.Size = New Size(16, 16)
		lblExpand.ImageList = imgList16
		If (Not IsExpanded) Then
			lblExpand.ImageIndex = 0
		Else
			lblExpand.ImageIndex = 1
		End If
		lblExpand.Location = New Point(lblExpand.Location.X + 16, lblExpand.Location.Y)
		lblExpand.BringToFront()
		Application.DoEvents()
	End Sub

	Private Sub ExpandibleListView_ParentChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ParentChanged
		If (_parentContainer IsNot Nothing) Then
			_parentContainer.Controls.Add(marcador)
		End If
	End Sub

	Private Sub ExpandibleListView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		_parentContainer = Parent
		lblExpand.BringToFront()
	End Sub

#End Region

#End Region


End Class
