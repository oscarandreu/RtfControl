Imports System.Windows.Forms


Public Class frmFindAndReplace

	Private Shared Open As Boolean = False
	Private rchTxtBox As RichTextBox

	Public Sub New(ByVal rtb As RichTextBox, ByVal Replacement As Boolean)
		InitializeComponent()

		rchTxtBox = rtb
		If (Replacement) Then
			tabReemplazar.Select()
		End If
	End Sub

	Private Sub btnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFind.Click
		Dim StartPosition As Integer
		Dim SearchType As CompareMethod
		If chkMatchCase.Checked = True Then
			SearchType = CompareMethod.Binary
		Else
			SearchType = CompareMethod.Text
		End If
		StartPosition = InStr(1, rchTxtBox.Text, txtSearchTerm.Text, SearchType)
		If StartPosition = 0 Then
			MessageBox.Show("La cadena: " & txtSearchTerm.Text.ToString() & " not found", "No hay coincidencias", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
			Exit Sub
		End If
		rchTxtBox.Select(StartPosition - 1, txtSearchTerm.Text.Length)
		rchTxtBox.ScrollToCaret()
		rchTxtBox.Focus()
		Application.DoEvents()
	End Sub

	Private Sub btnFindNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNext.Click
		Dim StartPosition As Integer = rchTxtBox.SelectionStart + 2
		Dim SearchType As CompareMethod
		If chkMatchCase.Checked = True Then
			SearchType = CompareMethod.Binary
		Else
			SearchType = CompareMethod.Text
		End If
		StartPosition = InStr(StartPosition, rchTxtBox.Text, txtSearchTerm.Text, SearchType)
		If StartPosition = 0 Then
			MessageBox.Show("String: " & txtSearchTerm.Text.ToString() & " no encontrada", "No hay coincidencias", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
			Exit Sub
		End If
		rchTxtBox.Select(StartPosition - 1, txtSearchTerm.Text.Length)
		rchTxtBox.ScrollToCaret()
		rchTxtBox.Focus()
		Application.DoEvents()
	End Sub

	Private Sub btnFindReplacement_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindReplacement.Click
		Dim StartPosition As Integer
		Dim SearchType As CompareMethod
		If chkMatchCaseReplacement.Checked = True Then
			SearchType = CompareMethod.Binary
		Else
			SearchType = CompareMethod.Text
		End If
		StartPosition = InStr(1, rchTxtBox.Text, txtSearchTermToReplace.Text, SearchType)
		If StartPosition = 0 Then
			MessageBox.Show("La cadena: '" & txtSearchTermToReplace.Text.ToString() & "' no encontrada", "No Matches", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
			Exit Sub
		End If
		rchTxtBox.Select(StartPosition - 1, txtSearchTermToReplace.Text.Length)
		rchTxtBox.ScrollToCaret()
		rchTxtBox.Focus()
		Application.DoEvents()
	End Sub

	Private Sub btnFindNextReplacement_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNextReplacement.Click
		Dim StartPosition As Integer = rchTxtBox.SelectionStart + 2
		Dim SearchType As CompareMethod
		If chkMatchCaseReplacement.Checked = True Then
			SearchType = CompareMethod.Binary
		Else
			SearchType = CompareMethod.Text
		End If
		StartPosition = InStr(StartPosition, rchTxtBox.Text, txtSearchTermToReplace.Text, SearchType)
		If StartPosition = 0 Then
			MessageBox.Show("La cadena: '" & txtSearchTermToReplace.Text.ToString() & "' not found", "No Matches", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
			Exit Sub
		End If
		rchTxtBox.Select(StartPosition - 1, txtSearchTermToReplace.Text.Length)
		rchTxtBox.ScrollToCaret()
		rchTxtBox.Focus()
		Application.DoEvents()
	End Sub

	Private Sub btnReplace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplace.Click
		If rchTxtBox.SelectedText.Length <> 0 Then
			rchTxtBox.SelectedText = txtReplacementText.Text
		End If
		Dim StartPosition As Integer = rchTxtBox.SelectionStart + 2
		Dim SearchType As CompareMethod
		If chkMatchCaseReplacement.Checked = True Then
			SearchType = CompareMethod.Binary
		Else
			SearchType = CompareMethod.Text
		End If
		StartPosition = InStr(StartPosition, rchTxtBox.Text, txtSearchTermToReplace.Text, SearchType)
		If StartPosition = 0 Then
			MessageBox.Show("La cadena: '" & txtSearchTermToReplace.Text.ToString() & "' not found", "No Matches", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
			Exit Sub
		End If
		rchTxtBox.Select(StartPosition - 1, txtSearchTermToReplace.Text.Length)
		rchTxtBox.ScrollToCaret()
		rchTxtBox.Focus()
		Application.DoEvents()
	End Sub

	Private Sub btnReplaceAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplaceAll.Click
		Dim currentPosition As Integer = rchTxtBox.SelectionStart
		Dim currentSelect As Integer = rchTxtBox.SelectionLength
		rchTxtBox.Rtf = Replace(rchTxtBox.Rtf, Trim(txtSearchTermToReplace.Text), Trim(txtReplacementText.Text))
		rchTxtBox.SelectionStart = currentPosition
		rchTxtBox.SelectionLength = currentSelect
		rchTxtBox.Focus()
		Application.DoEvents()
	End Sub

	Private Sub frmFindAndReplace_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If (Open) Then
			MessageBox.Show("Sólo puede relizar una busqueda cada vez, cierre la busqueda activa.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
			RemoveHandler MyBase.FormClosing, AddressOf frmFindAndReplace_FormClosing
			Close()
		Else
			Open = True
		End If
	End Sub

	Private Sub frmFindAndReplace_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
		Open = False
	End Sub

End Class